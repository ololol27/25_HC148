using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;
using System.IO;

public class PyramidAgent : Agent
{
    public GameObject area;
    PyramidArea m_MyArea;
    Rigidbody m_AgentRb;

    public SwitchManager switchManager;
    public bool useVectorObs;

    public List<Mobility> linkedMobilities = new List<Mobility>();

    private static int episodeCounter = 0;
    private float episodeStartTime;
    private string logPath;
    private int stepCount;

    private Vector3 initialPos;
    private Quaternion initialRot;

    private List<Vector3> visitedSwitches = new List<Vector3>();

    // 보상 값 (고정)
    private float goalReward = 50f;
    private float switchReward = 10f;

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_MyArea = area.GetComponent<PyramidArea>();
        if (!switchManager) switchManager = FindFirstObjectByType<SwitchManager>();

        string logDir = Path.Combine(Application.dataPath, "../Logs");
        if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);
        logPath = Path.Combine(logDir, "switch_log.txt");

        initialPos = transform.position;
        initialRot = transform.rotation;
    }

        //수동조작코드
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActions = actionsOut.DiscreteActions;

        // 기본: 0 = 아무것도 안 함
        int action = 0;

        if (Input.GetKey(KeyCode.W))
            action = 1; // 앞으로
        else if (Input.GetKey(KeyCode.S))
            action = 2; // 뒤로
        else if (Input.GetKey(KeyCode.D))
            action = 3; // 오른쪽 회전
        else if (Input.GetKey(KeyCode.A))
            action = 4; // 왼쪽 회전

        discreteActions[0] = action;
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        if (!useVectorObs) return;

        Vector3 localVelocity = transform.InverseTransformDirection(m_AgentRb.linearVelocity);
        Vector3 targetPos = (switchManager && switchManager.currentTarget)
            ? switchManager.currentTarget.position
            : transform.position;

        Vector3 localPosToTarget = transform.InverseTransformPoint(targetPos);

        sensor.AddObservation(localPosToTarget);
        sensor.AddObservation(localVelocity);
        sensor.AddObservation(switchManager && switchManager.currentIndex >= switchManager.numSwitches ? 1f : 0f);
    }

    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        var action = act[0];
        switch (action)
        {
            case 1: dirToGo = transform.forward; break;
            case 2: dirToGo = -transform.forward; break;
            case 3: rotateDir = transform.up; break;
            case 4: rotateDir = -transform.up; break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 200f);
        m_AgentRb.AddForce(dirToGo * 2f, ForceMode.VelocityChange);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        AddReward(-1f / MaxStep); // step penalty
        stepCount++;
        MoveAgent(actionBuffers.DiscreteActions);
    }

    public override void OnEpisodeBegin()
    {
        episodeCounter++;
        episodeStartTime = Time.realtimeSinceStartup;

        m_MyArea.CleanPyramidArea();
        m_AgentRb.linearVelocity = Vector3.zero;
        m_AgentRb.angularVelocity = Vector3.zero;
        transform.position = initialPos;
        transform.rotation = initialRot;

        //foreach (var mob in linkedMobilities) if (mob != null) mob.ResetMobility();

        if (!switchManager) switchManager = FindFirstObjectByType<SwitchManager>();
        switchManager.StartSequence();

        stepCount = 0;
        visitedSwitches.Clear();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            AddReward(-2f);   // 벽 충돌
        }

        if (collision.gameObject.CompareTag("mobility"))
        {
            AddReward(-3f);   // Mobility 충돌
        }

        if (collision.gameObject.CompareTag("goal"))
        {
            AddReward(goalReward);  // Goal 보상
            LogEpisodeSwitches(true);
            //foreach (var mob in linkedMobilities) if (mob != null) mob.ResetMobility();
            EndEpisode();
        }
    }

    // SwitchManager에서 호출
    public void RecordSwitch(Vector3 switchPos)
    {
        visitedSwitches.Add(switchPos);
        AddReward(switchReward); // 고정값 보상
        Debug.Log($"[Agent] Switch touched: {switchPos}, +{switchReward}");
    }

    // GUI로 에피소드 정보 출력
    void OnGUI()
    {
        float elapsed = Time.realtimeSinceStartup - episodeStartTime;
        float efficiency = GetCumulativeReward() / Mathf.Max(1, stepCount);

        GUILayout.BeginArea(new Rect(10, 10, 400, 400));
        GUILayout.Label($"Episode Number: {episodeCounter}");
        GUILayout.Label($"Time: {elapsed:F2} sec");
        GUILayout.Label($"Step Count: {stepCount}");
        GUILayout.Label($"Reward: {GetCumulativeReward():F2}");
        GUILayout.Label($"Efficiency: {efficiency:F3}");

        if (visitedSwitches.Count > 0)
        {
            GUILayout.Label("Switch:");
            foreach (var pos in visitedSwitches)
            {
                GUILayout.Label($"  ({pos.x:F1}, {pos.y:F1}, {pos.z:F1})");
            }
        }
        GUILayout.EndArea();
    }

    private void LogEpisodeSwitches(bool isFinal)
    {
        using (StreamWriter writer = new StreamWriter(logPath, true))
        {
            float elapsed = Time.realtimeSinceStartup - episodeStartTime;
            float efficiency = GetCumulativeReward() / Mathf.Max(1, stepCount);

            writer.WriteLine($"==== Episode {episodeCounter} ====");
            writer.WriteLine($"Time: {elapsed:F2} sec");
            writer.WriteLine($"Reward: {GetCumulativeReward():F2}");
            writer.WriteLine($"StepCount: {stepCount}");
            writer.WriteLine($"Efficiency: {efficiency:F3}");

            if (visitedSwitches.Count > 0)
            {
                writer.WriteLine("Switch:");
                foreach (var pos in visitedSwitches)
                {
                    writer.WriteLine($"  ({pos.x:F1}, {pos.y:F1}, {pos.z:F1})");
                }
            }

            if (isFinal) writer.WriteLine("== Episode End ==\n");
        }
    }
}

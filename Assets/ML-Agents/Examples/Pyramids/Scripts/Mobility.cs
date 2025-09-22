using UnityEngine;
using Unity.MLAgents;

public class Mobility : MonoBehaviour
{
    public enum MoveMode { Rectangle, PingPong } // 이동 모드 선택 가능
    [Header("Mode Settings")]
    public MoveMode moveMode = MoveMode.Rectangle;

    [Header("Path Settings")]
    public float width = 20f;       
    public float height = 60f;      
    public float moveSpeed = 20f;   
    public float reachThreshold = 0.2f; 
    public bool clockwise = true;   
    public bool startWithWidth = true; 

    [Header("PingPong Settings")]
    public float moveDistance = 20f;      
    public Vector3 moveDirection = Vector3.forward; // 왕복 방향

    [Header("Linked Agent")]
    public Agent linkedAgent;   // 연결된 Agent (Inspector에서 직접 지정)

    private Vector3[] pathPoints;
    private int currentPoint = 0;
    private Rigidbody rb;

    private Vector3 initialPos;
    private Quaternion initialRot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        initialPos = transform.position;
        initialRot = transform.rotation;

        BuildPath(initialPos);
    }

    void FixedUpdate()
    {
        if (pathPoints == null || pathPoints.Length == 0) return;

        Vector3 target = pathPoints[currentPoint];
        Vector3 direction = (target - transform.position).normalized;

        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, target) < reachThreshold)
        {
            if (moveMode == MoveMode.PingPong)
            {
                currentPoint = (currentPoint + 1) % 2; // 두 지점 왕복
            }
            else
            {
                currentPoint = (currentPoint + 1) % pathPoints.Length;
            }
        }
    }

    // 경로 생성
    private void BuildPath(Vector3 startPos)
    {
        if (moveMode == MoveMode.PingPong)
        {
            pathPoints = new Vector3[2];
            pathPoints[0] = startPos;
            pathPoints[1] = startPos + moveDirection.normalized * moveDistance;

            // 위쪽(Z > 0) → 아래쪽부터, 아래쪽(Z < 0) → 위쪽부터
            if (moveDirection == Vector3.forward || moveDirection == Vector3.back)
            {
                if (startPos.z > 0) currentPoint = 1;
                else currentPoint = 1;
            }
            else
            {
                currentPoint = 1;
            }
        }
        else
        {
            pathPoints = new Vector3[4];

            if (clockwise)
            {
                if (startWithWidth)
                {
                    pathPoints[0] = startPos;
                    pathPoints[1] = startPos + new Vector3(width, 0, 0);
                    pathPoints[2] = startPos + new Vector3(width, 0, height);
                    pathPoints[3] = startPos + new Vector3(0, 0, height);
                }
                else
                {
                    pathPoints[0] = startPos;
                    pathPoints[1] = startPos + new Vector3(0, 0, height);
                    pathPoints[2] = startPos + new Vector3(width, 0, height);
                    pathPoints[3] = startPos + new Vector3(width, 0, 0);
                }
            }
            else
            {
                if (startWithWidth)
                {
                    pathPoints[0] = startPos;
                    pathPoints[1] = startPos + new Vector3(width, 0, 0);
                    pathPoints[2] = startPos + new Vector3(width, 0, -height);
                    pathPoints[3] = startPos + new Vector3(0, 0, -height);
                }
                else
                {
                    pathPoints[0] = startPos;
                    pathPoints[1] = startPos + new Vector3(0, 0, height);
                    pathPoints[2] = startPos + new Vector3(-width, 0, height);
                    pathPoints[3] = startPos + new Vector3(-width, 0, 0);
                }
            }
        }
    }

    // 자기 Agent가 에피소드 끝낼 때만 초기화됨
    public void ResetMobility()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.position = initialPos;
        transform.rotation = initialRot;

        currentPoint = 0;
        BuildPath(initialPos);
    }
}

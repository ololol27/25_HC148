using System.Linq;
using UnityEngine;

/// <summary>
/// 스위치 N개를 순서대로 밟게 하고, 마지막에는 Goal을 활성화.
/// - 에피소드 시작 시 StartSequence() 호출
/// - currentTarget을 에이전트 관측에 사용
/// </summary>
public class SwitchManager : MonoBehaviour
{
    [Header("Refs (Inspector에서 반드시 연결)")]
    public PyramidArea area;        // 스폰 구역 제공
    public PyramidAgent agent;      // 사용 중인 에이전트
    public Transform goal;          // 고정 위치 Goal (씬에 배치 후 Inspector 연결)
    public GameObject switchPrefab; // PyramidSwitch 스크립트 포함 프리팹

    [Header("Settings")]
    [Range(0, 5)]                   // ✅ 0~5개 범위로 제한
    public int numSwitches = 3;     // 스위치 개수

    [Header("Runtime (read-only)")]
    public Transform currentTarget; // 현재 목표(스위치 or goal)
    public int currentIndex = -1;   // -1: 시작 전, 0..N-1: 스위치 단계, N: goal 단계

    // --- private ---
    GameObject _aliveSwitch;
    int[] _switchSpawnIdx;
    bool _busy; // 중복 처리 방지

    void Awake()
    {
        // Inspector 연결 확인
        if (!area || !agent || !goal || !switchPrefab)
        {
            Debug.LogError($"[SwitchManager] {gameObject.name} 참조 누락! (Area/Agent/Goal/SwitchPrefab 확인)");
            enabled = false;
        }

        // 시작 시 goal은 숨김
        if (goal) goal.gameObject.SetActive(false);
    }

    /// <summary>에피소드 시작 시 호출</summary>
    public void StartSequence()
    {
        _busy = false;

        // 기존 스위치 제거 & goal 숨기기
        if (_aliveSwitch) { Destroy(_aliveSwitch); _aliveSwitch = null; }
        HideGoalCompletely();

        // 스위치 스폰 위치 랜덤(중복 없음)
        int n = area.spawnAreas.Length;
        _switchSpawnIdx = Enumerable.Range(0, n)
                                    .OrderBy(_ => System.Guid.NewGuid())
                                    .Take(numSwitches)
                                    .ToArray();

        currentIndex = -1;
        NextStage();
    }

    /// <summary>스위치가 눌릴 때 PyramidSwitch에서 호출</summary>
    public void OnSwitchPressed(PyramidSwitch pressed)
    {
        if (_busy) return;
        if (!_aliveSwitch || pressed == null || pressed.gameObject != _aliveSwitch) return;

        // ❌ 중복 보상 호출 제거 (PyramidSwitch에서 이미 처리)
        _busy = true;
        Destroy(_aliveSwitch);
        _aliveSwitch = null;

        NextStage();
        _busy = false;
    }

    // --- 내부 동작 ---
    void NextStage()
    {
        currentIndex++;

        if (currentIndex < numSwitches)
        {
            SpawnSwitch(currentIndex);
        }
        else
        {
            ActivateGoal();
        }
    }

    void SpawnSwitch(int stage)
    {
        if (_aliveSwitch) { Destroy(_aliveSwitch); _aliveSwitch = null; }

        _aliveSwitch = Instantiate(switchPrefab, area.transform); // Area 자식으로 생성
        var sw = _aliveSwitch.GetComponent<PyramidSwitch>();
        if (sw) sw.manager = this;

        area.PlaceObject(_aliveSwitch, _switchSpawnIdx[stage]);   // 랜덤 위치 배치
        currentTarget = _aliveSwitch.transform;
    }

    void ActivateGoal()
    {
        if (!goal)
        {
            Debug.LogError("[SwitchManager] goal 미지정. Inspector에서 연결하세요.");
            return;
        }

        // Inspector에 배치된 고정 위치에서 활성화
        goal.tag = "goal";
        foreach (var r in goal.GetComponentsInChildren<Renderer>(true)) r.enabled = true;
        foreach (var c in goal.GetComponentsInChildren<Collider>(true)) c.enabled = true;

        goal.gameObject.SetActive(true);
        currentTarget = goal;
    }

    void HideGoalCompletely()
    {
        if (!goal) return;
        goal.gameObject.SetActive(false); // 비활성화 (위치는 그대로 둠 = 고정 위치)
    }
}

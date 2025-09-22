using UnityEngine;
using Unity.MLAgents;

public class PyramidSwitch : MonoBehaviour
{
    public Material onMaterial;
    public Material offMaterial;
    public GameObject myButton;
    [HideInInspector] public SwitchManager manager;

    bool m_State;
    PyramidArea m_AreaComponent;

    public bool GetState() => m_State;

    void Start()
    {
        if (!manager) manager = FindFirstObjectByType<SwitchManager>();
        m_AreaComponent = manager ? manager.area : FindFirstObjectByType<PyramidArea>();

        // 안전: 어떤 부모 밑에 생성돼도 동작하도록 부모 영향 제거 가능
        transform.SetParent(null);
    }

    public void ResetSwitch(int spawnAreaIndex, int _unused)
    {
        if (!m_AreaComponent) m_AreaComponent = FindFirstObjectByType<PyramidArea>();
        m_AreaComponent.PlaceObject(gameObject, spawnAreaIndex);

        m_State = false;
        tag = "switchOff";
        myButton.GetComponent<Renderer>().material = offMaterial;
    }

    void OnCollisionEnter(Collision other)
    {
        if (m_State) return;
        if (!other.gameObject.CompareTag("agent")) return;

        // 스위치 ON
        myButton.GetComponent<Renderer>().material = onMaterial;
        m_State = true;
        tag = "switchOn";

        // ✅ 보상은 Agent에 맡김
        var ag = other.gameObject.GetComponent<PyramidAgent>();
        if (ag) ag.RecordSwitch(transform.position);

        // 다음 단계로
        if (!manager) manager = FindFirstObjectByType<SwitchManager>();
        manager.OnSwitchPressed(this);
    }
}

using UnityEngine;
using Unity.MLAgentsExamples;

public class PyramidArea : Area
{
    public GameObject pyramid;
    public GameObject stonePyramid;
    public GameObject[] spawnAreas;
    public int numPyra;
    public float range;

    /*
    public void CreatePyramid(int numObjects, int spawnAreaIndex)
    {
        CreateObject(numObjects, pyramid, spawnAreaIndex);
    }
    */

    /*
    public void CreateStonePyramid(int numObjects, int spawnAreaIndex)
    {
        CreateObject(numObjects, stonePyramid, spawnAreaIndex);
    }
    */

    void CreateObject(int numObjects, GameObject desiredObject, int spawnAreaIndex)
    {
        for (var i = 0; i < numObjects; i++)
        {
            var newObject = Instantiate(desiredObject, Vector3.zero,
                Quaternion.Euler(0f, 0f, 0f), transform);
            PlaceObject(newObject, spawnAreaIndex);
        }
    }

    public void PlaceObject(GameObject objectToPlace, int spawnAreaIndex)
{
    var spawnTransform = spawnAreas[spawnAreaIndex].transform;

    // 1) Collider 있으면 정확한 바운드로, 없으면 lossyScale로 범위 계산
    float xRange, zRange, yBase;
    var col = spawnTransform.GetComponent<Collider>();
    if (col != null)
    {
        var b = col.bounds;
        xRange = b.extents.x * 0.95f;
        zRange = b.extents.z * 0.95f;
        yBase  = b.center.y; // 바닥 높이 근처
    }
    else
    {
        xRange = spawnTransform.lossyScale.x * 0.5f * 0.95f;
        zRange = spawnTransform.lossyScale.z * 0.5f * 0.95f;
        yBase  = spawnTransform.position.y;
    }

    // 2) 랜덤 위치(약간 여유)
    var pos = new Vector3(
        Random.Range(-xRange, xRange),
        0f,
        Random.Range(-zRange, zRange)
    ) + new Vector3(spawnTransform.position.x, yBase, spawnTransform.position.z);

    // 3) 살짝 띄워주기(바닥 파고드는 거 방지)
    var objectHeight = objectToPlace.transform.lossyScale.y;
    pos.y += objectHeight / 2f;

    objectToPlace.transform.SetPositionAndRotation(pos, Quaternion.identity);
}


    public void CleanPyramidArea()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("pyramid"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    public override void ResetArea()
    {
    }
}

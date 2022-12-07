using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour
{
    private BuildingSO BuildObjectSO;
    private Vector2Int origin;
    public static BuildObject Create(Vector3 worldPosition, Vector2Int origin, BuildingSO BuildSO)
    {
        Transform BuildObjectTransform = Instantiate(BuildSO.prefab, worldPosition, Quaternion.identity);
        BuildObject buildObject = BuildObjectTransform.GetComponent<BuildObject>();
        Debug.Log(BuildSO);
        buildObject.BuildObjectSO = BuildSO;
        buildObject.origin = origin;

        return buildObject;
    }
    public List<Vector2Int> GetGridPositionList()
    {
        return BuildObjectSO.GetGridPositionList(origin);

    }
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static BuildingSystem;

public class GameManager : MonoBehaviour
{
    private GridSystem<GridObject> buildgrid;
    private GridSystem<PathNode> nodeGrid;

    private Pathfinding pathfinding;
    private BuildingSystem buildingSystem;

    [SerializeField] private List<BuildingSO> BuildingList;
    [SerializeField] GameObject GroundPrefab;
    [SerializeField] private Soldier Soldier;

    Camera camera;
    private void Awake()
    {
        camera = Camera.main;

        Vector3 GridOrigin = camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 CameraTopLeft = camera.ScreenToWorldPoint(new Vector3(0, camera.pixelHeight, camera.nearClipPlane));
        Vector3 CameraBottomRight = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0, camera.nearClipPlane));

        float cellSize = 1.6f;

        int gridWidth = Mathf.FloorToInt((CameraBottomRight.x * 2) / cellSize);
        int gridHeight = Mathf.FloorToInt((CameraTopLeft.y * 2) / cellSize);


        pathfinding = new Pathfinding(gridWidth, gridHeight, cellSize, GridOrigin);
        buildingSystem = new BuildingSystem(gridWidth, gridHeight, cellSize, GridOrigin,BuildingList);

        InstantiateGround(pathfinding.GetGrid().GetAllPositions());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            buildingSystem.InstanceBuild(mousePosition);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            buildingSystem.DestroyBuild(mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            buildingSystem.SelectBuild(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            buildingSystem.SelectBuild(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Soldier.SetTargetPosition(GetMouseWorldPosition());
        }
    }

    void InstantiateGround(List<Vector3> GroundPositions) 
    {
        Transform GroundParent = new GameObject("GroundParent").transform;
        GroundParent.position = Vector3.zero;
        foreach (Vector3 pos in GroundPositions)
        {
            Instantiate(GroundPrefab,pos,Quaternion.identity,GroundParent.transform);
        }

    }


    Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = camera.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0;
        return vec;
    }
}

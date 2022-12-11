using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using static BuildingSystem;

public class GameManager : MonoBehaviour
{
    private GridSystem<GridObject> buildgrid;
    private GridSystem<PathNode> nodeGrid;

    //public UIManager UIManager;
    private Pathfinding pathfinding;
    private BuildingSystem buildingSystem;
    private ObjectPool objectPool;
    Soldier SelectedSoldier;
  
    [SerializeField] private List<BuildingSO> BuildingList;
    private BuildingSO buildingSO;
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
        objectPool = ObjectPool.Instance;
        InstantiateGround(pathfinding.GetGrid().GetAllPositions());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            if (buildingSO!=null)
            {
                buildingSystem.InstanceBuild(mousePosition,buildingSO);
                buildingSO= null;
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null)
                {
                    switch (hit.collider.tag)
                    {
                        case "Soldier":
                            SelectedSoldier = hit.collider.GetComponent<Soldier>();
                            break;
                        case "Build":
                            hit.collider.GetComponent<BuildObject>().OnClick();
                            break;
                    }
                }
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            if (SelectedSoldier!=null)
            {
                Vector3 mousePosition = GetMouseWorldPosition();
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider!=null&&hit.collider.GetComponent<IAttackableObject>()!=null)
                {
                    SelectedSoldier.SetTarget(hit.collider.GetComponent<IAttackableObject>());
                }
                else
                {
                    SelectedSoldier.SetTargetPosition(mousePosition);
                }
            }
            //buildingSystem.DestroyBuild(mousePosition);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            buildingSO = BuildingList[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            buildingSO = BuildingList[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Soldier.SetTargetPosition(GetMouseWorldPosition());
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            objectPool.SpawnObject(ObjectTypes.SoldierA,Vector3.zero);
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

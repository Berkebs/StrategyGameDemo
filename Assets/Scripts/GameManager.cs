using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    private Pathfinding pathfinding;
    private BuildingSystem buildingSystem;
    Soldier SelectedSoldier;

    
  
    [SerializeField] private List<BuildingSO> BuildingList;
    private BuildingSO buildingSO;
    [SerializeField] GameObject GroundPrefab;
    private GameObject BuildGhost;


    [SerializeField] Color CantBuildColor;
    [SerializeField] Color CanBuildColor;


    Camera camera;


    private void Awake()
    {
        camera = Camera.main;
        Instance= this;
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

    private void Update()
    {
        if (buildingSO != null)
        {
            GetGhost();
        }


        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            if (buildingSO!=null)
            {
                buildingSystem.InstanceBuild(mousePosition,buildingSO);

                if (BuildGhost != null)
                {
                    DisableGhost(BuildGhost);
                }
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
                            UIManager.Instance.DeselectBarrack();

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
                if (hit.collider!=null&&hit.collider.GetComponent<IAttackableObject>()!=null&&hit.collider.GetComponent<Soldier>()!=SelectedSoldier)
                {
                    SelectedSoldier.SetTarget(hit.collider.GetComponent<IAttackableObject>());
                }
                else
                {
                    SelectedSoldier.SetTargetPosition(mousePosition);
                }
            }
            else if (BuildGhost!=null)
            {
                DisableGhost(BuildGhost);
            }
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

    void GetGhost() 
    {    
        Vector3 mousePosition = GetMouseWorldPosition();

        if (BuildGhost != null)
        {
            
            Vector3 targetPosition = GetMouseWorldSnappedPosition(mousePosition);

            BuildGhost.transform.position = Vector3.Lerp(BuildGhost.transform.position, targetPosition, Time.deltaTime * 15f);
            if (buildingSystem.CanBuild(mousePosition, buildingSO))
            {
                BuildGhost.GetComponent<SpriteRenderer>().color = CanBuildColor;
            }
            else
            {
                BuildGhost.GetComponent<SpriteRenderer>().color = CantBuildColor;

            }
        }
        else
        {
            BuildGhost = EnableGhost(mousePosition);
        }
    }

    GameObject EnableGhost(Vector3 mousePosition) 
    {
       return ObjectPool.Instance.SpawnObject(buildingSO.ObjectType, mousePosition);
    }
    void DisableGhost(GameObject Ghost) 
    {
        Ghost.GetComponent<SpriteRenderer>().color= Color.white;
        Ghost.transform.position = new Vector3(-2000,0);
        Ghost.SetActive(false);
        BuildGhost = null;

        buildingSO = null;
    }


    Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = camera.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0;
        return vec;
    }
    public Vector3 GetMouseWorldSnappedPosition(Vector3 mousePosition)
    {
        BuildingSystem.Instance.buildgrid.GetXY(mousePosition, out int x, out int y);
        Vector3 placedObjectWorldPosition = BuildingSystem.Instance.buildgrid.GetWorldPosition(x, y);
        return placedObjectWorldPosition;
    }
    public void SetSelectedBuilding(BuildingSO SelectedBuildingSO) 
    {
        buildingSO = SelectedBuildingSO;
    }

    public List<BuildingSO> GetBuildingList() 
    {
        return BuildingList;
    }
}

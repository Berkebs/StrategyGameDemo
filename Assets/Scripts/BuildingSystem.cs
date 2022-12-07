using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{

    public static BuildingSystem Instance { get; private set; }
    private Pathfinding pathfinding;
    [SerializeField] private Soldier Soldier;


    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    [SerializeField] private List<BuildingSO> BuildingList;
    private BuildingSO BuildingSO;

    private GridSystem<GridObject> grid;
    Camera camera;

    private void Awake()
    {
        camera = Camera.main;
        Instance = this;

        Vector3 GridOrigin = camera.ScreenToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        Vector3 CameraTopLeft = camera.ScreenToWorldPoint(new Vector3(0, camera.pixelHeight, camera.nearClipPlane));
        Vector3 CameraBottomRight = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0, camera.nearClipPlane));

        float cellSize = 1.6f;

        int gridWidth = Mathf.FloorToInt((CameraBottomRight.x * 2) / cellSize);
        int gridHeight = Mathf.FloorToInt((CameraTopLeft.y * 2) / cellSize);


        pathfinding = new Pathfinding(gridWidth, gridHeight,cellSize,GridOrigin);
        grid = new GridSystem<GridObject>(gridWidth, gridHeight, cellSize, GridOrigin, (GridSystem<GridObject> g, int x, int y) => new GridObject(g, x, y));
        
        BuildingSO = null;
    }

    public void SelectBuild(int BuildIndex)
    {
        BuildingSO = BuildingList[BuildIndex];
        RefreshSelectedObjectType();
    }
    public class GridObject
    {
        private GridSystem<GridObject> grid;
        private int x;
        private int y;
        private BuildObject buildObject;
        public GridObject(GridSystem<GridObject> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;

        }
        public void SetBuildObject(BuildObject _buildObject)
        {
            this.buildObject = _buildObject;
            grid.TriggerGridObjectChanged(x, y);
        }
        public void ClearBuildObject()
        {
            this.buildObject = null;
            grid.TriggerGridObjectChanged(x, y);

        }

        public BuildObject GetBuildObject()
        {
            return buildObject;
        }
        public bool CanBuild()
        {
            return buildObject == null;
        }
        public override string ToString()
        {
            return x + "," + y + "\n" + buildObject;
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            grid.GetXY(mousePosition, out int x, out int y);

            Vector2Int placedObjectOrigin = new Vector2Int(x, y);
            List<Vector2Int> gridPositionList = BuildingSO.GetGridPositionList(placedObjectOrigin);

            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Debug.Log(BuildingSO);
                BuildObject buildObject = BuildObject.Create(grid.GetWorldPosition(x, y), new Vector2Int(x, y), BuildingSO);

                Vector3 buildObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    Debug.Log("Ttatat");
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetBuildObject(buildObject);
                    SetGridNodeWalkable(gridPosition,false);
                }

                OnObjectPlaced?.Invoke(this, EventArgs.Empty);
                DeselectObjectType();

            }
            else
            {
                Debug.LogError("Cannot Build Here!!!");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePosition = GetMouseWorldPosition();

            if (grid.GetGridObject(mousePosition) != null)
            {
                // Valid Grid Position
                BuildObject placedObject = grid.GetGridObject(mousePosition).GetBuildObject();
                if (placedObject != null)
                {
                    // Demolish
                    placedObject.DestroySelf();

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).ClearBuildObject();
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectBuild(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectBuild(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetSoldierTargetPosition(Soldier, GetMouseWorldPosition());
        }
    }

    void SetSoldierTargetPosition(Soldier soldier,Vector3 mousePosition) 
    {
        pathfinding.GetGrid().GetXY(mousePosition, out int x, out int y);
        List<PathNode> path = pathfinding.FindPath(0, 0, x, y);
        soldier.SetTargetPosition(mousePosition);
    }
    void SetGridNodeWalkable(Vector2Int mousePosition,bool isWalkable) 
    {
        // pathfinding.GetGrid().GetXY(mousePosition, out int x, out int y);
        Debug.LogError("111");
        Debug.Log(pathfinding.GetGrid().GetGridObject(mousePosition.x,mousePosition.y).isWalkable);

        pathfinding.GetGrid().GetGridObject(mousePosition.x, mousePosition.y).SetIsWalkable(isWalkable);
        //pathfinding.GetNode(x, y).SetIsWalkable(isWalkable);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0;
        return vec;
    }

    Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        grid.GetXY(mousePosition, out int x, out int y);

        if (BuildingSO != null)
        {
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y);
            Debug.Log(placedObjectWorldPosition);
            return placedObjectWorldPosition;
        }
        else
        {
            Debug.Log(mousePosition);
            return mousePosition;
        }
    }
    private void DeselectObjectType()
    {
        BuildingSO = null; 
        RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        grid.GetXY(worldPosition, out int x, out int y);
        return new Vector2Int(x, y);
    }

    public BuildingSO GetPlacedObjectTypeSO()
    {
        return BuildingSO;
    }
}

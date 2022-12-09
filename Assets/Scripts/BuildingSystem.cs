using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{

    public static BuildingSystem Instance { get; private set; }


    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    private List<BuildingSO> BuildingList;
    private BuildingSO BuildingSO;

    private GridSystem<GridObject> buildgrid;
    private GridSystem<PathNode> nodeGrid;

    public BuildingSystem(int width, int height, float cellSize, Vector3 gridOrigin,List<BuildingSO> Builds) 
    {

        Instance = this;
        buildgrid = new GridSystem<GridObject>(width, height, cellSize, gridOrigin, (GridSystem<GridObject> g, int x, int y) => new GridObject(g, x, y));
        BuildingList= Builds;
        BuildingSO = null;

    }

 

    public void SelectBuild(int BuildIndex)
    {
        Debug.Log(BuildingList[BuildIndex]);
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

    public void InstanceBuild(Vector3 mousePosition) 
    {
        if (BuildingSO!=null)
        {
            buildgrid.GetXY(mousePosition, out int x, out int y);

            Vector2Int placedObjectOrigin = new Vector2Int(x, y);
            List<Vector2Int> gridPositionList = BuildingSO.GetGridPositionList(placedObjectOrigin);
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                if (!buildgrid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }
            if (canBuild)
            {
                Debug.Log(BuildingSO);
                BuildObject buildObject = BuildObject.Create(buildgrid.GetWorldPosition(x, y), new Vector2Int(x, y), BuildingSO);
                Vector3 buildObjectWorldPosition = buildgrid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y);
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    buildgrid.GetGridObject(gridPosition.x, gridPosition.y).SetBuildObject(buildObject);
                    SetGridNodeWalkable(gridPosition, false);
                }
                OnObjectPlaced?.Invoke(this, EventArgs.Empty);
                DeselectObjectType();
            }
            else
            {
                Debug.LogError("Cannot Build Here!!!");
            }
        }
    }
    public void DestroyBuild(Vector3 mousePosition) 
    {
        if (buildgrid.GetGridObject(mousePosition) != null)
        {
            // Valid Grid Position
            BuildObject placedObject = buildgrid.GetGridObject(mousePosition).GetBuildObject();
            if (placedObject != null)
            {
                // Demolish
                placedObject.DestroySelf();

                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    buildgrid.GetGridObject(gridPosition.x, gridPosition.y).ClearBuildObject();
                }
            }
        }
    }
    void SetGridNodeWalkable(Vector2Int mousePosition,bool isWalkable) 
    {
        Pathfinding.Instance.GetGrid().GetGridObject(mousePosition.x, mousePosition.y).SetIsWalkable(isWalkable);
    }


   /* public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        buildgrid.GetXY(mousePosition, out int x, out int y);

        if (BuildingSO != null)
        {
            Vector3 placedObjectWorldPosition = buildgrid.GetWorldPosition(x, y);
            Debug.Log(placedObjectWorldPosition);
            return placedObjectWorldPosition;
        }
        else
        {
            Debug.Log(mousePosition);
            return mousePosition;
        }
    }*/
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
        buildgrid.GetXY(worldPosition, out int x, out int y);
        return new Vector2Int(x, y);
    }

    public BuildingSO GetPlacedObjectTypeSO()
    {
        return BuildingSO;
    }
}

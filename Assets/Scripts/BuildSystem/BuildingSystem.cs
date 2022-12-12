using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem
{

    public static BuildingSystem Instance { get; private set; }



    public GridSystem<GridObject> buildgrid;

    //Instance Build System
    public BuildingSystem(int width, int height, float cellSize, Vector3 gridOrigin,List<BuildingSO> Builds) 
    {
        
        Instance = this;
        buildgrid = new GridSystem<GridObject>(width, height, cellSize, gridOrigin, (GridSystem<GridObject> g, int x, int y) => new GridObject(g, x, y));

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
    }

    public void InstanceBuild(Vector3 mousePosition,BuildingSO buildingSO) 
    {

        buildgrid.GetXY(mousePosition, out int x, out int y);

        //Get Object width and height

        Vector2Int placedObjectOrigin = new Vector2Int(x, y);
        List<Vector2Int> gridPositionList = buildingSO.GetGridPositionList(placedObjectOrigin);
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (!buildgrid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                canBuild = false;
                break;
            }
        }

        //Check the coordinates of this object if there are any other objects

        if (canBuild)
        {
            BuildObject buildObject = BuildObject.Create(buildgrid.GetWorldPosition(x, y), new Vector2Int(x, y), buildingSO);
            foreach (Vector2Int gridPosition in gridPositionList)
            {
                buildgrid.GetGridObject(gridPosition.x, gridPosition.y).SetBuildObject(buildObject);
                SetGridNodeWalkable(gridPosition, false);
            }
        }
        
    }

    //Check the coordinates of this object if there are any other objects for Ghost.

    public bool CanBuild(Vector3 mousePosition, BuildingSO buildingSO) 
    {
        buildgrid.GetXY(mousePosition, out int x, out int y);

        Vector2Int placedObjectOrigin = new Vector2Int(x, y);
        List<Vector2Int> gridPositionList = buildingSO.GetGridPositionList(placedObjectOrigin);
        bool canBuild = true;
        foreach (Vector2Int gridPosition in gridPositionList)
        {
            if (!buildgrid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
            {
                canBuild = false;
                break;
            }
        }
        return canBuild;

    }
    public void DestroyBuild(Vector2Int BuildOrigin) 
    {
        if (buildgrid.GetGridObject(BuildOrigin.x,BuildOrigin.y) != null)
        {
            // Valid Grid Position
            BuildObject placedObject = buildgrid.GetGridObject(BuildOrigin.x,BuildOrigin.y).GetBuildObject();
            if (placedObject != null)
            {
               

                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    buildgrid.GetGridObject(gridPosition.x, gridPosition.y).ClearBuildObject();
                    SetGridNodeWalkable(gridPosition, true);
                }

                // Demolish
                placedObject.DestroySelf();
            }
        }
    }

    void SetGridNodeWalkable(Vector2Int mousePosition,bool isWalkable) 
    {
        Pathfinding.Instance.GetGrid().GetGridObject(mousePosition.x, mousePosition.y).SetIsWalkable(isWalkable);
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        buildgrid.GetXY(worldPosition, out int x, out int y);
        return new Vector2Int(x, y);
    }

    public Vector3 GetGridWorldPosition(Vector2Int ObjectOrigin) 
    {
        return buildgrid.GetWorldPosition(ObjectOrigin.x,ObjectOrigin.y);
    }

}

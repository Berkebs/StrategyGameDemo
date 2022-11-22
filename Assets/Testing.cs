using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Pathfinding pathfinding;
    [SerializeField] private Soldier Soldier;
    private void Start()
    {
        pathfinding = new Pathfinding(10,10);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPosition=GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition,out int x,out int y) ;
            List<PathNode> path = pathfinding.FindPath(0,0,x,y);
            if (path!=null)
            {
                for(int i=0;i<path.Count-1;i++)
                {
                    Debug.Log(path[i]);
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
                }
            }
            Soldier.SetTargetPosition(mouseWorldPosition);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition,out int x,out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x,y).isWalkable);
        }
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
    /*
    GridMap<GridMapObject> grid;

    private void Start()
    {
        grid = new GridMap<GridMapObject>(6,2,5f,new Vector3(-15,-8),(GridMap<GridMapObject> g,int x,int y)=> new GridMapObject(g,x,y));

       /* List<bool> boolList=new List<bool>();
        List<int> intList=new List<int>();*/
    /*  }
      public void Update()
      {
          Vector3 position = GetMouseWorldPosition();

          if (Input.GetMouseButtonDown(0))
          {
              GridMapObject gridmapObject= grid.GetGridObject(position);
              if (gridmapObject!=null)
              {
                  gridmapObject.AddValue(5);
              }
          }
      }

    

      public class GridMapObject 
      {
          private const int MIN = 0;
          private const int MAX = 100;

          private GridMap<GridMapObject> grid;
          private int x;
          private int y;
          private int value;


          public GridMapObject(GridMap<GridMapObject> grid,int x,int y) 
          {
              this.x= x;
              this.y= y;
              this.grid = grid;
          }
          public void AddValue(int addValue) 
          {
              value += addValue;
              value=Mathf.Clamp(value,MIN,MAX);
              grid.TriggerGridObjectChanged(x,y);
          }

          public float GetValueNormalized() 
          {
              return (float)value / MAX;
          }

          public override string ToString()
          {
              return value.ToString();
          }
      }*/
}

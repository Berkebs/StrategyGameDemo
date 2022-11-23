using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private Pathfinding pathfinding;
    [SerializeField] private Soldier Soldier;

    public GameObject Barracks;
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
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);

            Instantiate(Barracks, pathfinding.grid.GetGridCenterPosition(x, y),Quaternion.identity);
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
}



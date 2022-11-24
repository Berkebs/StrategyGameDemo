using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapVisual : MonoBehaviour
{
    private GridMap<Tilemap.TileMapObject> grid;
    private Mesh mesh;
    private bool updateMesh;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(GridMap<Tilemap.TileMapObject> grid)
    { 
        this.grid = grid;
        UpdateHeatMapVisual();
        grid.onGridObjectChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, GridMap<Tilemap.TileMapObject>.OnGridObjectChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = true;
            UpdateHeatMapVisual();
        }
    }

    private void UpdateHeatMapVisual() 
    {
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth()*grid.GetHeight(),out Vector3[] vertices,out Vector2[] uv,out int[] triangles);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int index=x*grid.GetHeight()+y;
                Vector3 quadSize=new Vector3(1,1)*grid.GetCellSize();

                Tilemap.TileMapObject gridObject = grid.GetGridObject(x,y);
                Tilemap.TileMapObject.TilemapSprite tilemapSprite = gridObject.GetTilemapSprite();
                Vector2 gridValueUV;
                if (tilemapSprite==Tilemap.TileMapObject.TilemapSprite.None)
                {
                    gridValueUV = Vector2.zero;
                    quadSize = Vector3.zero;
                }
                else
                {
                    gridValueUV = Vector2.one;
                }
                MeshUtils.AddToMeshArrays(vertices,uv,triangles,index,grid.GetWorldPosition(x,y)+quadSize*.5f,0f,quadSize,Vector2.zero,Vector2.zero);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap
{
    private GridMap<TileMapObject> grid;
    public Tilemap(int width, int height, float cellSize, Vector3 originPosition)
    {
        grid = new GridMap<TileMapObject>(width, height, cellSize, originPosition, (GridMap<TileMapObject> g, int x, int y) => new TileMapObject(grid,x,y));
    }

    public void SetTilemapSprite(Vector3 worldPosition,TileMapObject.TilemapSprite tilemapSprite) 
    {
        TileMapObject tilemapobject = grid.GetGridObject(worldPosition);
        if (tilemapobject!=null)
        {
            tilemapobject.SetTilemapSprite(tilemapSprite);
        }
    }

    public class TileMapObject
    {
        public enum TilemapSprite 
        {
            None,
            Ground
        }

        private GridMap<TileMapObject> grid;
        private int x;
        private int y;
        private TilemapSprite tilemapSprite;

        public TileMapObject(GridMap<TileMapObject> grid,int x,int y) 
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetTilemapSprite(TilemapSprite tilemapSprite) 
        {

            this.tilemapSprite = tilemapSprite;
            grid.TriggerGridObjectChanged(x,y);


        }

        public override string ToString()
        {
            return tilemapSprite.ToString();
        }

    }

}

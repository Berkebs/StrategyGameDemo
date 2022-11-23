using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridMap<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> onGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs 
    {
        public int x;
        public int y;
    }


    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    public GridMap(int width, int height,float cellSize,Vector3 originPosition,Func<GridMap<TGridObject>,int,int,TGridObject> createGridObject) 
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        gridArray= new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = true;
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                     debugTextArray[x,y] = CreateWorldText(gridArray[x , y].ToString(),null, GetGridCenterPosition(x,y), 20,Color.white,TextAnchor.MiddleCenter);
                     Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x,y+1), Color.white, 1000f);
                     Debug.DrawLine(GetWorldPosition(x,y),GetWorldPosition(x+1,y),Color.white,1000f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            onGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }



    }
    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * cellSize+originPosition;
    }

    public Vector3 GetGridCenterPosition(int x,int y)
    {
        return GetWorldPosition(x,y) + new Vector3(cellSize, cellSize) * .5f;
    }

    public void GetXY(Vector3 worldPosition,out int x,out int y) 
    {
        x=Mathf.FloorToInt((worldPosition-originPosition).x/cellSize);
        y = Mathf.FloorToInt((worldPosition-originPosition).y / cellSize);

    }

    public void SetGridObject(int x,int y,TGridObject value) 
    {
        if (x>=0&&y>=0&&x<width&&y<height)
        {
            gridArray[x, y] = value;
            // debugTextArray[x,y].text= gridArray[x, y].ToString();
            if (onGridObjectChanged!=null)
            {
                onGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
            }
        }
    }

    public void TriggerGridObjectChanged(int x,int y) 
    {
        if (onGridObjectChanged != null)
        {
            onGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void SetGridObject(Vector3 worldPosition,TGridObject value) 
    {
        int x, y;
        GetXY(worldPosition,out x,out y);
        SetGridObject(x,y,value);
    }
    public TGridObject GetGridObject(int x,int y) 
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];

        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition) 
    {
        int x, y;
        GetXY(worldPosition,out x,out y);
        return GetGridObject(x,y);
    }
    TextMesh CreateWorldText(string text,Transform parent,Vector3 localPosition,int fontSize,Color color,TextAnchor textanchor) 
    {
        GameObject gameObject= new GameObject("World_Text",typeof(TextMesh));
        Transform transform= gameObject.transform;
        transform.SetParent(parent,false);
        transform.localPosition=localPosition;
        TextMesh textMesh=gameObject.GetComponent<TextMesh>();
        textMesh.anchor= textanchor;
        textMesh.text= text;
        textMesh.fontSize = fontSize;
        textMesh.color= color;
        return textMesh;
    }
}

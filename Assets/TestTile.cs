using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTile : MonoBehaviour
{
    [SerializeField] private TilemapVisual tilemapVisual;
    private Tilemap tilemap;
    
    // Start is called before the first frame update
    void Start()
    {
        tilemap = new Tilemap(20, 10, 10f, Vector3.zero);

        tilemap.SetTilemapVisual(tilemapVisual);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector3 mouseWorldPosition = GetMouseWorldPosition();

            tilemap.SetTilemapSprite(mouseWorldPosition,Tilemap.TileMapObject.TilemapSprite.Ground);
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

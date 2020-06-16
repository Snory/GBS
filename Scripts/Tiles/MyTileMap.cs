using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MyTileMap : MonoBehaviour
{
    // Start is called before the first frame update
    //https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.html
    [SerializeField]
    Tilemap _tileMap;

    [SerializeField]
    TileBase _base;

    Vector2 _mouseInWorldPosition;

    [SerializeField]
    GameObject _test;
    Camera _main;

    private void Awake()
    {
        _main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

 
        Vector3 mouseInWorld3 = _main.ScreenToWorldPoint(Input.mousePosition);
      

        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int tileCoordinatesInGrid = _tileMap.WorldToCell(new Vector3(mouseInWorld3.x, mouseInWorld3.y, _tileMap.origin.z));
            //_tileMap.SetTileFlags(tileCoordinatesInGrid, TileFlags.None);
            //_tileMap.SetTile(tileCoordinatesInGrid, _base);




        }


    }


}

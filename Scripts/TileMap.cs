using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class TileMap : MonoBehaviour
{
    // Start is called before the first frame update
    //https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.html
    [SerializeField]
    Tilemap _tileMap;

    Vector2 _mouseInWorldPosition;

    [SerializeField]
    GameObject _test;

    void Start()
    {
        Debug.Log(_tileMap.size);
        Debug.Log(_tileMap.cellSize);

        for(int i = _tileMap.origin.x; i < _tileMap.origin.x + _tileMap.size.x; i++)
        {
            for(int j = _tileMap.origin.y; j < _tileMap.origin.y + _tileMap.size.y; j++)
            {
                Debug.Log($"Checking tile at {i}:{j}");
                TileBase tile = _tileMap.GetTile(new Vector3Int(i, j, 0));
                if(tile != null)
                {
                    Debug.Log($"Found tile at {i}:{j}");
                    //https://docs.unity3d.com/ScriptReference/Tilemaps.TileFlags.html
                    _tileMap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                    _tileMap.SetColor(new Vector3Int(i, j, 0), Color.red);
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseInWorldPosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

            Vector3Int tile = _tileMap.WorldToCell(new Vector3(_mouseInWorldPosition.x, _mouseInWorldPosition.y, 0));

            Instantiate(_test, _tileMap.GetCellCenterLocal(tile), Quaternion.identity);
            
            
        }
    }
}

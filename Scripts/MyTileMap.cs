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

    void Start()
    {
        Debug.Log(_tileMap.size);
        Debug.Log(_tileMap.cellSize);

        
        for(int i = _tileMap.origin.x; i < _tileMap.origin.x + _tileMap.size.x; i++)
        {
            for(int j = _tileMap.origin.y; j < _tileMap.origin.y + _tileMap.size.y; j++)
            {
                Debug.Log($"Checking tile at {i}:{j}");
                Vector3Int tileGridCoordination = new Vector3Int(i, j, 0);
                TileBase tile = _tileMap.GetTile(tileGridCoordination);
                AStarTile astartile = _tileMap.GetTile<AStarTile>(tileGridCoordination);
                if (tile != null)
                {
                    Debug.Log($"Found tile at {i}:{j}");

                    //https://docs.unity3d.com/ScriptReference/Tilemaps.TileFlags.html
                    //_tileMap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                    //_tileMap.SetColor(new Vector3Int(i, j, 0), Color.red);
                }

                if (astartile != null)
                {
                    Debug.Log($"Found astar tile at {i}:{j}");

                    astartile.GridCoordination = tileGridCoordination;
                    astartile.TileMap = _tileMap;

                    //https://docs.unity3d.com/ScriptReference/Tilemaps.TileFlags.html
                    //_tileMap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                    //_tileMap.SetColor(new Vector3Int(i, j, 0), Color.red);
                }

            }
        }
        

    }

    // Update is called once per frame
    void Update()
    {

        /**
        Vector3 mouseInWorld3 = _main.ScreenToWorldPoint(Input.mousePosition);
        _mouseInWorldPosition = new Vector2(mouseInWorld3.x,mouseInWorld3.y);
        Vector3Int tileCoordinatesInGrid = _tileMap.WorldToCell(new Vector3(_mouseInWorldPosition.x, _mouseInWorldPosition.y, _tileMap.origin.z));
        _tileMap.SetTileFlags(tileCoordinatesInGrid, TileFlags.None);
        //_tileMap.SetColor(tileCoordinatesInGrid, Color.black);
        _tileMap.SetTile(tileCoordinatesInGrid, _base); 

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.GetRayIntersection(_main.ScreenPointToRay(Input.mousePosition));

            if(hit.collider != null)
            {
                Debug.Log($"Hit at {hit.collider.transform.position}");
            } else
            {
                Debug.Log($"Nothing");
            }
        }
        **/
        
    }


}

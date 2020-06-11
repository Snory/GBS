using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;



public class PathFinding : MonoBehaviour
{
    [SerializeField]
    Tilemap _walkableTileMap;

    [SerializeField]
    Transform _start, _end;
    HexTile[,] _tiles;

    List<HexTile> path;

    [SerializeField]
    int _range = 0;



    public void Start()
    {
        _tiles = new HexTile[_walkableTileMap.size.x, _walkableTileMap.size.y];

        for (int i = _walkableTileMap.origin.x; i < _walkableTileMap.origin.x + _walkableTileMap.size.x; i++)
        {
            for (int j = _walkableTileMap.origin.y; j < _walkableTileMap.origin.y + _walkableTileMap.size.y; j++)
            {
                Vector3Int tileGridCoordination = new Vector3Int(i, j, 0);
                TileBase tileBase = _walkableTileMap.GetTile(tileGridCoordination);
   
                if (tileBase != null)
                {
                    HexTile currentTile = new HexTile();              
                        
                    currentTile.GridCoordination = tileGridCoordination;
                    currentTile.TileMap = _walkableTileMap;
                    _tiles[i + Math.Abs(_walkableTileMap.origin.x), j + Math.Abs(_walkableTileMap.origin.y)] = currentTile;

                    //https://docs.unity3d.com/ScriptReference/Tilemaps.TileFlags.html
                    //_tileMap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                    //_tileMap.SetColor(new Vector3Int(i, j, 0), Color.red);
                }

            }
        }        
    }

    private void Update()
    {
        _walkableTileMap.RefreshAllTiles();

  
        if (_start != null &&_end != null)
        {
            FindPath(_start.position, _end.position);
            Vector3Int startTilePosition = _walkableTileMap.WorldToCell(_start.position);
            Vector3Int endTilePosition = _walkableTileMap.WorldToCell(_end.position);

            HexTile startTile = _tiles[startTilePosition.x + Math.Abs(_walkableTileMap.origin.x), startTilePosition.y + Math.Abs(_walkableTileMap.origin.y)];
            HexTile endTile = _tiles[endTilePosition.x + Math.Abs(_walkableTileMap.origin.x), endTilePosition.y + Math.Abs(_walkableTileMap.origin.y)];

            path = RetracePath(startTile, endTile);
            if(path != null)
            {
                foreach (HexTile tile in path)
                {
                    _walkableTileMap.SetTileFlags(tile.GridCoordination, TileFlags.None); 
                    _walkableTileMap.SetColor(tile.GridCoordination, Color.red);
                }
            }
        }

  
       // OnClickShowNeighbour();
    }



    //for test only
    private void OnClickShowNeighbour()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseInWorld3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tileCoordinatesInGrid = _walkableTileMap.WorldToCell(new Vector3(mouseInWorld3.x, mouseInWorld3.y, _walkableTileMap.origin.z));
            HexTile tile = _tiles[tileCoordinatesInGrid.x + Math.Abs(_walkableTileMap.origin.x), tileCoordinatesInGrid.y + Math.Abs(_walkableTileMap.origin.y)];

            //_tileMap.SetTileFlags(tileCoordinatesInGrid, TileFlags.None);
            //_tileMap.SetTile(tileCoordinatesInGrid, _base);

            
            _walkableTileMap.SetTileFlags(tile.GridCoordination, TileFlags.None);
            _walkableTileMap.SetColor(tile.GridCoordination, Color.red);

            List<Vector3Int> neihbors = tile.GetNeighborCoordinationsInDistance(_range);

    
            foreach(Vector3Int neighbourCoordination in neihbors)
            {

                HexTile neighbour = _tiles[neighbourCoordination.x + Math.Abs(_walkableTileMap.origin.x), neighbourCoordination.y + Math.Abs(_walkableTileMap.origin.y)];
                _walkableTileMap.SetTileFlags(neighbour.GridCoordination, TileFlags.None);
                _walkableTileMap.SetColor(neighbour.GridCoordination, Color.red);
            }
            

        }
    }


    public void FindPath(Vector3 startPostion, Vector3 endPosition)
    {



        Vector3Int startTilePosition = _walkableTileMap.WorldToCell(startPostion);
        Vector3Int endTilePosition = _walkableTileMap.WorldToCell(endPosition);

        HexTile startTile = _tiles[startTilePosition.x + Math.Abs(_walkableTileMap.origin.x), startTilePosition.y + Math.Abs(_walkableTileMap.origin.y)];
        HexTile endTile = _tiles[endTilePosition.x + Math.Abs(_walkableTileMap.origin.x), endTilePosition.y + Math.Abs(_walkableTileMap.origin.y)];

        Heap<HexTile> openSet = new Heap<HexTile>(_tiles.Length);
        HashSet<HexTile> closedSet = new HashSet<HexTile>();

        openSet.Add(startTile);


        while (openSet.Count > 0)
        {

            //find note in the open set with lowest fcost
            HexTile currentTile = openSet.RemoveFirst();
            closedSet.Add(currentTile);

             if (currentTile == endTile)
            {

                return; 
            }

            Debug.Log("Before foreach");
            //musime najit sousedy
            foreach(Vector3Int neighbourCoordination in currentTile.GetNeighborCoordinationsInDistance(1))
            {
                Debug.Log("foreach");
                HexTile neighbour = _tiles[neighbourCoordination.x + Math.Abs(_walkableTileMap.origin.x), neighbourCoordination.y + Math.Abs(_walkableTileMap.origin.y)];
                //walkable, close list
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = (int)currentTile.GCost + (int)currentTile.GetDistanceToCoordination(neighbourCoordination);

                if(newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = (int) neighbour.GetDistanceToCoordination(endTilePosition);
                    neighbour.Parent = currentTile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);


                    }
                }

            }

            Debug.Log("After foreach");



        }
    }


    private List<HexTile> RetracePath(HexTile startTile, HexTile endTile)
    {
        List<HexTile> path = new List<HexTile>();
        HexTile currentTile = endTile;

        while(currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
        }
        path.Add(currentTile);
        path.Reverse();

        return path;
    }


}

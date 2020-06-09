using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;


public enum TileType {SQUARE,HEX}

public class PathFinding : MonoBehaviour
{
    [SerializeField]
    Tilemap _walkableTileMap;

    [SerializeField]
    Transform _start, _end;
    IPathFindable[,] _tiles;

    List<IPathFindable> path;

    [SerializeField]
    TileType _tileType;

    [SerializeField]
    int _range = 0;

    public void Start()
    {
        _tiles = new IPathFindable[_walkableTileMap.size.x, _walkableTileMap.size.y];

        for (int i = _walkableTileMap.origin.x; i < _walkableTileMap.origin.x + _walkableTileMap.size.x; i++)
        {
            for (int j = _walkableTileMap.origin.y; j < _walkableTileMap.origin.y + _walkableTileMap.size.y; j++)
            {
                Vector3Int tileGridCoordination = new Vector3Int(i, j, 0);
                TileBase tileBase = _walkableTileMap.GetTile(tileGridCoordination);
   
                if (tileBase != null)
                {
                    IPathFindable currentTile = null;

                    switch (_tileType)
                    {
                        case TileType.SQUARE:
                            currentTile = new SquareTile();
                            break;
                        case TileType.HEX:
                            currentTile = new HexTile();
                            break;
                    }
                        
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
        //_walkableTileMap.RefreshAllTiles();

  
        if (_start != null &&_end != null)
        {
            FindPath(_start.position, _end.position);
            Vector3Int startTilePosition = _walkableTileMap.WorldToCell(_start.position);
            Vector3Int endTilePosition = _walkableTileMap.WorldToCell(_end.position);

            IPathFindable startTile = _tiles[startTilePosition.x + Math.Abs(_walkableTileMap.origin.x), startTilePosition.y + Math.Abs(_walkableTileMap.origin.y)];
            IPathFindable endTile = _tiles[endTilePosition.x + Math.Abs(_walkableTileMap.origin.x), endTilePosition.y + Math.Abs(_walkableTileMap.origin.y)];

            path = RetracePath(startTile, endTile);
            if(path != null)
            {
                foreach (IPathFindable tile in path)
                {
                    _walkableTileMap.SetTileFlags(tile.GridCoordination, TileFlags.None); 
                    _walkableTileMap.SetColor(tile.GridCoordination, Color.red);
                }
            }
        }

  
        OnClickShowNeighbour();
    }



    //for test only
    private void OnClickShowNeighbour()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseInWorld3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tileCoordinatesInGrid = _walkableTileMap.WorldToCell(new Vector3(mouseInWorld3.x, mouseInWorld3.y, _walkableTileMap.origin.z));
            IPathFindable tile = _tiles[tileCoordinatesInGrid.x + Math.Abs(_walkableTileMap.origin.x), tileCoordinatesInGrid.y + Math.Abs(_walkableTileMap.origin.y)];

            //_tileMap.SetTileFlags(tileCoordinatesInGrid, TileFlags.None);
            //_tileMap.SetTile(tileCoordinatesInGrid, _base);

            
            _walkableTileMap.SetTileFlags(tile.GridCoordination, TileFlags.None);
            _walkableTileMap.SetColor(tile.GridCoordination, Color.red);

            List<Vector3Int> neihbors = tile.GetNeighborCoordinationsInDistance(_range);

    
            foreach(Vector3Int neighbourCoordination in neihbors)
            {

                IPathFindable neighbour = _tiles[neighbourCoordination.x + Math.Abs(_walkableTileMap.origin.x), neighbourCoordination.y + Math.Abs(_walkableTileMap.origin.y)];
                _walkableTileMap.SetTileFlags(neighbour.GridCoordination, TileFlags.None);
                _walkableTileMap.SetColor(neighbour.GridCoordination, Color.red);
            }
            

        }
    }


    public void FindPath(Vector3 startPostion, Vector3 endPosition)
    {


        Vector3Int startTilePosition = _walkableTileMap.WorldToCell(startPostion);
        Vector3Int endTilePosition = _walkableTileMap.WorldToCell(endPosition);

        IPathFindable startTile = _tiles[startTilePosition.x + Math.Abs(_walkableTileMap.origin.x), startTilePosition.y + Math.Abs(_walkableTileMap.origin.y)];
        IPathFindable endTile = _tiles[endTilePosition.x + Math.Abs(_walkableTileMap.origin.x), endTilePosition.y + Math.Abs(_walkableTileMap.origin.y)];

        List<IPathFindable> openSet = new List<IPathFindable>();
        HashSet<IPathFindable> closedSet = new HashSet<IPathFindable>();

        openSet.Add(startTile);


        while (openSet.Count > 0)
        {

            //find note in the open set with lowest fcost
            IPathFindable currentTile = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].FCost < currentTile.FCost || openSet[i].FCost == currentTile.FCost && openSet[i].HCost < currentTile.HCost)
                { 
                    currentTile = openSet[i]; 
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

             if (currentTile == endTile)
            {

                return; 
            }

            //musime najit sousedy
            foreach(Vector3Int neighbourCoordination in currentTile.GetNeighborCoordinationsInDistance(1))
            {
                IPathFindable neighbour = _tiles[neighbourCoordination.x + Math.Abs(_walkableTileMap.origin.x), neighbourCoordination.y + Math.Abs(_walkableTileMap.origin.y)];
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



        }
    }


    private List<IPathFindable> RetracePath(IPathFindable startTile, IPathFindable endTile)
    {
        List<IPathFindable> path = new List<IPathFindable>();
        IPathFindable currentTile = endTile;

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

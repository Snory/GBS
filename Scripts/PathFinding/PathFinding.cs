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


    AStarTile[,] _tiles;

    private void Update()
    {
        if(_start != null &&_end != null)
        {
            FindPath(_start.position, _end.position);
        }
    }

    public void Start()
    {
        _tiles = new AStarTile[_walkableTileMap.size.x, _walkableTileMap.size.y];

        for (int i = _walkableTileMap.origin.x; i < _walkableTileMap.origin.x + _walkableTileMap.size.x; i++)
        {
            for (int j = _walkableTileMap.origin.y; j < _walkableTileMap.origin.y + _walkableTileMap.size.y; j++)
            {
                Vector3Int tileGridCoordination = new Vector3Int(i, j, 0);
                TileBase tile = _walkableTileMap.GetTile(tileGridCoordination);
   
                if (tile != null)
                {

                    AStarTile aStarTile = new AStarTile();

                    aStarTile.GridCoordination = tileGridCoordination;
                    aStarTile.TileMap = _walkableTileMap;

                    _tiles[i + Math.Abs(_walkableTileMap.origin.x), j + Math.Abs(_walkableTileMap.origin.y)] = aStarTile;

                    //https://docs.unity3d.com/ScriptReference/Tilemaps.TileFlags.html
                    //_tileMap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                    //_tileMap.SetColor(new Vector3Int(i, j, 0), Color.red);
                }

            }
        }

        
    }


    public void FindPath(Vector3 startPostion, Vector3 endPosition)
    {


        Vector3Int startTilePosition = _walkableTileMap.WorldToCell(startPostion);
        Vector3Int endTilePosition = _walkableTileMap.WorldToCell(endPosition);

        AStarTile startTile = _tiles[startTilePosition.x + Math.Abs(_walkableTileMap.origin.x), startTilePosition.y + Math.Abs(_walkableTileMap.origin.y)];
        AStarTile endTile = _tiles[endTilePosition.x + Math.Abs(_walkableTileMap.origin.x), endTilePosition.y + Math.Abs(_walkableTileMap.origin.y)];

        List<AStarTile> openSet = new List<AStarTile>();
        HashSet<AStarTile> closedSet = new HashSet<AStarTile>();

        openSet.Add(startTile);

        _walkableTileMap.SetTileFlags(startTile.GridCoordination, TileFlags.None);
        _walkableTileMap.SetColor(startTile.GridCoordination, Color.blue);

        while (openSet.Count > 0)
        {

            //find note in the open set with lowest fcost
            AStarTile currentTile = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].FCost < currentTile.FCost || openSet[i].FCost == currentTile.FCost && openSet[i].HCost < currentTile.HCost)
                { 
                    currentTile = openSet[i]; 
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            _walkableTileMap.SetTileFlags(currentTile.GridCoordination, TileFlags.None);
            _walkableTileMap.SetColor(currentTile.GridCoordination, Color.red);

            if (currentTile == endTile)
            {

                return; 
            }

            //musime najit sousedy
            foreach(Vector3Int neighbourCoordination in currentTile.GetNeighborCoordinations())
            {
                AStarTile neighbour = _tiles[neighbourCoordination.x + Math.Abs(_walkableTileMap.origin.x), neighbourCoordination.y + Math.Abs(_walkableTileMap.origin.y)];
                //walkable, close list
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentTile.GCost + GetDistance(currentTile, neighbour);

                if(newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = GetDistance(neighbour, endTile);
                    neighbour.Parent = currentTile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);

                        _walkableTileMap.SetTileFlags(neighbour.GridCoordination, TileFlags.None);
                        _walkableTileMap.SetColor(neighbour.GridCoordination, Color.blue);
                    }
                }

            }



        }
    }


    private void RetracePath(AStarTile startTile, AStarTile endTile)
    {
        List<AStarTile> path = new List<AStarTile>();
        AStarTile currentTile = endTile;

        while(currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
        }
        path.Reverse();
    }

    private int GetDistance(AStarTile a, AStarTile b)
    {
        int dstx = Math.Abs(a.GridCoordination.x - b.GridCoordination.x);
        int dsty = Math.Abs(a.GridCoordination.y - b.GridCoordination.y);

        if(dstx > dsty)
        {
            return 14 * dsty + 10 * (dstx - dsty);
        } else
        {
            return 14 * dstx + 10 * (dsty - dstx);
        }


    }
}

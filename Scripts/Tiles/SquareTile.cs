using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SquareTile : IPathFindable { 

    public int FCost { get { return GCost + HCost; } }
    public int GCost { get; set; }
    public int HCost { get; set; }

    public Vector3Int GridCoordination { get; set; }


    public IPathFindable Parent { get; set; }
    public Vector3 WorldCoordination { get; set; }

    public float GetDistanceToCoordination(Vector3Int a)
    {
        int dstx = Math.Abs(this.GridCoordination.x - a.x);
        int dsty = Math.Abs(this.GridCoordination.y - a.y);

        if (dstx > dsty)
        {
            return 14 * dsty + 10 * (dstx - dsty);
        }
        else
        {
            return 14 * dstx + 10 * (dsty - dstx);
        }
    }

    public List<Vector3Int> GetNeighborCoordinationsInDistance(int range)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = GridCoordination.x + x;
                int checkY = GridCoordination.y + y;

                //check if it is inside of our tilemap;
                TileBase neighbor = PathFinding.Instance.WalkableTileMap.GetTile(new Vector3Int(checkX, checkY, PathFinding.Instance.WalkableTileMap.origin.z));   
                
                if(neighbor != null)
                {
                    neighbors.Add(new Vector3Int(checkX, checkY, PathFinding.Instance.WalkableTileMap.origin.z));
                }

                
            }
        }

        return neighbors;
    }
}

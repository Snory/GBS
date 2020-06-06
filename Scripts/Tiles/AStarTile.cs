using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarTile { 

    public int FCost { get { return GCost + HCost; } }
    public int GCost { get; set; }
    public int HCost { get; set; }

    public Vector3Int GridCoordination { get; set; }

    public Tilemap TileMap { get; set; }

    public AStarTile Parent { get; set; }


    public List<Vector3Int> GetNeighborCoordinations()
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
                TileBase neighbor = TileMap.GetTile(new Vector3Int(checkX, checkY, TileMap.origin.z));   
                
                if(neighbor != null)
                {
                    neighbors.Add(new Vector3Int(checkX, checkY, TileMap.origin.z));
                }

                
            }
        }

        return neighbors;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarTile : Tile, ITilePathFindable
{

    public int FCost { get { return GCost + HCost; } }
    public int GCost { get; set; }
    public int HCost { get; set; }

    public Vector3Int GridCoordination { get; set; }

    public Tilemap TileMap { get; set; }


    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);


    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);

    }

    [MenuItem("Assets/Create/Tile/AStartTile")]
    public static void CreateTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save AStar Tile", "New AStar Tile", "asset", "Save AStar Tile", "Assets");
        if (path == "")
            return;

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AStarTile>(), path);
    }

    public List<AStarTile> GetNeighbor()
    {
        List<AStarTile> neighbors = new List<AStarTile>();

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
                AStarTile neighbor = TileMap.GetTile<AStarTile>(new Vector3Int(checkX, checkY, TileMap.origin.z));   
                
                if(neighbor != null)
                {
                    neighbors.Add(neighbor);
                }

                
            }
        }

        return neighbors;
    }
}

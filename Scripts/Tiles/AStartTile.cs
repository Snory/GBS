using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStartTile : Tile
{
    private int _fcost;
    private int _gcost;
    private int _hcost;

    public int FCost { get { return _fcost; } set { _fcost = value; } }
    public int GCost { get { return _gcost; } set { _gcost = value; } }
    public int HCost { get { return _hcost; } set { _hcost = value; } }


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

        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<AStartTile>(), path);
    }
}

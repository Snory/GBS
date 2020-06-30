using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexGrid : MonoBehaviour
{


    [SerializeField]
    public Tilemap Tilemap;

    public HexTile[,] Tiles { get; private set; }



    private void Awake()
    {
        Tiles = new HexTile[Tilemap.size.x, Tilemap.size.y];

        for (int i = Tilemap.origin.x; i < Tilemap.origin.x + Tilemap.size.x; i++)
        {
            for (int j = Tilemap.origin.y; j < Tilemap.origin.y + Tilemap.size.y; j++)
            {
                Vector3Int tileGridCoordination = new Vector3Int(i, j, 0);
                TileBase tileBase = Tilemap.GetTile(tileGridCoordination);

                if (tileBase != null)
                {
                    HexTile currentTile = new HexTile();

                    currentTile.GridCoordination = tileGridCoordination;
                    currentTile.WorldCoordination = Tilemap.CellToWorld(tileGridCoordination);
                    Tiles[i + Math.Abs(Tilemap.origin.x), j + Math.Abs(Tilemap.origin.y)] = currentTile;

                    //https://docs.unity3d.com/ScriptReference/Tilemaps.TileFlags.html
                    //_tileMap.SetTileFlags(new Vector3Int(i, j, 0), TileFlags.None);
                    //_tileMap.SetColor(new Vector3Int(i, j, 0), Color.red);
                }

            }
        }
    }


    public void SetHexTileOccupantOnWorldPosition(GameObject occupant, Vector3 worldPosition)
    {
        HexTile occupantTile = GetHexTileOnWorldPosition(worldPosition);
        occupantTile.OccupiedBy = occupant;

        if(occupant == null)
        {
            Tilemap.RefreshAllTiles();
        }
        else
        {
            Tilemap.SetTileFlags(occupantTile.GridCoordination, TileFlags.None);
            Tilemap.SetColor(occupantTile.GridCoordination, Color.red);

          
        }
    }

    public GameObject GetHexTileOccupantOnWorldPosition(Vector3 worldPostion)
    {
        return GetHexTileOnWorldPosition(worldPostion).OccupiedBy;
    }



    public HexTile GetHexTileOnWorldPosition(Vector3 worldPostion)
    {
        HexTile tile = null;

        Vector3 mouseInWorld3 = worldPostion;
        Vector3Int tileCoordinatesInGrid = Tilemap.WorldToCell(new Vector3(mouseInWorld3.x, mouseInWorld3.y, Tilemap.origin.z));

        int checkX = tileCoordinatesInGrid.x + Math.Abs(Tilemap.origin.x);
        int checkY = tileCoordinatesInGrid.y + Math.Abs(Tilemap.origin.y);


        if(checkX < Tilemap.size.x && checkY < Tilemap.size.y) { 
            tile = Tiles[checkX, checkY];
        }
        return tile;
    }

    public HexTile GetHexTileOnGridPosition(Vector3Int gridPosition)
    {
        HexTile tile = null;

        int checkX = gridPosition.x + Math.Abs(Tilemap.origin.x);
        int checkY = gridPosition.y + Math.Abs(Tilemap.origin.y); 

        if (checkX < Tilemap.size.x && checkY < Tilemap.size.y)
        {
            tile = Tiles[checkX, checkY];
        }
        return tile;
    }
}

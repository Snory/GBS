using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface ITilePathFindable
{
    public int FCost { get; }
    public int GCost { get; set; }
    public int HCost { get; set; }

    public Vector3Int GridCoordination { get; set; }

    public Tilemap TileMap { get; set; }
}

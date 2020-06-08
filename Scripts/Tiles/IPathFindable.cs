using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IPathFindable
{
    int FCost { get; }
    int GCost { get; set; }
    int HCost { get; set; }

    Vector3Int GridCoordination { get; set; }

    Tilemap TileMap { get; set; }

    IPathFindable Parent { get; set; }


    List<Vector3Int> GetNeighborCoordinations(int range);

    float GetDistanceTo(IPathFindable a);
}

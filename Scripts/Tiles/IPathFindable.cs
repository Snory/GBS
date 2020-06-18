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
    Vector3 WorldCoordination { get; set; }

    IPathFindable Parent { get; set; }


    List<Vector3Int> GetNeighborCoordinationsInDistance(int distance);

    float GetDistanceToCoordination(Vector3Int a);
}

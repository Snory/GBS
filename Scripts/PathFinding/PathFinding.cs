using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    [SerializeField]
    Tilemap _walableTileMap;

    AStartTile _aStartTile;

    public void Start()
    {
        //iterate tilemap and create tile with f,g and h cost
    }


    public void FindPath(Vector3 startPostion, Vector3 endPosition)
    {
        Vector3Int startTile = _walableTileMap.WorldToCell(startPostion);
        Vector3Int endTile = _walableTileMap.WorldToCell(endPosition);
    }
}

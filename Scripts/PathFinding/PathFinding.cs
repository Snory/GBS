using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    [SerializeField]
    Tilemap _walableTileMap;



    public void Start()
    {
        //iterate tilemap and create tile with f,g and h cost
    }


    public void FindPath(Vector3 startPostion, Vector3 endPosition)
    {
        Vector3Int startTilePosition = _walableTileMap.WorldToCell(startPostion);
        Vector3Int endTilePosition = _walableTileMap.WorldToCell(endPosition);
        AStarTile startTile = _walableTileMap.GetTile<AStarTile>(startTilePosition);
        AStarTile endTile = _walableTileMap.GetTile<AStarTile>(endTilePosition);

        List<AStarTile> openSet = new List<AStarTile>();
        HashSet<AStarTile> closedSet = new HashSet<AStarTile>();

        openSet.Add(startTile);

        while(openSet.Count > 0)
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

            if(currentTile == endTile)
            {
                return; 
            }

            //musime najit sousedy


        }
    }
}

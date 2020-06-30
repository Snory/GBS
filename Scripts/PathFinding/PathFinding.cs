using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;



public class PathFinding : Singleton<PathFinding>
{
 
    [SerializeField]
    private Tilemap WalkableTileMap;
    PathRequestManager _pathRequestManager;

    public HexGrid WalkableGrid { get; set; }


    protected override void Awake()
    {
        base.Awake();
        _pathRequestManager = this.GetComponent<PathRequestManager>();
        WalkableGrid = WalkableTileMap.GetComponent<HexGrid>();
     
    }

  
    internal void StartFindPath(Vector3 pathStart, Vector3 pathEnd)
    {
        StartCoroutine(FindPath(pathStart, pathEnd));
    }



    private IEnumerator FindPath(Vector3 startPostion, Vector3 endPosition)
    {

        HexTile[] wayPoints = new HexTile[0];
        bool pathSuccess = false;


        HexTile startTile =  WalkableGrid.GetHexTileOnWorldPosition(startPostion);
        HexTile endTile = WalkableGrid.GetHexTileOnWorldPosition(endPosition);


        List<HexTile> openSet = new List<HexTile>();
        HashSet<HexTile> closedSet = new HashSet<HexTile>();

        openSet.Add(startTile);

        while (openSet.Count > 0 && endTile != null)
        {

            //find note in the open set with lowest fcost
            HexTile currentTile = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].FCost < currentTile.FCost || openSet[i].FCost == currentTile.FCost && openSet[i].HCost < currentTile.HCost)
                { 
                    currentTile = openSet[i]; 
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

             if (currentTile == endTile)
            {
                pathSuccess = true;
                break; 
            }

            //musime najit sousedy
            foreach(Vector3Int neighbourCoordination in currentTile.GetNeighborCoordinationsInDistance(1))
            {
          
                HexTile neighbour = WalkableGrid.GetHexTileOnGridPosition(neighbourCoordination);


                if(neighbour == null)
                {
                    continue;
                }

                //walkable, close list
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = (int)currentTile.GCost + (int)currentTile.GetDistanceToCoordination(neighbourCoordination);

                if(newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
                {
                    neighbour.GCost = newMovementCostToNeighbour;
                    neighbour.HCost = (int) neighbour.GetDistanceToCoordination(endTile.GridCoordination);
                    neighbour.Parent = currentTile;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);


                    }
                }

            }
        }
        yield return null; //wait for one frame before returning;
        if(pathSuccess == true)
        {
            wayPoints = RetracePath(startTile, endTile);
        }
        _pathRequestManager.PathProcessingFinished(wayPoints, pathSuccess);
    }


    private HexTile[] RetracePath(HexTile startTile, HexTile endTile)
    {
        List<HexTile> path = new List<HexTile>();
        HexTile currentTile = endTile;

        while(currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.Parent;
        }
        path.Add(currentTile);
        path.Reverse();

        return path.ToArray();
    }


}

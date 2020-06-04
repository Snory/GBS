using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    private Vector2 _worldSpaceCoordination;
    private Vector2 _gridCoordination;

    public Vector2 WorldSpaceCoordination { get; set; }
    public Vector2 GridSpaceCoordination { get; set; }


    public Node(Vector2 worldSpaceCoordination, Vector2 gridCoordination)
    {
        WorldSpaceCoordination = worldSpaceCoordination;
        GridSpaceCoordination = gridCoordination;
    }

    
  
}

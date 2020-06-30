using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class Cursor : MonoBehaviour
{



    // Update is called once per frame
    void Update()
    {
        Vector3 mouseInWorld3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        HexTile tile;
        tile = PathFinding.Instance.WalkableGrid.GetHexTileOnWorldPosition(mouseInWorld3);


        if (tile != null)
        {
            Vector3 tilePosition = new Vector3(tile.WorldCoordination.x, tile.WorldCoordination.y - 0.00001f, PathFinding.Instance.WalkableGrid.Tilemap.origin.z);
            this.transform.position = tilePosition;
        }

    }
}

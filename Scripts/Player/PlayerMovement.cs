using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{

    [Range(0,5)]
    [SerializeField]
    private int _movementRange;

    [SerializeField]
    private Tilemap _tileMap;

    [SerializeField]
    private LayerMask _actionableMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowMovementRange();
    }


    void ShowMovementRange()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _tileMap.RefreshAllTiles();

            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(camRay.origin, camRay.direction, Mathf.Infinity, _actionableMask);

            if (hit)
            {
 
            }

        }
    }
}

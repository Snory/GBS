using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class UnitMovement : MonoBehaviourPun
{

    float _speed = 2f;
    int targetIndex;
    Coroutine _movingRoutine;
    [SerializeField]
    GameObject _selectedVisual;
    bool _isSelected;



    // Start is called before the first frame update
    void Start()
    {
        PathFinding.Instance.WalkableGrid.SetHexTileOccupantOnWorldPosition(this.gameObject, this.gameObject.transform.position);
    }


    [PunRPC]
    void Initialize(bool isMine)
    {
        if(isMine)
        {
            PlayerController.Me.Units.Add(this.gameObject);
        }
        else
        {
            if(GameManager.Instance == null)
            {
                Debug.LogError("WTF gamemanager");
            }

            if(GameManager.Instance.GetOtherPlayer(PlayerController.Me) == null)
            {
                Debug.LogError("WTF other player");
            }

            GameManager.Instance.GetOtherPlayer(PlayerController.Me).Units.Add(this.gameObject);
        }
    }

    public void MoveToWorldPosition(Vector3 wolrdPosition)
    {

            if(_movingRoutine != null)
            {
                StopCoroutine(_movingRoutine);
            }
            PathRequestManager.RequestPath(this.transform.position, wolrdPosition, OnPathFound);
        
    }

    public void OnPathFound(HexTile[] path, bool result)
    {
        if(result == true)
        {
            targetIndex = 0;
            _movingRoutine = StartCoroutine(FollowPath(path));
        }
    }

    IEnumerator FollowPath(HexTile[] path)
    {
        Vector3 currentWayPoint = path[1].WorldCoordination;

        PathFinding.Instance.WalkableGrid.SetHexTileOccupantOnWorldPosition(null, this.transform.position);
        PathFinding.Instance.WalkableGrid.SetHexTileOccupantOnWorldPosition(this.gameObject, path[path.Length-1].WorldCoordination);

        while (true)
        {
            if(transform.position == currentWayPoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    
                    yield break;
                }
                currentWayPoint = path[targetIndex].WorldCoordination;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, _speed * Time.deltaTime);
           

            yield return null;

        }
    }
}

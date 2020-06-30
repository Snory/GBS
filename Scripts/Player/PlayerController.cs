using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun
{

    public Player PhotonPlayer;
    public string[] UnitsToSpawn;
    public Transform[] SpawnPoints;

    public List<GameObject> Units = new List<GameObject>();
    private GameObject _selectedUnit;

    public static PlayerController Me; //local player
    public static PlayerController Enemy; //netowork player


    [PunRPC]
    void Initialize(Player player)
    {
        PhotonPlayer = player;

        //local player, spawn units
        if (player.IsLocal)
        {
            Me = this;

            SpawnUnits();
        }
        else
        {
            Enemy = this;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && GameManager.Instance.curPlayer == this)
        {
            Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void TrySelect(Vector3 selectPos)
    {
        GameObject unit = PathFinding.Instance.WalkableGrid.GetHexTileOccupantOnWorldPosition(selectPos);

        if(unit == null &&  _selectedUnit != null)
        {
            _selectedUnit.GetComponent<UnitMovement>().MoveToWorldPosition(selectPos);
        }

        if (Units.Contains(unit) && unit != null)
        {
            SelectUnit(unit);
        }            
    }

    private void SelectUnit(GameObject unit)
    {
        _selectedUnit = unit;    
    }


    void SpawnUnits()
    {
        Debug.Log("Here");

        for(int x = 0; x < UnitsToSpawn.Length -1; x++)
        {
            GameObject unit = PhotonNetwork.Instantiate("unit", SpawnPoints[x].position, Quaternion.identity);
            unit.GetPhotonView().RPC("Initialize", RpcTarget.Others, false);
            unit.GetPhotonView().RPC("Initialize", PhotonPlayer, true);
        }
    }



}

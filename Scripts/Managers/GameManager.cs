using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    public PlayerController topPlayer;
    public PlayerController botPlayer;

    public PlayerController curPlayer;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetPlayers();
        }
    }


    private void SetPlayers() 
    { 

    
        //set owner of the two players photon view
        botPlayer.photonView.TransferOwnership(1);
        topPlayer.photonView.TransferOwnership(2);

        // init player

        botPlayer.photonView.RPC("Initialize", RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.GetPlayer(1));
        topPlayer.photonView.RPC("Initialize", RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.GetPlayer(2));

        photonView.RPC("SetNextTurn", RpcTarget.AllBuffered);

    }

    [PunRPC]
    void SetNextTurn()
    {
        if(curPlayer == null)
        {
            botPlayer = topPlayer;
        }
        else
        {
            curPlayer = curPlayer == botPlayer ? topPlayer : botPlayer;
        }
    }

    public PlayerController GetOtherPlayer (PlayerController player)
    {
        return player == topPlayer ? botPlayer : topPlayer;
    }


    
}

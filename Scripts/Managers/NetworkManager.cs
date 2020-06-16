using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{

    private static NetworkManager _instance;
    public static NetworkManager Instance { get { return _instance; } }

    private void Awake()
    {
        if(Instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {            
            Destroy(this.gameObject);            

        }

    }
    private void Start()
    {
        //connect to the master server
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");

    }
}

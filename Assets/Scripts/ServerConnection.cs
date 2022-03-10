using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ServerConnection : MonoBehaviourPunCallbacks
{
    public Transform loadingPanel, nicknamePanel;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        //base.OnJoinedLobby();
        //disable loading
        nicknamePanel.gameObject.SetActive(true);
        loadingPanel.gameObject.SetActive(false);
    }
}

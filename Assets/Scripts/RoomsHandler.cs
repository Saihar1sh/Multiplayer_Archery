using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class RoomsHandler : MonoBehaviourPunCallbacks,IPunTurnManagerCallbacks
{
    public InputField createInputField, joinInputField, nicknameField;

    public Button createBtn, joinBtn, nicknameEnter;

    public Transform nicknamePanel;

    public Player player;

    private RoomOptions roomOptions;

    const string NickNamePlayerPrefsKey = "NickName";

    private void Awake()
    {
        createBtn.onClick.AddListener(CreateRoom);
        joinBtn.onClick.AddListener(JoinRoom);
        nicknameEnter.onClick.AddListener(NicknameEnter);

        nicknameField.text = PlayerPrefs.HasKey(NickNamePlayerPrefsKey)?PlayerPrefs.GetString(NickNamePlayerPrefsKey):"";

    }
    private void Start()
    {
        player = PhotonNetwork.LocalPlayer;
    }
    public void NicknameEnter()
    {
        player.NickName = nicknameField.text;
        PlayerPrefs.SetString(NickNamePlayerPrefsKey, nicknameField.text);
        nicknamePanel.gameObject.SetActive(false);
    }
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInputField.text, new RoomOptions() { MaxPlayers = 2 , PlayerTtl = 2000, CleanupCacheOnLeave = true}) ;
/*        RoomOptions roomOptions = PhotonNetwork.CurrentRoom.;
        roomOptions.PlayerTtl = ;
*/    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInputField.text);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(1);
    }

    public void OnTurnBegins(int turn)
    {
        throw new System.NotImplementedException();
    }

    public void OnTurnCompleted(int turn)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerMove(Player player, int turn, object move)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerFinished(Player player, int turn, object move)
    {
        throw new System.NotImplementedException();
    }

    public void OnTurnTimeEnds(int turn)
    {
        throw new System.NotImplementedException();
    }
}

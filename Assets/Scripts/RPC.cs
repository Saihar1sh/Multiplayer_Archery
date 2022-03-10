using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class RPC : MonoBehaviourPun, IPunTurnManagerCallbacks,IInRoomCallbacks,IMatchmakingCallbacks
{
    public Text remotePlayerText, localPlayerText,TimeText, TurnText, resultTxt;

    private bool IsShowingResults= false;

    private PunTurnManager turnManager;

    private ResultType result;

    private float serverTime,ahs;

    private bool endT = true;

    private void Start()
    {
        this.turnManager = gameObject.AddComponent<PunTurnManager>();
        this.turnManager.TurnManagerListener = this;
        this.turnManager.TurnDuration = 30f;

        Debug.Log("Rooom code: " + PhotonNetwork.CurrentRoom.Name);

        if (this.TurnText != null)
        {
            this.TurnText.text = this.turnManager.Turn.ToString();
        }

        if (this.turnManager.Turn >= 0 && this.TimeText != null && !IsShowingResults)//.l
        {

            this.TimeText.text = this.turnManager.RemainingSecondsInTurn.ToString("F1") + " SECONDS";
        }

        resultTxt.gameObject.SetActive(false);
    }

        private void Update()
    {
        //Debug.Log("server stamp:" + PhotonNetwork.ServerTimestamp + " getturnstart " + PhotonNetwork.CurrentRoom.GetTurnStart());
        ahs = serverTime - this.turnManager.RemainingSecondsInTurn;
        //Debug.Log(" sgfa: "+(PhotonNetwork.Time));
        UpdatePlayerDeets();

        if (turnManager.Turn > 3)
        {
            Debug.Log("Stop");
            PlayerController.canInput = false;
            //return;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            if (this.turnManager.IsOver)
            {
                return;
            }

            /*
			// check if we ran out of time, in which case we loose
			if (turnEnd<0f && !IsShowingResults)
			{
					Debug.Log("Calling OnTurnCompleted with turnEnd ="+turnEnd);
					OnTurnCompleted(-1);
					return;
			}
		*/

            if (this.TurnText != null)
            {
                this.TurnText.text = this.turnManager.Turn.ToString();
            }

            if (this.turnManager.Turn >= 0 && this.TimeText != null && !IsShowingResults)//.l
            {

                this.TimeText.text = this.turnManager.RemainingSecondsInTurn.ToString("F1") + " SECONDS";

                //TimerFillImage.anchorMax = new Vector2(1f - this.turnManager.RemainingSecondsInTurn / this.turnManager.TurnDuration, 1f);
            }


        }

        Debug.Log("lucif is finished by local: " + turnManager.IsFinishedByMe + " is completed by all: " + turnManager.IsCompletedByAll);
        if (turnManager.IsFinishedByMe)
        {
            PlayerController.canInput = false;
        }
        else
        {
            PlayerController.canInput = true;

        }
        if (turnManager.IsCompletedByAll)
        {
            this.OnTurnCompleted(this.turnManager.Turn);
        }
    }
    public void OnTurnBegins(int turn)
    {
        Debug.Log("OnTurnBegins() turn: " + turn);

        this.IsShowingResults = false;
        this.resultTxt.gameObject.SetActive(false);
    }
    public void LocalPlayerHitTarget(int score)
    {
        Player player = PhotonNetwork.LocalPlayer;
        player.AddScore(score);
    }
    private void UpdateScores()
    {
        if (this.result == ResultType.LocalWin)
        {
            PhotonNetwork.LocalPlayer.AddScore(1);   // this is an extension method for PhotonPlayer. you can see it's implementation
        }
    }

    private void UpdatePlayerDeets()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        Player remotePlayer = PhotonNetwork.LocalPlayer.GetNext();

        if (remotePlayer != null)
        {
            // should be this format: "name        00"
            this.remotePlayerText.text = remotePlayer.NickName + "        " + remotePlayer.GetScore().ToString("D2");
        }
        else
        {

            //TimerFillImage.anchorMax = new Vector2(0f, 1f);
            //this.TimeText.text = "";
            this.remotePlayerText.text = "waiting for another player        00";
        }

        if (localPlayer != null)
        {
            // should be this format: "YOU   00"
            this.localPlayerText.text = localPlayer.NickName+"   " + localPlayer.GetScore().ToString("D2");
        }
    }

    public void OnTurnCompleted(int turn)
    {
        Debug.Log("OnTurnCompleted: " + turn);

        this.UpdateScores();
        if(turn >= 3)
        {
            this.CalculateWinAndLoss();
            return;
        }

        this.OnEndTurn();
    }

    public void OnPlayerMove(Player player, int turn, object move)
    {
        Debug.Log("OnPlayerMove: " + player + " turn: " + turn + " action: " + move);
        throw new System.NotImplementedException();
    }

    public void OnPlayerFinished(Player player, int turn, object move)
    {
        Debug.Log("OnTurnFinished: " + player + " turn: " + turn + " action: " + move);
    }

    public void OnTurnTimeEnds(int turn)
    {
        if (!IsShowingResults)
        {
            Debug.Log("OnTurnTimeEnds: Calling OnTurnCompleted");
            OnTurnCompleted(-1);
        }
    }


    #region Core Gameplay Methods


    /// <summary>Call to start the turn (only the Master Client will send this).</summary>
    public void StartTurn()
    {
        this.resultTxt.gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerController.canInput = true;
            this.turnManager.BeginTurn();

        }
    }

    public void MakeTurn(float selection)
    {
        this.turnManager.SendMove((byte)selection, true);
    }

    public void OnEndTurn()
    {
        if (endT)
        {
        this.StartCoroutine(ShowResultsBeginNextTurnCoroutine());

        }
    }

    public IEnumerator ShowResultsBeginNextTurnCoroutine()
    {
        endT = false;
        IsShowingResults = true;
        yield return new WaitForSeconds(2.0f);

        this.StartTurn();
        endT = true;
    }


    public void EndGame()
    {
        Debug.Log("EndGame");
    }

    private void CalculateWinAndLoss()
    {
        //calculate both scores
        int playerScore = PhotonNetwork.LocalPlayer.GetScore();
        int remoteScore = PhotonNetwork.LocalPlayer.GetNext().GetScore();

        if (playerScore == remoteScore)
        {
            this.resultTxt.text = "Draw....";
        }
        else if (playerScore > remoteScore)
        {
            this.resultTxt.text = "Won!!!!";

        }
        else if(playerScore < remoteScore)
        {
            this.resultTxt.text = "Round loss";

        }
        this.resultTxt.gameObject.SetActive(true);

    }

    #endregion
    /*    public override void OnJoinedRoom()
        {
            RefreshUIViews();

            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                if (this.turnManager.Turn == 0)
                {
                    // when the room has two players, start the first turn (later on, joining players won't trigger a turn)
                    this.StartTurn();
                }
            }
            else
            {
                Debug.Log("Waiting for another player");
            }
        }
    */
    #region inroomcallbacks
    public void OnPlayerEnteredRoom(Player newPlayer)
    {
/*        Debug.Log("Other player arrived");

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if (this.turnManager.Turn == 0)
            {
                // when the room has two players, start the first turn (later on, joining players won't trigger a turn)
                this.StartTurn();
            }
        }
*/    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Other player disconnected! " + otherPlayer.ToStringFull());
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        throw new System.NotImplementedException();
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    #region matchmaking callbacks

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new System.NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new System.NotImplementedException();
    }

    public void OnJoinedRoom()
    {
        //RefreshUIViews();

        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            if (this.turnManager.Turn == 0)
            {
                // when the room has two players, start the first turn (later on, joining players won't trigger a turn)
                this.StartTurn();
            }
        }
        else
        {
            Debug.Log("Waiting for another player");
        }

    }

    #endregion

}

public enum ResultType
{
    None,
    LocalWin,
    LocalLoss,
    Draw
}
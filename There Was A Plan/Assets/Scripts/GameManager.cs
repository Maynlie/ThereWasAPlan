using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    ///

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        /*else if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            Debug.LogFormat("PhotonNetwork : Loading Level : PlanificationScene");
            PhotonNetwork.LoadLevel("PlanificationScene");
        }*/
        
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (PlayerNetworkTest.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                float x = -200 + 200 * (PhotonNetwork.CurrentRoom.PlayerCount - 1);
                GameObject player = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(x, 0f, 0f), Quaternion.identity, 0);
                //player.transform.SetParent(GameObject.Find("Canvas").transform);
                //player.transform.parent = GameObject.Find("Canvas").transform;
                //player.transform.position = new Vector3(-210f, -50f, 0f);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

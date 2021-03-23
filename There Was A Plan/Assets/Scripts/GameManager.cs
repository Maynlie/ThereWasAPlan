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
    public GameObject playerUIPrefab;

    public GameObject[] startPoints;

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

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        
            if (PlayerNetworkTest.LocalPlayerInstance == null)
            {
                if (SceneManager.GetActiveScene().name == "School_Manon_Night")
                {
                var allPlayers = PhotonNetwork.PlayerList;
                foreach(Player p in allPlayers)
                {
                    if(p == PhotonNetwork.LocalPlayer)
                    {
                        PhotonNetwork.Instantiate(this.playerPrefab.name, startPoints[p.ActorNumber - 1].transform.position, Quaternion.identity, 0);
                        Debug.Log("playerNumber = " + p.ActorNumber);
                    }
                }
                    
                }
                else
                { 
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    float x = -200 + 200 * (PhotonNetwork.CurrentRoom.PlayerCount - 1);
                    PhotonNetwork.Instantiate(this.playerUIPrefab.name, new Vector3(x, 0f, 0f), Quaternion.identity, 0);
                }
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

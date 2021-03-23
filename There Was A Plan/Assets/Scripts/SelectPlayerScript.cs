using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SelectPlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Sprite[] sprites;
    int currentSprite;
    public GameObject readyBtn;
    public GameObject readyLbl;
    public GameObject leftBtn;
    public GameObject rightBtn;
    GameObject startBtn;
    bool isReady = false;

    void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        if (!photonView.IsMine)
        {
            int index = GameObject.Find("Panel").transform.GetSiblingIndex();
            transform.SetSiblingIndex(index + 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine) {
            var str = "ColorP" + PhotonNetwork.LocalPlayer.ActorNumber;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { str, 0 } });
            currentSprite = 0;
            readyLbl.SetActive(false);
            readyBtn.GetComponent<Button>().onClick.AddListener(ready); ;
            leftBtn.GetComponent<Button>().onClick.AddListener(selectLeft);
            rightBtn.GetComponent<Button>().onClick.AddListener(selectRight);
        }
        else
        {
            leftBtn.SetActive(false);
            rightBtn.SetActive(false);
            readyBtn.SetActive(false);
            readyLbl.SetActive(false);
        }
        startBtn = GameObject.Find("StartBtn");
        if (startBtn != null)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                startBtn.SetActive(false);
            }
            else
            {
                startBtn.GetComponent<Button>().onClick.AddListener(startLevel);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().sprite = sprites[currentSprite];
    }

    void startLevel()
    {
        if (PhotonNetwork.IsMasterClient && photonView.IsMine) {
            PhotonNetwork.LoadLevel("PlanificationDemo");
        }
    }

    void selectRight()
    {
        if (photonView.IsMine) {
            if (currentSprite == sprites.Length - 1)
            {
                currentSprite = 0;
            }
            else
            {
                currentSprite++;
            }
            var str = "ColorP" + PhotonNetwork.LocalPlayer.ActorNumber;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { str, currentSprite } });
        }
    }

    void selectLeft()
    {
        if (photonView.IsMine)
        {
            if (currentSprite == 0)
            {
                currentSprite = sprites.Length - 1;
            }
            else
            {
                currentSprite--;
            }
            var str = "ColorP" + PhotonNetwork.LocalPlayer.ActorNumber;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { str, currentSprite } });
        }
        
    }

    void ready()
    {
        if (photonView.IsMine)
        {
            readyBtn.SetActive(false);
            leftBtn.SetActive(false);
            rightBtn.SetActive(false);
            readyLbl.SetActive(true);
            isReady = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isReady);
            stream.SendNext(currentSprite);
        }
        else
        {
            isReady = (bool)stream.ReceiveNext();
            currentSprite = (int)stream.ReceiveNext();
            if(isReady)
            {
                readyBtn.SetActive(false);
                leftBtn.SetActive(false);
                rightBtn.SetActive(false);
                readyLbl.SetActive(true);
            }
        }
    }
}

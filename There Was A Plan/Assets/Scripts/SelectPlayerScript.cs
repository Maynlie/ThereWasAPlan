using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SelectPlayerScript : MonoBehaviourPunCallbacks, IPunObservable
{
    public Sprite[] sprites;
    int currentSprite;
    public GameObject readyBtn;
    public GameObject readyLbl;
    public GameObject leftBtn;
    public GameObject rightBtn;
    bool isReady = false;
    int nbReady = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().sprite = sprites[currentSprite];
        if(PhotonNetwork.IsMasterClient && nbReady == 2)
        {
            nbReady = -1;
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
        }
        
    }

    void ready()
    {
        if (photonView.IsMine)
        {
            readyBtn.SetActive(false);
            readyLbl.SetActive(true);
            isReady = true;
            nbReady++;
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
                readyLbl.SetActive(true);
                if(!photonView.IsMine) nbReady++;
            }
        }
    }
}

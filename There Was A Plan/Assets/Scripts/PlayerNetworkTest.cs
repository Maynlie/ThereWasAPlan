using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerNetworkTest : MonoBehaviourPun
{
    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    private void Start()
    {
        
    }

    private void Awake()
    {
        if(photonView.IsMine)
        {
            PlayerNetworkTest.LocalPlayerInstance = this.gameObject;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void HandleMove()
    {
        if(photonView.IsMine)
        {
            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(moveH * 0.1f, moveV * 0.1f, 0);
            transform.position = transform.position + move;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected) return;
        HandleMove();
    }
}

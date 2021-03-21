using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TimerScript : MonoBehaviour
{
    public Text timerLbl;
    public float timer;
    bool isLoading;
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            isLoading = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        timerLbl.text = "" + (int)timer;
        if(timer <= 0 && PhotonNetwork.IsMasterClient && !isLoading)
        {
            PhotonNetwork.LoadLevel("School_Manon_Night");
            isLoading = true;
        }
    }
}

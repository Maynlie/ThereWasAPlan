using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerNetworkTest : NetworkBehaviour
{
    void HandleMove()
    {
        if(isLocalPlayer)
        {
            float moveH = Input.GetAxis("Horizontal");
            float moveV = Input.GetAxis("Vertical");
            Vector3 move = new Vector3(moveH * 0.1f, moveV * 0.1f, 0);
            transform.position = transform.position + move;
        }
    }

    private void Update()
    {
        HandleMove();
    }
}

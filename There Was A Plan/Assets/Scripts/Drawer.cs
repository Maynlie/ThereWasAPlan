using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Drawer : MonoBehaviourPunCallbacks, IPunObservable
{
    Vector3 mousePos;
    bool isDrawing;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(mousePos);
            stream.SendNext(isDrawing);
        }
        else
        {
            mousePos = (Vector3)stream.ReceiveNext();
            isDrawing = (bool)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        { 
            if (Input.GetMouseButton(0))
            {
                isDrawing = true;
                mousePos = Input.mousePosition;
            }
            else
            {
                isDrawing = false;
            }
        }
        if(isDrawing)
        {
            Plane objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(mousePos);
            float rayDistance;
            GetComponent<TrailRenderer>().enabled = true;
            if (objPlane.Raycast(mRay, out rayDistance))
                this.transform.position = mRay.GetPoint(rayDistance);
        }
    }
}

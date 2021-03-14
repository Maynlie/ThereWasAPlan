using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DrawerFactory : MonoBehaviourPun
{
    public GameObject prefab;
    private Drawer d;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            d = PhotonNetwork.Instantiate(prefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0).GetComponent<Drawer>(); //Instantiate(prefab).GetComponent<Drawer>();
            Debug.Log("bbb");
        }
        if (Input.GetMouseButtonUp(0))
        {
            d.enabled = false;
            Debug.Log("aaa");
        }
    }
}

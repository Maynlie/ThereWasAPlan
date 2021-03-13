using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerFactory : MonoBehaviour
{
    public GameObject prefab;
    private Drawer d;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            d = Instantiate(prefab).GetComponent<Drawer>();
            Debug.Log("bbb");
        }
        if (Input.GetMouseButtonUp(0))
        {
            d.enabled = false;
            Debug.Log("aaa");
        }
    }
}

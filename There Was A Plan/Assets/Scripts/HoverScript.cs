using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverScript : MonoBehaviour
{
    public GameObject details;
    void OnMouseEnter()
    {
        details.GetComponent<MeshRenderer>().enabled = true;
    }

    void OnMouseExit()
    {
        details.GetComponent<MeshRenderer>().enabled = false;
    }
}

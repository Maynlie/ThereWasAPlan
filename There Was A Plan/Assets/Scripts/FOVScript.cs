using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVScript : MonoBehaviour
{
    public float timeToUpdate = .5f;
    public float rotationRange = 90f;
    public GameObject player;
    public GameObject actives;
    public float visibleDistance = 2.0f;

    float elapsed = 0f;
    // Update is called once per frame
    void Update()
    {
        foreach(Transform child in actives.transform)
        {
            MeshRenderer m = child.gameObject.GetComponentInChildren<MeshRenderer>();
            if (m != null)
            {
                if (Vector3.Distance(child.position, player.transform.position) < visibleDistance)
                    m.enabled = true;
                else
                    m.enabled = false;
            }
            
        }
        elapsed += Time.deltaTime;
        if(elapsed > timeToUpdate)
        {
            elapsed -= timeToUpdate;
            gameObject.transform.Rotate(0f, rotationRange * Random.value, 0f);
        }
    }
}

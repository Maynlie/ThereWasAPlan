using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actionnable : MonoBehaviour
{

    public GameObject requiredToActivate;
    public bool holdButton;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log(audioSource);
    }

    public virtual bool Activate(GameObject userHandles)
    {
        bool canActivate = false;
        if (!requiredToActivate || userHandles == requiredToActivate)
        {
            audioSource.Play();
            canActivate = true;
        }
        return canActivate;
    }
}

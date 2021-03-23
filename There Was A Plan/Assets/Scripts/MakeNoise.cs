using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeNoise : Actionnable
{

    void Start()
    {

    }

    public override bool Activate(GameObject userHandles)
    {
        bool canActivate = false;
        if (base.Activate(userHandles))
        {
            Debug.Log("MAKE SOME NOISE !!!");
            canActivate = true;
            GameObject.Find("Demon").GetComponent<DemonIA>().heardSound();
        }
        return canActivate;
    }

}

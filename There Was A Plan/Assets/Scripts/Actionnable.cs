using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actionnable : MonoBehaviour
{

    public PickableItem requiredToActivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activate(PickableItem userHandles)
    {
        
        if (!requiredToActivate || userHandles == requiredToActivate)
        {
            Debug.Log("Activate");
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("Can't activate");
        }
    }
}

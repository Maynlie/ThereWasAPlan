using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actionnable : MonoBehaviour
{

    public GameObject requiredToActivate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activate(GameObject userHandles)
    {
        
        if (!requiredToActivate || userHandles == requiredToActivate)
        {
            Debug.Log("Activate");
            gameObject.GetComponent<DoorRotation>().Activate();
        }
        else
        {
            Debug.Log("Can't activate");
        }
    }
}

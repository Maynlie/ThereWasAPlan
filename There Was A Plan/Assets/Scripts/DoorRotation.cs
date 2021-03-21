using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class DoorRotation : MonoBehaviour
{
    public float openSpeed = 100;
    public float openAngle = 90;

    float defaultRotationAngle;
    float currentRotationAngle;
    float openTime = 0;

    bool open = false;

    void Start()
    {
        defaultRotationAngle = transform.localEulerAngles.y;
        currentRotationAngle = transform.localEulerAngles.y;
    }

    // Main function
    void Update()
    {
        if (openTime < 1)
        {
            openTime += Time.deltaTime * openSpeed;
        }
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Mathf.LerpAngle(currentRotationAngle, defaultRotationAngle + (open ? openAngle : 0), openTime), transform.localEulerAngles.z);
    }

    public void Activate()
    {
        open = !open;
        currentRotationAngle = transform.localEulerAngles.y;
        openTime = 0;
    }
}

    

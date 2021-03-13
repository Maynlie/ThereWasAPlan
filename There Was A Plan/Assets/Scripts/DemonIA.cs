using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonIA : MonoBehaviour
{
    private enum IAState { IDLE, CHECK_SOUND, CHASE, ALERT, LOOK_AROUND, BACK_TO_IDLE, BACK_TO_BED };
    private IAState state;
    [SerializeField]
    Vector3 startPoint;
    [SerializeField]
    Vector3 soundPoint;
    [SerializeField]
    Vector3 childPos;
    [SerializeField]
    Vector3 bedPos;
    [SerializeField]
    Vector3[] rondePos;

    private float timer = 0.0f;
    private int currentPoint = -1;
    // Start is called before the first frame update
    void Start()
    {
        state = IAState.IDLE;
        transform.position = startPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            state = IAState.CHECK_SOUND;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            state = IAState.CHASE;
        }
        switch (state)
        {
            case IAState.IDLE:
                idle();
                break;
            case IAState.CHECK_SOUND:
                checkSound();
                break;
            case IAState.CHASE:
                chase();
                break;
            case IAState.ALERT:
                alert();
                break;
            case IAState.LOOK_AROUND:
                lookAround();
                break;
            case IAState.BACK_TO_IDLE:
                backToIdle();
                break;
            case IAState.BACK_TO_BED:
                backToBed();
                break;
        }
    }

    void idle()
    {

    }

    void checkSound()
    {
        if (goToPoint(soundPoint))
        {
            state = IAState.LOOK_AROUND;
        }
    }

    void lookAround()
    {
        Debug.Log("Looking Around");
        timer += Time.deltaTime;
        if(timer >= 2.0f)
        {
            timer = 0;
            state = IAState.BACK_TO_IDLE;
        }
    }

    void backToIdle()
    {
        if (goToPoint(startPoint))
        {
            state = IAState.IDLE;
        }
    }

    void chase()
    {
        if(goToPoint(childPos))
        {
            state = IAState.BACK_TO_BED;
        }
    }

    void backToBed()
    {
        if (goToPoint(bedPos))
        {
            if(checkBeds())
            {
                state = IAState.BACK_TO_IDLE;
            }
            else
            {
                state = IAState.ALERT;
            }
        }
    }

    bool checkBeds()
    {
        return false;
    }

    void alert()
    {
        if(currentPoint == -1)
        {
            float minDist = -1.0f;
            int i = 0;
            foreach(Vector3 v in rondePos)
            {
                Vector3 diff = v - transform.position;
                float dist = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y);
                if(minDist < 0.0f)
                {
                    minDist = dist;
                    currentPoint = i;
                } else if(dist < minDist)
                {
                    minDist = dist;
                    currentPoint = i;
                }
                i++;
            }
        }
        if(goToPoint(rondePos[currentPoint]))
        {
            currentPoint++;
            if(currentPoint == rondePos.Length)
            {
                currentPoint = 0;
            }
        }
    }

    bool goToPoint(Vector3 target)
    {
        Vector3 diff = target - transform.position;
        float dist = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y);
        if (dist <= 0.5f)
        {
            return true;
        }
        else
        {
            transform.position += 0.5f * diff;
            return false;
        }
    }
}

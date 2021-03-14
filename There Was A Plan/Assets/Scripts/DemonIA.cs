using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class DemonIA : MonoBehaviour
{
    private enum IAState { IDLE, CHECK_SOUND, CHASE, ALERT, LOOK_AROUND, BACK_TO_IDLE, BACK_TO_BED };
    private IAState state;
    [SerializeField]
    Vector2 startPoint;
    [SerializeField]
    Vector2 soundPoint;
    [SerializeField]
    Vector2 childPos;
    [SerializeField]
    Vector2 bedPos;
    [SerializeField]
    Vector2[] rondePos;

    Vector2 currentTarget;

    Seeker seeker;
    Rigidbody rb;

    private float timer = 0.0f;
    private int currentPoint = 0;

    public float speed = 200f;
    public float nextDist = 3f;

    Path path;
    int currentWayPoint = 0;
    bool reached = false;

    // Start is called before the first frame update
    void Start()
    {
        state = IAState.IDLE;
        transform.position = new Vector3(startPoint.x, startPoint.y, 1.5f);
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
        currentTarget = startPoint;
        Debug.Log("currentTarget" + currentTarget);

        InvokeRepeating("UpdatePath", 0.0f, 0.5f);
        
    }

    void UpdatePath()
    {
        Debug.Log(state);
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
            case IAState.BACK_TO_IDLE:
                backToIdle();
                break;
            case IAState.BACK_TO_BED:
                backToBed();
                break;
        }
        if (state != IAState.IDLE && state != IAState.LOOK_AROUND)
        {
            Debug.Log("Seek for target " + currentTarget + "with state" + state);
            seeker.StartPath(transform.position, currentTarget, onPathComplete);
        } else
        {
            path = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            state = IAState.CHECK_SOUND;
            path = null;
            reached = false;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            state = IAState.CHASE;
        }
        if(state == IAState.LOOK_AROUND)
        {
            lookAround();
        }
        if (path == null) return;
        if(currentWayPoint >= path.vectorPath.Count)
        {
            reached = true;
            return;
        } else
        {
            reached = false;
        }
        if (state == IAState.LOOK_AROUND)
        {
            reached = false;
            lookAround();
        }
        Vector2 direction = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        transform.position += new Vector3(force.x, force.y, 0);

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]);

        if(distance < nextDist)
        {
            currentWayPoint++;
        }
    }

    void onPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    void idle()
    {
        
    }

    void checkSound()
    {
        currentTarget = soundPoint;
        Debug.Log("currentTarget" + currentTarget);
        if (reached)
        {
            state = IAState.LOOK_AROUND;
        }
    }

    void lookAround()
    {
        timer += Time.deltaTime;
        Debug.Log("Looking Around " + timer);
        if (timer >= 2.0f)
        {
            timer = 0;
            reached = false;
            currentTarget = startPoint;
            Debug.Log("currentTarget" + currentTarget);
            state = IAState.BACK_TO_IDLE;
        }
    }

    void backToIdle()
    {
        currentTarget = startPoint;
        Debug.Log("currentTarget" + currentTarget);
        if (reached)
        {
            state = IAState.IDLE;
        }
    }

    void chase()
    {
        currentTarget = childPos;
        Debug.Log("currentTarget" + currentTarget);
        if (reached)
        {
            state = IAState.BACK_TO_BED;
        }
    }

    void backToBed()
    {
        currentTarget = bedPos;
        Debug.Log("currentTarget" + currentTarget);
        if (reached)
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
        currentTarget = rondePos[currentPoint];
        Debug.Log("currentTarget" + currentTarget);
        if (reached)
        {
            currentPoint++;
            if(currentPoint == rondePos.Length)
            {
                currentPoint = 0;
            }
        }
    }
}

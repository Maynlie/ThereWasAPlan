using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TopDownController : MonoBehaviour
{
    //Player Camera variables
    public enum CameraDirection { x, z }
    public CameraDirection cameraDirection = CameraDirection.x;
    public float cameraHeight = 10f;
    public float cameraDistance = 7f;
    public Camera playerCamera;
    //public GameObject targetIndicatorPrefab;
    //Player Controller variables
    public float speed = 5.0f;
    public float gravity = 14.0f;
    public float maxVelocityChange = 10.0f;
    public bool canJump = true;
    public float jumpHeight = 2.0f;
    //Private variables
    bool grounded = false;
    Rigidbody r;
    GameObject targetObject;
    //Mouse cursor Camera offset effect
    Vector2 playerPosOnScreen;
    Vector2 cursorPosition;
    Vector2 offsetVector;

    // Reference to the currently held item.
    private GameObject pickedItem;

    private GameObject itemAtHand;

    // Reference to the slot for holding picked item.
    [SerializeField]
    private Transform slot;

    public BoxCollider interactionCollider;

    private bool isHidden;
    private Vector3 returnPosition;

    private float actionCooldown = 0;
    public float cooldownLength = 0.2f;

    void Awake()
    {
        r = GetComponent<Rigidbody>();
        r.freezeRotation = true;

        //Hide the cursor
        Cursor.visible = true;
    }

    void FixedUpdate()
    {
        if (actionCooldown > 0)
        {
            actionCooldown -= 0.02f;
        }

        //Setup camera offset
        Vector3 cameraOffset = Vector3.zero;
        if (cameraDirection == CameraDirection.x)
        {
            cameraOffset = new Vector3(cameraDistance, cameraHeight, 0);
        }
        else if (cameraDirection == CameraDirection.z)
        {
            cameraOffset = new Vector3(0, cameraHeight, cameraDistance);
        }

        if (grounded)
        {
            Vector3 targetVelocity = Vector3.zero;
            if (!isHidden)
            { 
                targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                targetVelocity *= speed;
            }
            
            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = r.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            r.AddForce(velocityChange, ForceMode.VelocityChange);

            // Jump
            if (canJump && Input.GetButton("Jump"))
            {
                r.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
            }

            // Execute logic only on button pressed!
            if (Input.GetButtonDown("Fire1") && actionCooldown <= 0)
            {
                // Check if player picked some item already
                if (isHidden)
                {
                    isHidden = false;
                    r.transform.position = returnPosition;
                    actionCooldown = cooldownLength;
                    GameObject.Find("Body").GetComponent<SpriteRenderer>().enabled = true;
                }
                else if (itemAtHand && itemAtHand.GetComponent<Hide>())
                {
                    Debug.Log("Gonna hide");
                    isHidden = true;
                    returnPosition = r.transform.position;
                    GameObject.Find("Body").GetComponent<SpriteRenderer>().enabled = false;
                    r.transform.position = new Vector3(itemAtHand.transform.position.x, r.transform.position.y, itemAtHand.transform.position.z);
                    actionCooldown = cooldownLength;
                }
                else if (pickedItem)
                {
                    if (itemAtHand && itemAtHand.GetComponent<Actionnable>())
                    {
                        Debug.Log("Gonna activate");
                        itemAtHand.GetComponent<Actionnable>().Activate(pickedItem);
                    }
                    else
                    {
                        // If yes, drop picked item
                        DropItem(pickedItem);
                    }
                    actionCooldown = cooldownLength;
                }
                else if (itemAtHand)
                {
                    if (itemAtHand.GetComponent<Actionnable>())
                    {
                        Debug.Log("Gonna activate");
                        Debug.Log(itemAtHand.GetComponent<Actionnable>());
                        itemAtHand.GetComponent<Actionnable>().Activate(null);
                    }
                    if (itemAtHand.GetComponent<PickableItem>())
                    {
                        PickItem(itemAtHand);
                    }
                    actionCooldown = cooldownLength;
                }
            }
        }

        // We apply gravity manually for more tuning control
        r.AddForce(new Vector3(0, -gravity * r.mass, 0));

        grounded = false;

        if(playerCamera != null)
        {
            //Mouse cursor offset effect
            playerPosOnScreen = playerCamera.WorldToViewportPoint(transform.position);
            cursorPosition = playerCamera.ScreenToViewportPoint(Input.mousePosition);
            offsetVector = cursorPosition - playerPosOnScreen;

            //Camera follow
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, transform.position + cameraOffset, Time.deltaTime * 7.4f);
        }
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if ((otherCollider.gameObject.GetComponent<PickableItem>() != null
            || otherCollider.gameObject.GetComponent<Actionnable>() != null
            || otherCollider.gameObject.GetComponent<Hide>() != null)
            && otherCollider.gameObject != pickedItem)
        {
            Debug.Log("I can interact with " + otherCollider.gameObject.name);
            itemAtHand = otherCollider.gameObject;
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider.gameObject == itemAtHand)
        {
            Debug.Log("I can't interact anymore with" + otherCollider.gameObject.name);
            itemAtHand = null;
        }
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    /// <summary>
    /// Method for picking up item.
    /// </summary>
    /// <param name="item">Item.</param>
    private void PickItem(GameObject item)
    {
        // Assign reference
        pickedItem = item;

        // Disable rigidbody and reset velocities
        item.GetComponent<PickableItem>().Rb.isKinematic = true;
        item.GetComponent<PickableItem>().Rb.velocity = Vector3.zero;
        item.GetComponent<PickableItem>().Rb.angularVelocity = Vector3.zero;
        item.GetComponent<BoxCollider>().enabled = false;

        // Set Slot as a parent

        if (item.GetComponent<PickableItem>().isCarriable)
        {
            item.transform.position = slot.transform.position;
        }
        else
        {
            item.transform.position += (item.transform.position - interactionCollider.transform.position) * 0.2f;
        }
        item.transform.SetParent(slot);
    }

    /// <summary>
    /// Method for dropping item.
    /// </summary>
    /// <param name="item">Item.</param>
    private void DropItem(GameObject item)
    {
        // Remove reference
        pickedItem = null;

        // Remove parent
        item.transform.SetParent(null);

        // Enable rigidbody
        item.GetComponent<PickableItem>().Rb.isKinematic = false;
        item.GetComponent<BoxCollider>().enabled = true;
    }

}

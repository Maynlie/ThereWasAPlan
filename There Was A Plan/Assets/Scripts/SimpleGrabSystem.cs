using UnityEngine;

/// <summary>
/// Simple example of Grabbing system.
/// </summary>
public class SimpleGrabSystem : MonoBehaviour
{
    // Reference to the character camera.
    [SerializeField]
    private Camera characterCamera;

    // Reference to the slot for holding picked item.
    [SerializeField]
    private Transform slot;

    // Reference to the currently held item.
    private PickableItem pickedItem;

    private GameObject canBePicked;

    public BoxCollider playerCollider;

    /// <summary>
    /// Method called very frame.
    /// </summary>
    private void Update()
    {
        // Execute logic only on button pressed!
        if (Input.GetButtonDown("Fire1"))
        {
            // Check if player picked some item already
            if (pickedItem)
            {
                if (canBePicked && canBePicked.GetComponent<Actionnable>())
                {
                    Debug.Log("Gonna activate");
                    canBePicked.GetComponent<Actionnable>().activate(pickedItem);
                }
                else
                { 
                    // If yes, drop picked item
                    DropItem(pickedItem);
                }
            }
            else if (canBePicked)
            {
                if (canBePicked.GetComponent<Actionnable>())
                {
                    Debug.Log("Gonna activate");
                    canBePicked.GetComponent<Actionnable>().activate(null);
                }
                if (canBePicked.GetComponent<PickableItem>())
                {
                    PickItem(canBePicked.GetComponent<PickableItem>());
                }
            }
        }
    }

    /// <summary>
    /// Method for picking up item.
    /// </summary>
    /// <param name="item">Item.</param>
    private void PickItem(PickableItem item)
    {
        // Assign reference
        pickedItem = item;

        // Disable rigidbody and reset velocities
        item.Rb.isKinematic = true;
        item.Rb.velocity = Vector3.zero;
        item.Rb.angularVelocity = Vector3.zero;

        // Set Slot as a parent

        if (item.isCarriable)
        {
            //item.transform.position = new Vector3(
            //    item.transform.position.x,
            //    playerCollider.transform.position.y + (playerCollider.size.y * playerCollider.transform.localScale.y / 2),
            //    item.transform.position.z
            //);
            item.transform.position = slot.transform.position;
        }
        else
        {
            item.transform.position += (item.transform.position - playerCollider.transform.position) * 0.2f;
        }
        item.transform.SetParent(slot);

        // Reset position and rotation
        //item.transform.localPosition = Vector3.zero;
        //item.transform.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// Method for dropping item.
    /// </summary>
    /// <param name="item">Item.</param>
    private void DropItem(PickableItem item)
    {
        // Remove reference
        pickedItem = null;

        // Remove parent
        item.transform.SetParent(null);

        // Enable rigidbody
        item.Rb.isKinematic = false;

        // Add force to throw item a little bit
        //item.Rb.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.gameObject.GetComponent<PickableItem>() != null || otherCollider.gameObject.GetComponent<Actionnable>() != null)
        {
            canBePicked = otherCollider.gameObject;
            Debug.Log("I can grab that");
        }
    }

    void OnTriggerExit(Collider otherCollider)
    {
        canBePicked = null;
        Debug.Log("I can't grab it anymore");
    }
}


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

    private PickableItem canBePicked;

    public CapsuleCollider collider;

    /// <summary>
    /// Method called very frame.
    /// </summary>
    private void Update()
    {
        // Execute logic only on button pressed
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("boop");
            // Check if player picked some item already
            if (pickedItem)
            {
                // If yes, drop picked item
                DropItem(pickedItem);
            }
            else if (canBePicked)
            {
                PickItem(canBePicked);
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
        item.Rb.AddForce(item.transform.forward * 2, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision collision)
    {;
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            canBePicked = contact.otherCollider.gameObject.GetComponent<PickableItem>();
            if (canBePicked)
            {
                Debug.Log("I can grab that");
            }
        }
    }
}


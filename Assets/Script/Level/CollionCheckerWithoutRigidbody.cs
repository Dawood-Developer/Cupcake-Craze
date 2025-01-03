using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollionCheckerWithoutRigidbody : MonoBehaviour
{
    private Collider thisCollider;     // Reference to this object's collider
    private GameObject other;          // Reference to the detected object
    public LayerMask detectionLayer;

    private void Awake()
    {
        thisCollider = GetComponent<Collider>(); // Automatically get the attached collider
        if (thisCollider == null)
        {
            Debug.LogError("No Collider found on " + gameObject.name + "! Please attach a Collider.");
        }
    }

    private void Update()
    {
        if (thisCollider == null) return; // Exit if there's no collider

        // Use bounds from the current collider for dynamic overlap detection
        Vector3 boxCenter = thisCollider.bounds.center;
        Vector3 boxSize = thisCollider.bounds.size;

        // Perform collision detection within the box boundaries
        Collider[] overlappingColliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity,detectionLayer);

        if (overlappingColliders.Length > 0)
        {
            foreach (Collider collider in overlappingColliders)
            {
                if (collider.gameObject != this.transform.parent & collider.gameObject != this.gameObject)
                {
                    //isAnyOtherBoxOnThisBox = true;
                    other = collider.gameObject;
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (thisCollider == null) return;

        // Draw a gizmo to visualize the box detection area
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(thisCollider.bounds.center, thisCollider.bounds.size);
    }
}

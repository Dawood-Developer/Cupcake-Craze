using Assets.Scripts.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPlaceHolder : MonoBehaviour
{
    public static SlotPlaceHolder Instance;

    [SerializeField] CupHandler cupHandler;
    [SerializeField]Slot[] Slots;
    // Start is called before the first frame update
    public Transform startPoint; // Starting point of the projectile
    public Transform targetPoint; // Target point for the projectile
    public float height = 5f; // Arc height of the projectile
    public float speed = 2f; // Speed of the projectile

    private Vector3 controlPoint; // Control point for the arc
    private float progress = 0f; // Tracks the progress of the motion
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    public static void LaunchProjectile(GameObject projectile, Vector3 startPoint, Vector3 targetPoint, float height, float speed)
    {
        // Start a coroutine to handle the projectile motion
        projectile.GetComponent<MonoBehaviour>().StartCoroutine(MoveProjectile(projectile, startPoint, targetPoint, height, speed));
    }

    private static System.Collections.IEnumerator MoveProjectile(GameObject projectile, Vector3 startPoint, Vector3 targetPoint, float height, float speed)
    {
        float progress = 0f; // Tracks progress (0 to 1)

        // Calculate control point for the arc
        Vector3 controlPoint = (startPoint + targetPoint) / 2 + Vector3.up * height;

        while (progress < 1f)
        {
            progress += Time.deltaTime * speed; // Increment progress over time

            // Calculate position on the Bezier curve
            Vector3 position = CalculateBezierPoint(progress, startPoint, controlPoint, targetPoint);

            // Move the projectile
            projectile.transform.position = position;

            yield return null; // Wait for the next frame
        }

        // Ensure the projectile ends exactly at the target point
        projectile.transform.position = targetPoint;
    }

    private static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }

    public void RemoveMeBecauseImFull(GameObject go)
    {
        foreach (var slot in Slots)
        {
            if (slot.CheckGameObject() != null)
            {
                if (slot.CheckGameObject().gameObject == go)
                {
                    slot.removeBecauseImFull();
                }
            }
        }
    }

    public bool CheckForGameLoss()
    {
        foreach (var slot in Slots)
        {
            if (slot.CheckGameObject() == null && !slot.isLockedForVideoAd())
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckIfIHaveThatColor(Cup itemClr)
    {
        foreach (var slot in Slots)
        {
            if (slot.CheckGameObject() != null)
            {
                if (slot.CheckGameObject().itemColor == itemClr.itemColor /*&& slot.CheckGameObject().isOnCounterForPacking*/)
                {
                    return slot.CheckGameObject().DoAddCoffeeInBox(itemClr.gameObject);
                    //return true;
                }
            }
        }
        return false;
    }

    float projectileAnimationTime = 2f;
    public void SetBoxOnPlaceHolder(Box go,out bool type)
    {
        type = false;
        foreach (var slot in Slots)
        {
            if (slot.CheckGameObject() == null && !slot.isLockedForVideoAd())
            {
                //go.transform.position = slot.transform.position + slot.offSet;
                type = true;
                LaunchProjectile(go.gameObject, go.transform.position, slot.transform.position + slot.offSet,5f,2f);
                slot.AssignGameObject(go);
                go.OpenBox();
                go.transform.rotation = slot.transform.rotation;
                AudioManager.instance.clickOnBoxPick();
                break;
            }
        }
    }

    public void startPacking()
    {
        
        StartCoroutine(StartCup());

    }
    IEnumerator StartCup()
    {
        yield return new WaitForSeconds(projectileAnimationTime * 0.3f);
        cupHandler.StartCheckingForBoxes();
    }

    //launch projectile and wait for function to complete then make the box bool check of isReadyToPack true then launch start cup,

}
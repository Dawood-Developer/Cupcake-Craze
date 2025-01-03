using Assets.Scripts.Audio;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoxScript : MonoBehaviour
{
    bool isSelected = false;
    bool isAnyOtherBoxOnThisBox = false;

    [SerializeField] Renderer renderer;
    [SerializeField] Renderer rendererLeft;
    [SerializeField] Renderer rendererRight;
    [SerializeField] Collider thisCollider;
    [SerializeField] Transform[] coffeeHolder;
    [SerializeField] Renderer[] childRenderers; // Renderers of specific child objects
    [SerializeField] Material[] originalMaterials; // Store original materials of child objects
    [SerializeField] Material[] instanceMaterials;
    int coffeeHolderIndex = 0;
    //public LayerMask detectionLayer;
    GameObject parent;

    private GameObject other;

    public void OpenBox()
    {
        GetComponent<Animator>()?.SetBool("Open", true);
        StartCoroutine(WaitForAnimationToEnd());
    }
    private IEnumerator WaitForAnimationToEnd()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            while (stateInfo.IsName("Open") && stateInfo.normalizedTime < 1.0f) // Ensure animation is complete
            {
                stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                yield return null;
            }
        }

        Debug.Log("Box animation completed!");
        SlotPlaceHolder.Instance.startPacking();
    }
    public void setMaterial(Material material)
    {
        if (renderer != null && material != null)
        {
            renderer.material = material;
            rendererLeft.material = material;
            rendererRight.material = material;
        }
    }
    private void Awake()
    {
        parent = transform.parent.gameObject;
        //thisCollider = gameObject.transform.GetChild(0).GetComponent<Collider>(); // Automatically get the attached collider
        if (thisCollider == null)
        {
            Debug.LogError("No Collider found on " + gameObject.name + "! Please attach a Collider.");
        }
    }

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        if (thisCollider == null) return; // Exit if there's no collider

        // Use bounds from the current collider for dynamic overlap detection
        Vector3 boxCenter = thisCollider.bounds.center;
        Vector3 boxSize = thisCollider.bounds.size;

        // Perform collision detection within the box boundaries
        Collider[] overlappingColliders = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity);

        if (overlappingColliders.Length > 0)
        {
            bool isAnythingUp = false;
            foreach (Collider collider in overlappingColliders)
            {
                if (collider.gameObject != this.transform & collider.gameObject != thisCollider.gameObject)
                {
                    //isAnyOtherBoxOnThisBox = true;
                    other = collider.gameObject;
                    isAnyOtherBoxOnThisBox = true;
                    isAnythingUp = true;
                   UpdateChildAppearance(isAnyOtherBoxOnThisBox);

                    break;
                }
            }

            if (!isAnythingUp)
            {
                other = null;
                isAnyOtherBoxOnThisBox = false;
                   UpdateChildAppearance(isAnyOtherBoxOnThisBox);

            }
        }

    }
    void UpdateChildAppearance(bool isBlocked)
    {
        for (int i = 0; i < childRenderers.Length; i++)
        {
            if (isBlocked)
            {
                instanceMaterials[i].color = new Color(originalMaterials[i].color.r * 0.5f, originalMaterials[i].color.g * 0.5f, originalMaterials[i].color.b * 0.5f, originalMaterials[i].color.a); // Dull color
            }
            else
            {
                instanceMaterials[i].color = originalMaterials[i].color; // Restore original color
            }
        }
    }
    private void Init()
    {
        childRenderers = new Renderer[3];
        childRenderers[0] = transform.GetChild(0).GetComponent<Renderer>();
        childRenderers[1] = transform.GetChild(1).GetComponent<Renderer>();
        childRenderers[2] = transform.GetChild(2).GetComponent<Renderer>();

        // Initialize material arrays
        originalMaterials = new Material[3];
        instanceMaterials = new Material[3];

        // Clone materials for each child
        for (int i = 0; i < childRenderers.Length; i++)
        {
            originalMaterials[i] = childRenderers[i].material;
            instanceMaterials[i] = new Material(originalMaterials[i]);
            childRenderers[i].material = instanceMaterials[i];
        }
    }

    public bool AddCoffeeOfSameColor(GameObject go)
    {
        if (coffeeHolderIndex >= coffeeHolder.Length)
        {
            return false;
        }
        //go.transform.position = coffeeHolder[coffeeHolderIndex].transform.position;
        SlotPlaceHolder.LaunchProjectile(go, go.transform.position, coffeeHolder[coffeeHolderIndex].transform.position,5,2f);
        go.transform.parent = coffeeHolder[coffeeHolderIndex].transform;
        AudioManager.instance.CoffeeInBox();
        print(coffeeHolderIndex + ":" +coffeeHolder.Length);
        if (++coffeeHolderIndex == coffeeHolder.Length)
        {
            SlotPlaceHolder.Instance.RemoveMeBecauseImFull(parent);
            StartCoroutine(setActive());
            ///do Packages this
            print("do pack this");
        }
        return true;
    }

    IEnumerator setActive()
    {
        yield return new WaitForSeconds(0.4f);
        GetComponent<Animator>()?.SetBool("Close", true);
        yield return new WaitForSeconds(0.4f);
        SlotPlaceHolder.LaunchProjectile(gameObject, gameObject.transform.position, new Vector3(20,20,0), 5, 2f);
    }

    [Button]
    private void onMouseDown()
    {
        print(gameObject.name);
        if (!isSelected && !isAnyOtherBoxOnThisBox)
        {
            SlotPlaceHolder.Instance.SetBoxOnPlaceHolder(this.gameObject.transform.parent.gameObject.GetComponent<Box>(),out isSelected);
            if (gameObject.transform.parent.gameObject.transform.parent.tag == "BeltParent")
            {

                GameManager.instance.lvlManager().BoxesOnConvoyer.Remove(this.gameObject.transform.parent.gameObject.GetComponent<Box>());
                this.gameObject.transform.parent.gameObject.GetComponent<Box>().isOnPacking = true;
                print("aaa" + GameManager.instance.lvlManager().BoxesOnConvoyer.Count);
            }
        }
    }

    public void onClickThisObj()
    {
        onMouseDown();
        if (gameObject.transform.GetComponentInParent<MultiBoxHandler>()!=null)
        {
            gameObject.transform.GetComponentInParent<MultiBoxHandler>().EnableNextBox();
        }
    }
}

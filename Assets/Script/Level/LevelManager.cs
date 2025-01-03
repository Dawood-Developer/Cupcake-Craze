using Assets.Scripts.Audio;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public List<LevelData> levels;    // List of LevelData (box layers)
    public int currentLevelIndex;     // The current level index to load

    public Box boxPrefab;
    public Transform conyerBoxParent;
    public GameObject conveyorBelt;
    public float layerHeightIncrement = 1.0f; // The height increment between layers
    public List<BoxLayer> BoxLayerList = new List<BoxLayer>();
    public List<Box> BoxesOnConvoyer = new List<Box>();

    public CupHandler cupHandler;     // Reference to the CupHandler component


    public void CheckBoxesInLayers(ItemColor itemclr)
    {
        foreach (var layer in BoxLayerList)
        {
            if (layer.SameColorBox(itemclr))
                break;
        }
    }

    [Button]
    public void LoadLevel()
    {
        float yPosition = 10.35f;  // Initial position for the first layer
        if (currentLevelIndex >= 0 && currentLevelIndex < levels.Count)
        {
            foreach (var boxLayer in levels[currentLevelIndex].boxLayer)
            {
                var boxLayerInstance = Instantiate(boxLayer, new Vector3(-1.5f, yPosition, 0f), Quaternion.identity, transform);
                boxLayerInstance.transform.localPosition = new Vector3(-4.5f, yPosition, 6.82f);
                BoxLayerList.Add(boxLayerInstance);
                GenerateCupsForBoxLayer(boxLayerInstance);
                yPosition -= layerHeightIncrement;
            }
        }
        else
        {
            Debug.LogError("Invalid level index.");
        }

        if (levels[currentLevelIndex].conveyerBoxes.Count > 0)
        {
            yPosition -= 6f;
            conveyorBelt.SetActive(true);
            conveyorBelt.transform.localPosition = new Vector3(1, yPosition, -0.73f);
            foreach (ConveyerBoxes item in levels[currentLevelIndex].conveyerBoxes)
            {
                Box boxLayerInstance = Instantiate(boxPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity, conyerBoxParent);
                boxLayerInstance.transform.localPosition = new Vector3(0f, 0f, 0f);
                boxLayerInstance.SetBox(item.BoxSize, item.BoxColor);
                if (boxLayerInstance == null)
                {
                    BoxesOnConvoyer = new List<Box>();
                }
                BoxesOnConvoyer.Add(boxLayerInstance);
                int numCups = item.BoxSize switch
                {
                    Box.BoxSize.FourSized => 4,
                    Box.BoxSize.SixSized => 6,
                    Box.BoxSize.TenSized => 10,
                    _ => 0
                };

                // Delegate cup generation to the CupHandler
                cupHandler.GenerateCups(conyerBoxParent.position, boxLayerInstance.itemColor, numCups);
                cupHandler.PositionCups();
            }
             StartCoroutine(MoveBoxesOneByOne());
        }
    }

    private void Start()
    {
        InitNewLevel();
    }
    public void InitNewLevel()
    {
        StartCoroutine(InitNewLevelCoroutine());
    }
    IEnumerator InitNewLevelCoroutine()
    {
        //confirming currentlevelindex equals to prefs.level
        currentLevelIndex = Prefs.level;
        ClearLevels();
        yield return new WaitForSeconds(3f);
        LoadLevel();
        
    }
    IEnumerator MoveBoxesOneByOne()
    {
        //while (BoxesOnConvoyer.Count != 0)
        //{

        while (BoxesOnConvoyer.Count != 0)
        {

            /*for (int i = 0; i < BoxesOnConvoyer.Count; i++)
            {
                if (BoxesOnConvoyer[i] != null)
                {
                    // Box ko move karna start karein
                    if (BoxesOnConvoyer[i].gameObject)
                    {
                    }*/

                    // Agle box ke liye delay karein
                    StartCoroutine(MoveBox(BoxesOnConvoyer[0]));
                    BoxesOnConvoyer.RemoveAt(0);
                    yield return new WaitForSeconds(1f);
                //}
            //}
            /*if (BoxesOnConvoyer.Count <= 0)
            {
                break;
            }*/
            //}
        }
    }

    IEnumerator MoveBox(Box box)
    {

        if (box != null)
        {
            //bool isLocalPos = false;
            while (box.transform.position.x < 8)
            {
                if (!box.isOnPacking)
                {
                    // Box ko move karte raho
                    box.transform.Translate(Vector3.right * 2f * Time.deltaTime);
                    //isLocalPos = true;
                    yield return null; // Next frame tak wait karo
                }
                else
                {
                    break;
                }
            }

            //if (isLocalPos)
            //{
            if (!box.isOnPacking)
            {
                box.transform.localPosition = new Vector3(0, 0, 0);
                BoxesOnConvoyer.Add(box);
                StartCoroutine(MoveBoxesOneByOne());
            }
                //MoveBox(box);
            //}
            // Box ko wapas start position par le jao jab wo end position par pohonch jaye
        }
    }

    public LayerMask collisionLayers;
    void Update()
    {
        // Check for mouse click or screen touch
        if (Input.GetMouseButtonDown(0) && !GameManager.instance.isAlreadyOpenAnyPanel) // Left mouse button or screen tap
        {
            PerformRaycast(Input.mousePosition); // Use mouse position for raycast
        }
    }

    void PerformRaycast(Vector3 screenPosition)
    {
        // Convert the screen position to a Ray
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red, 5f);
        RaycastHit hit;
        // Perform the raycast
        if (Physics.Raycast(ray, out hit, 10000, collisionLayers))
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name} at {hit.point}");
            OnRaycastHit(hit.collider.gameObject);
            if (hit.collider.gameObject.GetComponent<BoxScript>() != null)
            {
                hit.collider.gameObject.GetComponent<BoxScript>().onClickThisObj();
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object.");
        }
    }

    void OnRaycastHit(GameObject hitObject)
    {
        // Custom behavior when raycast hits an object
        Debug.Log($"You clicked/touched: {hitObject.name}");
    }

    void update()
    {
        for (int i = 0; i < BoxesOnConvoyer.Count; i++)
        {
            if (BoxesOnConvoyer[i] != null)
            {
                // Box ko move karte raho
                BoxesOnConvoyer[i].transform.Translate(Vector3.right * 2f * Time.deltaTime);

                // Agar box end position par pohonch gaya
                if (BoxesOnConvoyer[i].transform.localPosition.x >= 25)
                {
                    // Usko wapas start position par le aao
                    BoxesOnConvoyer[i].transform.localPosition = new Vector3(0, 0, 0);
                }
            }
        }
    }

    void GenerateCupsForBoxLayer(BoxLayer boxLayer)
    {
        foreach (var box in boxLayer.boxList)
        {
            int numCups = box.currentBoxSize switch
            {
                Box.BoxSize.FourSized => 4,
                Box.BoxSize.SixSized => 6,
                Box.BoxSize.TenSized => 10,
                _ => 0
            };

            // Delegate cup generation to the CupHandler
            cupHandler.GenerateCups(boxLayer.transform.position, box.itemColor, numCups);
        }
    }

    [Button]
    public void SwipeBoxesOfAllLayers()
    {
        foreach (var item in BoxLayerList)
        {
            item?.SwapSameSizeBoxes();
        }
    }

    [Button]
    public void ClearLevels()
    {
        cupHandler.ClearCups(); // Clear all cups
        foreach (var boxLayer in BoxLayerList)
        {
            if (boxLayer != null)
            {
                DestroyImmediate(boxLayer.gameObject);
            }
        }
        while (conyerBoxParent.childCount > 0)
        {
            DestroyImmediate(conyerBoxParent.GetChild(0).gameObject);
        }
        BoxesOnConvoyer = new List<Box>();
        conveyorBelt.SetActive(false);
        BoxLayerList.Clear();
    }
}

using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using static Box;
using UnityEngine.UI;

public class BoxLayer : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int gridSize = new Vector2Int(9, 9); // Total grid size (rows x columns)
    public Box boxPrefab;                             // Prefab for the boxes
    public float spacing = 1f;                        // Spacing between boxes

    private Transform boxGridParent;

    // List to store all generated boxes
    public List<Box> boxList = new List<Box>();


    [Button("Generate Grid")]
    public void GenerateGrid()
    {
        if (boxPrefab == null)
        {
            Debug.LogError("Box Prefab is not assigned!");
            return;
        }

        // Create a parent object to store the boxes
        if (boxGridParent == null)
        {
            boxGridParent = new GameObject("BoxGrid").transform;
            boxGridParent.parent = transform;
        }

        // Clear any existing boxes
        ClearChildObjects(boxGridParent);
        boxList.Clear();

        // Loop through the grid size and instantiate boxes
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.y; z++)
            {
                // Instantiate the box prefab
                Box newBox = Instantiate(boxPrefab, boxGridParent);

                // Position the box based on grid coordinates
                newBox.transform.localPosition = new Vector3(x * spacing, 0, z * spacing);
                newBox.GenerateFullRandom();
                // Add the newly created box to the list
                boxList.Add(newBox);

            }
        }
    }

    public Box SameColorBox(ItemColor clr)
    {
        foreach (Box box in boxList)
        {
            if (box.itemColor == clr)
            {
                return box;
            }
        }
        return null;
    }

    [Button]
    public void SwapSameSizeBoxes()
    {
        // Track which sizes have already been processed
        HashSet<BoxSize> processedSizes = new HashSet<BoxSize>();

        foreach (Box boxA in boxList)
        {
            // Skip if this size has already been processed
            if (processedSizes.Contains(boxA.currentBoxSize))
                continue;

            // Find all boxes with the same size as boxA
            List<Box> sameSizeBoxes = boxList.FindAll(box => box.currentBoxSize == boxA.currentBoxSize);

            // Swap positions and rotations of all matching boxes
            for (int i = 0; i < sameSizeBoxes.Count - 1; i += 2)
            {
                Box box1 = sameSizeBoxes[i];
                Box box2 = sameSizeBoxes[i + 1];

                // Start animation coroutine for swapping
                StartCoroutine(AnimateSwap(box1, box2));
            }

            // Mark this size as processed
            processedSizes.Add(boxA.currentBoxSize);
        }
    }

    private System.Collections.IEnumerator AnimateSwap(Box box1, Box box2)
    {
        float duration = 1f; // Duration of the animation in seconds
        float elapsedTime = 0f;

        Vector3 startPosition1 = box1.transform.position;
        Vector3 startPosition2 = box2.transform.position;

        Quaternion startRotation1 = box1.transform.rotation;
        Quaternion startRotation2 = box2.transform.rotation;

        Vector3 startScale1 = box1.transform.localScale;
        Vector3 startScale2 = box2.transform.localScale;

        // Reduce scale to zero at the start
        while (elapsedTime < duration / 2)
        {
            float t = elapsedTime / (duration / 2);
            box1.transform.localScale = Vector3.Lerp(startScale1, Vector3.zero, t);
            box2.transform.localScale = Vector3.Lerp(startScale2, Vector3.zero, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Swap positions and rotations
        box1.transform.position = startPosition2;
        box2.transform.position = startPosition1;
        box1.transform.rotation = startRotation2;
        box2.transform.rotation = startRotation1;

        elapsedTime = 0f;

        // Restore scale back to normal with animation
        while (elapsedTime < duration / 2)
        {
            float t = elapsedTime / (duration / 2);
            box1.transform.localScale = Vector3.Lerp(Vector3.zero, startScale1, t);
            box2.transform.localScale = Vector3.Lerp(Vector3.zero, startScale2, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure scale is fully restored
        box1.transform.localScale = startScale1;
        box2.transform.localScale = startScale2;
    }

    [Button("Clear Grid")]
    public void ClearGrid()
    {
        ClearChildObjects(boxGridParent);
        boxList.Clear();
    }

    private void ClearChildObjects(Transform parent)
    {
        if (parent == null) return;

        // Destroy all child objects
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }
}

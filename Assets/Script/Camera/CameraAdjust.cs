using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CameraAdjust : MonoBehaviour
{
    public float referenceWidth = 1080f;  // Example: Width of reference device (portrait resolution)
    public float referenceHeight = 1920f; // Example: Height of reference device

    void Start()
    {
        AdjustCamera();
    }
    [Button]
    void AdjustCamera()
    {
        // Current screen's aspect ratio
        float screenAspect = (float)Screen.width / Screen.height;

        // Reference aspect ratio
        float referenceAspect = referenceWidth / referenceHeight;

        // Calculate and set orthographic size
        if (screenAspect >= referenceAspect)
        {
            // Wider screens (e.g., tablets, landscape): Adjust based on height
            Camera.main.orthographicSize = referenceHeight / 2f / 100f; // Divide by 100 for Unity units
        }
        else
        {
            // Taller screens (e.g., phones, portrait): Adjust based on width
            float difference = referenceAspect / screenAspect;
            Camera.main.orthographicSize = (referenceHeight / 2f / 100f) * difference;
        }

        print(Screen.width);
        print(Screen.height);
        Debug.Log($"Screen Aspect: {screenAspect}, Orthographic Size: {Camera.main.orthographicSize}, ref");

    }
}
using Sirenix.OdinInspector;
using UnityEngine;

public class Box : MonoBehaviour
{
    public enum BoxSize { FourSized, SixSized, TenSized }

    [Title("Box Configuration")]
    [EnumToggleButtons, HideLabel]
    public BoxSize currentBoxSize;

    [EnumToggleButtons, HideLabel]
    public ItemColor itemColor;

    [Title("Box Models")]
    [Required] public GameObject fourSized;
    [Required] public GameObject sixSized;
    [Required] public GameObject tenSized;


    [Title("Materials"), InlineProperty]
    [Required, Tooltip("Materials should be added in the same order as the BoxColor enum.")]
    public Material[] colorMaterials;

    public bool isOnPacking = false;
    public bool isOnCounterForPacking = false;

    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    private void UpdateBoxState()
    {
        // Activate only the selected box model
        fourSized.SetActive(currentBoxSize == BoxSize.FourSized);
        sixSized.SetActive(currentBoxSize == BoxSize.SixSized);
        tenSized.SetActive(currentBoxSize == BoxSize.TenSized);

        // Get the currently active box
        GameObject activeObject = currentBoxSize switch
        {
            BoxSize.FourSized => fourSized,
            BoxSize.SixSized => sixSized,
            BoxSize.TenSized => tenSized,
            _ => null
        };

            Material selectedMaterial = colorMaterials[(int)itemColor];
            activeObject.GetComponent<BoxScript>().setMaterial(selectedMaterial);
            
    }

    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void GenerateRandomColor()
    {
        itemColor = (ItemColor)Random.Range(0, System.Enum.GetValues(typeof(ItemColor)).Length);
        UpdateBoxState();
    }

    [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void GenerateFullRandom()
    {
        itemColor = (ItemColor)Random.Range(0, System.Enum.GetValues(typeof(ItemColor)).Length);
        currentBoxSize = (BoxSize)Random.Range(0, System.Enum.GetValues(typeof(BoxSize)).Length);
        UpdateBoxState();
    }

    public void SetBox(BoxSize bs, ItemColor ic)
    {
        itemColor = ic;
        currentBoxSize = bs;
        UpdateBoxState();
    }

    public bool DoAddCoffeeInBox(GameObject go)
    {
        switch (currentBoxSize) {
            case BoxSize.FourSized: return fourSized.GetComponent<BoxScript>().AddCoffeeOfSameColor(go); break;
            case BoxSize.SixSized:  return sixSized.GetComponent<BoxScript>().AddCoffeeOfSameColor(go); break;
            case BoxSize.TenSized:  return tenSized.GetComponent<BoxScript>().AddCoffeeOfSameColor(go); break;
            default: return false;
        }
    }

    public void OpenBox()
    {
        GameObject activeObject = currentBoxSize switch
        {
            BoxSize.FourSized => fourSized,
            BoxSize.SixSized => sixSized,
            BoxSize.TenSized => tenSized,
            _ => null
        };

        activeObject?.GetComponent<BoxScript>()?.OpenBox();
    }
}



[System.Serializable]
public enum ItemColor { Orange, Blue, Green, Pink, Purple, Red, Yellow }

using Sirenix.OdinInspector;
using UnityEngine;

public class Cup : MonoBehaviour
{
    [EnumToggleButtons, HideLabel]
    public ItemColor itemColor;


    public MeshRenderer mesh;


    [Title("Materials"), InlineProperty]
    [Required, Tooltip("Materials should be added in the same order as the BoxColor enum.")]
    public Material[] colorMaterials;


    [Button]
    public void SetCupColor(ItemColor color)
    {
        itemColor = color;
        mesh.material =  colorMaterials[(int)itemColor];
    }
}

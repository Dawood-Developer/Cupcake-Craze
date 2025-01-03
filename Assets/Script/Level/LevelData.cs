// LevelData.cs
using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public List<BoxLayer> boxLayer;
    public List<ConveyerBoxes> conveyerBoxes;

}

[Serializable]
public class ConveyerBoxes
{
    public Box.BoxSize BoxSize;
    public ItemColor BoxColor;
}
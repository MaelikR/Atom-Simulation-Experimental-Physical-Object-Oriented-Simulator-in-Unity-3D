// EquipmentSlot.cs
using UnityEngine;

[System.Serializable]
public class EquipmentSlot
{
    public string slotName;
    public Texture2D icon;
    public string itemName;
    public string description;

    public bool IsEquipped => !string.IsNullOrEmpty(itemName);
}

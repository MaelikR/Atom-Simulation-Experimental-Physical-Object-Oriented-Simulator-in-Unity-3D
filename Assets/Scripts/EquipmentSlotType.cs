// =========================
// EQUIPMENT SLOT SYSTEM
// =========================

using UnityEngine;
using System.Collections.Generic;

public enum EquipmentSlotType { Weapon, Head, Chest, Legs, Ring, Amulet }

[System.Serializable]
public class EquipmentItem
{
    public string itemName;
    public EquipmentSlotType slotType;
    public Texture2D icon;

    // Bonus Stats
    public int bonusSTR;
    public int bonusAGI;
    public int bonusINT;
    public int bonusVIT;
    public int bonusSPR;
}

public class EquipmentManager : MonoBehaviour
{
    public Dictionary<EquipmentSlotType, EquipmentItem> equippedItems = new();

    public void Equip(EquipmentItem item)
    {
        equippedItems[item.slotType] = item;
        ApplyStats(item);
    }

    public void Unequip(EquipmentSlotType slot)
    {
        if (equippedItems.TryGetValue(slot, out EquipmentItem item))
        {
            RemoveStats(item);
            equippedItems.Remove(slot);
        }
    }

    private void ApplyStats(EquipmentItem item)
    {
        CharacterStats stats = GetComponent<CharacterStats>();
        stats.Strength += item.bonusSTR;
        stats.Agility += item.bonusAGI;
        stats.Intelligence += item.bonusINT;
        stats.Vitality += item.bonusVIT;
        stats.Spirit += item.bonusSPR;
    }

    private void RemoveStats(EquipmentItem item)
    {
        CharacterStats stats = GetComponent<CharacterStats>();
        stats.Strength -= item.bonusSTR;
        stats.Agility -= item.bonusAGI;
        stats.Intelligence -= item.bonusINT;
        stats.Vitality -= item.bonusVIT;
        stats.Spirit -= item.bonusSPR;
    }
}

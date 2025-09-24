using UnityEngine;
using System;
[CreateAssetMenu(menuName = "RPG Setup/Item Data/Equipment item", fileName = "Equipment data-")]
public class EquipmentData_SO :  ItemDataSO
{
    [Header("item modifiers")]
    public ItemModifier[] modifiers;

}
[Serializable]
public class ItemModifier
{
    public Stats_Type statType;
    public float value;

}
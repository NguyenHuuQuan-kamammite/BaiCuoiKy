using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData 
{
    public int gold;


    public List<Inventory_Item> itemList;
    public SerializableDictionary<string, int> inventory; // itemSaveId -> stackSize
    public SerializableDictionary<string, int> storageItems;
    public SerializableDictionary<string, int> storageMaterials;

    public SerializableDictionary<string, Item_Type> equipedItems; // itemSaveId -> slotType;

    public int skillPoints;
    public SerializableDictionary<string, bool> skillTreeUI; // skill name -> unlock status
    public SerializableDictionary<Skill_Type, SkillUnlock_Type> skillUnlock; // skill type -> Unlock type

    public Vector3 savedCheckpoint;
    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
        storageItems = new SerializableDictionary<string, int>();
        storageMaterials = new SerializableDictionary<string, int>();

        equipedItems = new SerializableDictionary<string, Item_Type>();

        skillTreeUI = new SerializableDictionary<string, bool>();
        skillUnlock = new SerializableDictionary<Skill_Type, SkillUnlock_Type>();
    }
}

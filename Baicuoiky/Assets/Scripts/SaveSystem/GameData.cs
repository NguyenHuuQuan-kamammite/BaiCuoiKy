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

    public SerializableDictionary<string, bool> unlockedCheckpoints; // checkpoint id -> unlocked status
    public SerializableDictionary<string, Vector3> inScenePortals; // scene name > portal position


    public SerializableDictionary<string, bool> completedQuests; // quest save id -> complete status
    public SerializableDictionary<string, int> activeQuests; // active quest save id -> current progress

    public string portalDestinationSceneName;
    public bool returningFromTown;

    public string lastScenePlayed;
    public Vector3 lastPlayerPosition;
    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
        storageItems = new SerializableDictionary<string, int>();
        storageMaterials = new SerializableDictionary<string, int>();

        equipedItems = new SerializableDictionary<string, Item_Type>();

        skillTreeUI = new SerializableDictionary<string, bool>();
        skillUnlock = new SerializableDictionary<Skill_Type, SkillUnlock_Type>();

        unlockedCheckpoints = new SerializableDictionary<string, bool>();
        inScenePortals = new SerializableDictionary<string, Vector3>();

        completedQuests = new SerializableDictionary<string, bool>();
        activeQuests = new SerializableDictionary<string, int>();
    }
}

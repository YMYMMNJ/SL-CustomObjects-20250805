using System;
using System.Collections.Generic;
using UnityEngine;

public class MERTLockerComponent : MERToolboxBlocks.Block
{
    [Serializable]
    public class ChamberItemData
    {
        [Tooltip("The Item to spawn.")]
        public MERTItemType Item;

        [Tooltip("The Amount of items to be spawned.")]
        public int Amount;

        [Range(0f, 100f)]
        [Tooltip("The % Chance this will be selected to spawn.")]
        public int Chance;
    }
    
    [Serializable]
    public class ChamberData
    {
        [HideInInspector]
        public int Chamber;

        [Tooltip("ItemData")]
        public List<ChamberItemData> ItemData;

        [Tooltip("The Permissions needed to open this locker chamber")]
        public DoorPermissionFlags Permissions;
    }
    
    [HideInInspector]
    public LockerType LockerType;
    public List<ChamberData> Chambers;

    [Range(0f, 100f)]
    [Tooltip("The % Chance this locker will spawn")]
    public int Chance;
    
    public override void Compile(Transform rootTransform)
    {
        for (int i = 0; i < Chambers.Count; i++)
            Chambers[i].Chamber = i;

        this.Properties = new Dictionary<string, object>
        {
            { "LockerChambers", Chambers },
            { "LockerType", LockerType },
            { "SpawnChance", Chance },
        };
        base.Compile(rootTransform);
    }
}
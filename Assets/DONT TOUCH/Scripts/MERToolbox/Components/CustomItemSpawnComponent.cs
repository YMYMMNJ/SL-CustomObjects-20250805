using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomItemSpawnComponent : MERToolboxBlocks.Block
{
    [Tooltip("The Id of the CustomItem to spawn. Use the CustomItem name if your using LabApiCustomItemApi")]
    public string CustomItemID;

    [Tooltip("The CustomItem API to use.")]
    public CustomItemType CustomItemType;

    public override void Compile(Transform rootTransform)
    {
        this.Properties = new Dictionary<string, object>
		{
			{ "CustomItemId", CustomItemID },
            { "CustomItemType", CustomItemType },
		};

        base.Compile(rootTransform);
    }
}
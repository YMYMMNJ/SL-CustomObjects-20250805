using System.Collections.Generic;
using UnityEngine;

public class ClutterComponent : MERToolboxBlocks.Block
{
    [Range(0f, 100f)]
    [Tooltip("The spawn chance for this Clutter.")]
    public float SpawnChance;

    [Tooltip("The type of Clutter this is.")]
    public ClutterType ClutterType;

    public override void Compile(Transform rootTransform)
    {
        this.Properties = new Dictionary<string, object>
		{
			{ "SpawnChance", SpawnChance },
            { "ClutterType", ClutterType },
		};

        base.Compile(rootTransform);
    }
}
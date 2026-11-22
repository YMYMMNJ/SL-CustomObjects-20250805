using System;
using UnityEngine;

[ExecuteInEditMode]
public class WaypointComponent : SchematicBlock
{
	public override BlockType BlockType => BlockType.Waypoint;

	public override void Compile(SchematicBlockData block) => base.Compile(block);

	public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
	{
		gameObject = Create<GameObject>("Assets/Resources/Blocks/Waypoint.prefab");
		base.Decompile(ref gameObject, block, parent);
	}

	// Private fields are reset by script recompilation (domain reload) while Start is not called again
	// in edit mode, so the references are re-acquired lazily.
	private void Update()
	{
		if (_filter == null)
			TryGetComponent(out _filter);

		if (_renderer == null)
			TryGetComponent(out _renderer);

		if (_filter != null)
			_filter.hideFlags = HideFlags.HideInInspector;

		if (_renderer != null)
			_renderer.hideFlags = HideFlags.HideInInspector;
	}

	private MeshFilter _filter;
	private MeshRenderer _renderer;

}

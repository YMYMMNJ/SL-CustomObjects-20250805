using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TextComponent : SchematicBlock
{
	public override BlockType BlockType => BlockType.Text;

	private TMP_Text _textMesh;
	private MeshRenderer _renderer;

	// Private fields are reset by script recompilation (domain reload) while Awake is not called again
	// in edit mode, so the references are re-acquired lazily.
	private void CacheReferences()
	{
		if (_textMesh == null)
			TryGetComponent(out _textMesh);

		if (_renderer == null)
			TryGetComponent(out _renderer);
	}

	private void Update()
	{
		CacheReferences();

		if (_textMesh != null)
			_textMesh.margin = Vector4.zero;

		if (_renderer != null)
			_renderer.hideFlags = HideFlags.HideInInspector;
	}

	public override void Compile(SchematicBlockData block)
	{
		CacheReferences();

		block.Properties = new Dictionary<string, object>
		{
			{ "Text", _textMesh.text },
			{ "DisplaySize", (SerializableVector)_textMesh.rectTransform.sizeDelta }
		};

		base.Compile(block);
	}

	public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
	{
		TMP_Text text = Create<GameObject>("Assets/Resources/Blocks/Text.prefab").GetComponent<TMP_Text>();
		gameObject = text.gameObject;

		text.text = Convert.ToString(block.Properties["Text"]);
		text.rectTransform.sizeDelta = JsonConvert.DeserializeObject<Vector2>(block.Properties["DisplaySize"].ToString());

		base.Decompile(ref gameObject, block, parent);
	}
}

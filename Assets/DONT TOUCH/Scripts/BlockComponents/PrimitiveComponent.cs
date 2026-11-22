using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PrimitiveComponent : SchematicBlock
{
    [ColorUsage(true, true)]
    [Tooltip("The color of the primitive. Supports transparent colors.")]
    public Color Color;

    [Tooltip("Whether the primitive should have a collider attached to it.")]
    public bool Collidable = true;

    [Tooltip("Whether the primitive should be visible in game.")]
    public bool Visible = true;

    public override BlockType BlockType => BlockType.Primitive;

    public override void Compile(SchematicBlockData block)
    {
        PrimitiveFlags primitiveFlags = PrimitiveFlags.None;
        if (Collidable)
            primitiveFlags |= PrimitiveFlags.Collidable;

        if (Visible)
            primitiveFlags |= PrimitiveFlags.Visible;

        block.Properties = new Dictionary<string, object>
        {
            { "PrimitiveType", (PrimitiveType)Enum.Parse(typeof(PrimitiveType), tag) },
            { "Color", ColorString },
            { "PrimitiveFlags", primitiveFlags },
        };

        base.Compile(block);
    }

    public override void Decompile(ref GameObject gameObject, SchematicBlockData block, Transform parent)
    {
        PrimitiveType primitiveType = (PrimitiveType)Convert.ToInt32(block.Properties["PrimitiveType"]);
        PrimitiveComponent primitiveComponent = Create<PrimitiveComponent>($"Assets/Resources/Blocks/Primitives/{primitiveType}.prefab");
        gameObject = primitiveComponent.gameObject;

        primitiveComponent.Color = GetColorFromString(block.Properties["Color"].ToString());

        PrimitiveFlags primitiveFlags;
        if (block.Properties.TryGetValue("PrimitiveFlags", out object flags))
        {
            primitiveFlags = (PrimitiveFlags)Convert.ToByte(flags);
        }
        else
        {
            // Backward compatibility
            primitiveFlags = PrimitiveFlags.Visible;
            if (block.Scale.x >= 0f)
                primitiveFlags |= PrimitiveFlags.Collidable;
        }

        primitiveComponent.Collidable = primitiveFlags.HasFlag(PrimitiveFlags.Collidable);
        primitiveComponent.Visible = primitiveFlags.HasFlag(PrimitiveFlags.Visible);

        base.Decompile(ref gameObject, block, parent);
    }

    private string ColorString
    {
        get
        {
            if (Color.r <= 1 && Color.g <= 1 && Color.b <= 1)
                return ColorUtility.ToHtmlStringRGBA(Color);

            return string.Format("{0}:{1}:{2}:{3}", Color.r * 255f, Color.g * 255f, Color.b * 255f, Color.a).Replace(',', '.');
        }
    }

    public static Color GetColorFromString(string colorText)
    {
        Color color = new(-1f, -1f, -1f);
        string[] charTab = colorText.Split(':');
        if (charTab.Length >= 4)
        {
            if (float.TryParse(charTab[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float red))
                color.r = red / 255f;

            if (float.TryParse(charTab[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float green))
                color.g = green / 255f;

            if (float.TryParse(charTab[2], NumberStyles.Any, CultureInfo.InvariantCulture, out float blue))
                color.b = blue / 255f;

            if (float.TryParse(charTab[3], NumberStyles.Any, CultureInfo.InvariantCulture, out float alpha))
                color.a = alpha;

            return color != new Color(-1f, -1f, -1f) ? color : Color.magenta * 3f;
        }

        if (colorText[0] != '#' && colorText.Length == 8)
            colorText = '#' + colorText;

        return ColorUtility.TryParseHtmlString(colorText, out color) ? color : Color.magenta * 3f;
    }

    // Private fields are reset by script recompilation (domain reload) while Awake/Start are not called again
    // in edit mode, so the references are re-acquired lazily instead of only once in Start.
    private bool CacheReferences()
    {
        if (_filter == null)
            TryGetComponent(out _filter);

        if (_renderer == null)
            TryGetComponent(out _renderer);

        if (_sharedRegular == null)
        {
            Material regular = Resources.Load<Material>("Materials/Regular");
            if (regular != null)
                _sharedRegular = new Material(regular);
        }

        if (_sharedTransparent == null)
        {
            Material transparent = Resources.Load<Material>("Materials/Transparent");
            if (transparent != null)
                _sharedTransparent = new Material(transparent);
        }

        return _filter != null && _renderer != null && _sharedRegular != null && _sharedTransparent != null;
    }

    private void Update()
    {
        if (!CacheReferences())
            return;

        _filter.hideFlags = HideFlags.HideInInspector;
        _renderer.hideFlags = HideFlags.HideInInspector;

#if UNITY_EDITOR
        if (EditorUtility.IsPersistent(gameObject))
            return;
#endif

        _renderer.sharedMaterial = Color.a >= 1f ? _sharedRegular : _sharedTransparent;
        _renderer.sharedMaterial.color = Color;

        _renderer.enabled = Visible;
    }

    private void OnDrawGizmos()
    {
        if (Visible)
            return;

        if (_filter == null)
            TryGetComponent(out _filter);

        if (_filter == null)
            return;

        Gizmos.color = new Color(Color.r, Color.g, Color.b, 1f);
        Gizmos.DrawWireMesh(_filter.sharedMesh, 0, transform.position, transform.rotation, transform.localScale);
    }

    internal MeshFilter _filter;
    private MeshRenderer _renderer;
    private Material _sharedRegular;
    private Material _sharedTransparent;
}
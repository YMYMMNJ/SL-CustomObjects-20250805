using UnityEngine;

public class ClutterSerialization : MERToolboxBlocks.Block
{
    public ClutterSerialization(MERToolboxBlocks.Block block)
    {
        this.Name = block.Name;
        this.ObjectId = block.ObjectId;
        this.ParentId = block.ParentId;
        
        transform.GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation);
        this.Position = localPosition;
        this.Rotation = localRotation.eulerAngles;
    }

    public override MERTBlockTypes BlockType { get; set; } = MERTBlockTypes.Clutter;
    public float SpawnChance { get; set; }
    public ClutterType ClutterType { get; set; }
}
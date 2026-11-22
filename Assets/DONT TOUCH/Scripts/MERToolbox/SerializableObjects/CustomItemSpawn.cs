using UnityEngine;

public class CustomItemSpawn : MERToolboxBlocks.Block
{
    public CustomItemSpawn(MERToolboxBlocks.Block block)
    {
        this.Name = block.Name;
        this.ObjectId = block.ObjectId;
        this.ParentId = block.ParentId;
        
        transform.GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation);
        this.Position = localPosition;
        this.Rotation = localRotation.eulerAngles;
    }

    public string CustomItemID { get; set; }
    public CustomItemType CustomItemType { get; set; }
}
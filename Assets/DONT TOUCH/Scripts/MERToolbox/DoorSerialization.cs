using UnityEngine;

public class DoorSerialization : MERToolboxBlocks.Block
{
    public DoorSerialization(MERToolboxBlocks.Block block)
    {
        this.Name = block.Name;
        this.ObjectId = block.ObjectId;
        this.ParentId = block.ParentId;
        
        transform.GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation);
        this.Position = localPosition;
        this.Rotation = localRotation.eulerAngles;
    }
    
    public override MERTBlockTypes BlockType { get; set; } = MERTBlockTypes.Door;

    public MERToolbox.Enums.DoorType DoorType { get; set; } = MERToolbox.Enums.DoorType.EntranceDoor;
    public DoorPermissionFlags Permissions { get; set; } = DoorPermissionFlags.None;
    public bool IsOpen { get; set; } = false;
    public bool IsLocked { get; set; } = false;
    public bool RequireAllPermissions { get; set; } = false;
}
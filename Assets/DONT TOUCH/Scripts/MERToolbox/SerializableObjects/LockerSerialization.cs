using System.Collections.Generic;
using UnityEngine;

public class LockerSerialization : MERToolboxBlocks.Block
{
    public LockerSerialization(MERToolboxBlocks.Block block)
    {
        this.Name = block.Name;
        this.ObjectId = block.ObjectId;
        this.ParentId = block.ParentId;
        
        transform.GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation);
        this.Position = localPosition;
        this.Rotation = localRotation.eulerAngles;
    }
    
    public override MERTBlockTypes BlockType { get; set; } = MERTBlockTypes.Locker;

    public LockerType LockerType;
    public List<LockerType> Chambers;
}
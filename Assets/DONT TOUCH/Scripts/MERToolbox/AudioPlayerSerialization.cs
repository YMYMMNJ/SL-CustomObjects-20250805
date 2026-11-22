using UnityEngine;

public class AudioPlayerSerialization : MERToolboxBlocks.Block
{
    public AudioPlayerSerialization(MERToolboxBlocks.Block block)
    {
        this.Name = block.Name;
        this.ObjectId = block.ObjectId;
        this.ParentId = block.ParentId;
        
        transform.GetLocalPositionAndRotation(out Vector3 localPosition, out Quaternion localRotation);
        this.Position = localPosition;
        this.Rotation = localRotation.eulerAngles;
    }

    public override MERTBlockTypes BlockType { get; set; } = MERTBlockTypes.AudioPlayer;

    public string AudioPath { get; set; }
    public float Volume { get; set; }
    public float AudibleDistance { get; set; }
    public bool Loop { get; set; }
}
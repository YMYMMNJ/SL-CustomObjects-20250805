using System.Collections.Generic;
using UnityEngine;

public class MERTCameraComponent : MERToolboxBlocks.Block
{
    [Tooltip("The field of view of the camera.")]
    public CameraType CameraType;
    [Tooltip("The camera label shown when viewing this camera")]
    public string Label;
	public FacilityZone ZoneIcon;

    public override void Compile(Transform rootTransform)
    {
        this.Properties = new Dictionary<string, object>
        {
            { "CameraType", CameraType },
            { "CameraLabel", Label },
            { "ZoneIcon", ZoneIcon }
        };

        base.Compile(rootTransform);
    }
}
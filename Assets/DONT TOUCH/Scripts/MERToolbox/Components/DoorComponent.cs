using System.Collections.Generic;
using UnityEngine;

namespace MERToolbox.Components
{
    public class DoorComponent : MERToolboxBlocks.Block
    {
        [Tooltip("The type of door to spawn")]
        public DoorType DoorType;

        [Tooltip("Permission flags required to open this door")]
        public DoorPermissionFlags DoorPermissionFlags;

        [Tooltip("Whether the door starts opened when spawned.")]
        public bool IsOpen;

        [Tooltip("Whether the door starts locked when spawned.")]
        public bool IsLocked;

        [Tooltip("If true, the player must have ALL of the specified permissions instead of ANY.")]
        public bool RequireAllPermissions;

        public override void Compile(Transform rootTransform)
        {
            this.Properties = new Dictionary<string, object>
        {
            { "DoorType", DoorType },
            { "DoorPermissionFlags", DoorPermissionFlags },
            { "IsOpen", IsOpen },
            { "IsLocked", IsLocked },
            { "RequireAllPermissions", RequireAllPermissions },
        };

            base.Compile(rootTransform);
        }
    }
}

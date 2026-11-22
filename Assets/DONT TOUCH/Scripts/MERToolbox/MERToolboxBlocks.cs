using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.IO.Compression;
using UnityEngine;
using CompressionLevel = System.IO.Compression.CompressionLevel;

[ExecuteInEditMode]
public class MERToolboxBlocks : MonoBehaviour
{
    [Serializable]
    public class BlockData
    {
        public string Name { get; set; }
        public int ObjectId { get; set; }
        public int ParentId { get; set; }
        public SerializableVector Position { get; set; }
        public SerializableVector Rotation { get; set; }
        public MERTBlockTypes BlockType { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }

    public class Block : MonoBehaviour
    {
        [JsonIgnore]
        public new GameObject gameObject => base.gameObject;
        
        [JsonIgnore]
        public new Transform transform => base.transform;

        public string Name { get; set; }
        public int ObjectId { get; set; }
        public int ParentId { get; set; }

        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public virtual MERTBlockTypes BlockType { get; set; }
        public virtual Dictionary<string, object> Properties { get; set; }

        public virtual void Compile(Transform rootTransform)
        {
            Transform t = transform;
            this.Name = t.name;
            this.ObjectId = t.GetInstanceID();
            this.ParentId = t.parent.GetInstanceID();

            this.Position = rootTransform.InverseTransformPoint(t.position);
            this.Rotation = (Quaternion.Inverse(rootTransform.rotation) * t.rotation).eulerAngles;

            if (this.Properties != null)
            {
                this.Properties.Add("Static", gameObject.isStatic);
            }
            else
            {
                this.Properties = new Dictionary<string, object>()
                {
                    { "Static", gameObject.isStatic }
                };
            }
        }

        public BlockData ToData()
        {
            return new BlockData
            {
                Name = this.Name,
                ObjectId = this.ObjectId,
                ParentId = this.ParentId,
                Position = this.Position,
                Rotation = this.Rotation,
                BlockType = this.BlockType,
                Properties = this.Properties
            };
        }
    }

    readonly List<BlockData> blockDataList = new();
    private static readonly Config Config = SchematicManager.Config;

    public void Compile()
    {
        blockDataList.Clear();
        SetupOutput(out string directoryPath);
        Transform rootTransform = transform;
        
        foreach (Block block in GetComponentsInChildren<Block>())
        {
            block.Compile(rootTransform);
            switch (block)
            {
                case DoorComponent:
                    block.BlockType = MERTBlockTypes.Door;
                    break;
                case ClutterComponent:
                    block.BlockType = MERTBlockTypes.Clutter;
                    break;
                case AudioPlayerComponent:
                    block.BlockType = MERTBlockTypes.AudioPlayer;
                    break;
                case CustomItemSpawnComponent:
                    block.BlockType = MERTBlockTypes.CustomItemSpawner;
                    break;
                case MERTCameraComponent:
                    block.BlockType = MERTBlockTypes.Camera;
                    break;
                case MERTLockerComponent:
                    block.BlockType = MERTBlockTypes.Locker;
                    break;
            }
            blockDataList.Add(block.ToData());
        }

        File.WriteAllText(Path.Combine(directoryPath, $"{name}.json"),
            JsonConvert.SerializeObject(blockDataList, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        
        if (Config.ZipCompiledSchematics)
        {
            ZipFile.CreateFromDirectory(directoryPath, $"{directoryPath}.zip", CompressionLevel.Optimal, true);
            Directory.Delete(directoryPath, true);
        }
    }


    private void SetupOutput(out string directoryPath)
    {
        string parentDirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MERToolbox_Compiled_Items");
        directoryPath = Path.Combine(parentDirectoryPath, name);

        if (!Directory.Exists(parentDirectoryPath))
            Directory.CreateDirectory(parentDirectoryPath);

        if (Directory.Exists(directoryPath))
            DeleteDirectory(directoryPath);

        if (File.Exists($"{directoryPath}.zip"))
            File.Delete($"{directoryPath}.zip");

        Directory.CreateDirectory(directoryPath);
    }

    private static void DeleteDirectory(string path)
    {
        string[] files = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);

        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }

        foreach (string dir in dirs)
        {
            DeleteDirectory(dir);
        }

        Directory.Delete(path, false);
    }
}
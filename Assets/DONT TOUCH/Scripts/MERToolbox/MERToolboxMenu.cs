using UnityEngine;
using UnityEditor;
using DONT_TOUCH.Scripts.Modifiers;
using System.IO;
using System;
using Newtonsoft.Json;

public class MERToolboxMenu : EditorWindow
{
    public static Config Config { get; private set; }

    public static string ConfigPath { get; }

    static MERToolboxMenu()
    {
        ConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "config.json");
        Config = File.Exists(ConfigPath) ? JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath)) : new Config();
    }

    [MenuItem("MERToolbox/Compile %#d")]
    public static void Compile()
    {
        Debug.ClearDeveloperConsole();

        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            if (FindObjectsOfType<ModifierBase>().Length > 0)
            {
                EditorApplication.ExecuteMenuItem("Edit/Play");
                return;
            }
        }

        CompileAll();
    }

    private static void CompileAll()
    {
        foreach (MERToolboxBlocks schematic in FindObjectsOfType<MERToolboxBlocks>())
            schematic.Compile();

        if (Config.OpenDirectoryAfterCompiling)
            OpenDirectory();
    }

    [MenuItem("MERToolbox/Open MERToolbox directory")]
    private static void OpenDirectory()
    {
        if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MERToolbox_Compiled_Items")))
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MERToolbox_Compiled_Items"));

        System.Diagnostics.Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MERToolbox_Compiled_Items"));
    }
}
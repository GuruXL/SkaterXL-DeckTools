using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using DeckTools.UI;
using DeckTools.Presets;
using System;
using Object = UnityEngine.Object;

namespace DeckTools
{
    [EnableReloading]
    internal static class Main
    {
        public static bool enabled;
        public static Harmony harmonyInstance;
        public static string modId = "DeckTools";
        public static UnityModManager.ModEntry modEntry;
        public static Settings settings;
        public static GameObject ScriptManager;
        public static GameObject PresetManager;
        public static DeckTools deckTools;
        public static UIController uiController;

        public static PresetSettings presetSettings;
        public static PresetController PresetCtrl;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
                modEntry.OnGUI = OnGUI;
                modEntry.OnSaveGUI = new System.Action<UnityModManager.ModEntry>(OnSaveGUI);
                modEntry.OnToggle = new System.Func<UnityModManager.ModEntry, bool, bool>(OnToggle);
                modEntry.OnUnload = new System.Func<UnityModManager.ModEntry, bool>(Unload);
                Main.modEntry = modEntry;
                Logger.Log(nameof(Load));
            }
            catch (Exception ex)
            {
                Logger.Error($"Error Loading {modEntry}: {ex.Message}");
                return false;
            }

            return true;
        }
       
        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {

        }
        private static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }
        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (enabled == value)
                return true;

            enabled = value;

            if (enabled)
            {
                try
                {
                    harmonyInstance = new Harmony((modEntry.Info).Id);
                    harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

                    ScriptManager = new GameObject("DeckTools");
                    PresetManager = new GameObject("PresetManager");
                    PresetManager.transform.SetParent(ScriptManager.transform);

                    deckTools = ScriptManager.AddComponent<DeckTools>();
                    uiController = ScriptManager.AddComponent<UIController>();
                    presetSettings = PresetManager.AddComponent<PresetSettings>();
                    PresetCtrl = PresetManager.AddComponent<PresetController>();

                    Object.DontDestroyOnLoad(ScriptManager);
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error during {modEntry} initialization: {ex.Message}");
                    enabled = false; // Rollback enabling if an error occurs
                    return false;
                }
            }
            else
            {
                Unload(modEntry);
            }

            return true;
        }
        public static bool Unload(UnityModManager.ModEntry modEntry)
        {
            try
            {
                harmonyInstance?.UnpatchAll(harmonyInstance.Id);

                if (ScriptManager != null)
                {
                    Object.Destroy(ScriptManager);
                    ScriptManager = null;
                }

                Logger.Log(nameof(Unload));
            }
            catch (Exception ex)
            {
                Logger.Error($"Error during {modEntry} unload: {ex.Message}");
                return false;
            }

            return true;
        }

        public static UnityModManager.ModEntry.ModLogger Logger => modEntry.Logger;
    }
}

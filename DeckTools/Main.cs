using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityModManagerNet;

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
        public static UI ui;

        public static PresetSettings presetSettings;
        public static PresetController PresetCtrl;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            //modEntry.OnGUI = OnGUI;
            //modEntry.OnSaveGUI = new System.Action<UnityModManager.ModEntry>(OnSaveGUI);
            modEntry.OnToggle = new System.Func<UnityModManager.ModEntry, bool, bool>(OnToggle);
            modEntry.OnUnload = new System.Func<UnityModManager.ModEntry, bool>(Unload);
            Main.modEntry = modEntry;
            Logger.Log(nameof(Load));

            return true;
        }
        private static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            bool flag;
            if (enabled == value)
            {
                flag = true;
            }
            else
            {
                enabled = value;
                if (enabled)
                {
                    harmonyInstance = new Harmony((modEntry.Info).Id);
                    harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                    ScriptManager = new GameObject("DeckTools");
                    PresetManager = new GameObject("PresetManager");
                    PresetManager.transform.SetParent(ScriptManager.transform);

                    deckTools = ScriptManager.AddComponent<DeckTools>();
                    ui = ScriptManager.AddComponent<UI>();

                    presetSettings = PresetManager.AddComponent<PresetSettings>();
                    PresetCtrl = PresetManager.AddComponent<PresetController>();

                    Object.DontDestroyOnLoad(ScriptManager);
                }
                else
                {
                    harmonyInstance.UnpatchAll(harmonyInstance.Id);
                    Object.Destroy(ScriptManager);
                }
                flag = true;
            }
            return flag;
        }
        public static bool Unload(UnityModManager.ModEntry modEntry)
        {
            Logger.Log(nameof(Unload));
            return true;
        }

        public static UnityModManager.ModEntry.ModLogger Logger => modEntry.Logger;
    }
}

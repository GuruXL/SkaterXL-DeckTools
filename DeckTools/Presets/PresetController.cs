using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ModIO.UI;

namespace DeckTools
{
    public class PresetController : MonoBehaviour
    {
        private PresetSettings savePreset;
        private PresetSettings loadedPreset;
        private GameObject savePresetGO;
        private GameObject loadedPresetGO;

        private string mainPath;
        public string PresetName = "";
        public string PresetToLoad = "Select Preset to Load";
        private string LastPresetLoaded = "Select Preset to Load";

        private void Start()
        {
            mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SkaterXL\\DeckTools\\");

            if (!Directory.Exists(mainPath + "DeckPresets"))
            {
                Directory.CreateDirectory(mainPath + "DeckPresets");
                MessageSystem.QueueMessage(MessageDisplayData.Type.Info, $"DeckPresets folder created", 2.5f);
                Main.Logger.Log("No DeckPresets Folder Found: new folder has been created");
            }

            CreatePresets();
        }

        private void Update()
        {
            if (PresetToLoad != "Select Preset to Load")
            {
                LoadPreset();
            }
        }

        private void CreatePresets()
        {
            savePresetGO = new GameObject("SavePresetSettings");
            loadedPresetGO = new GameObject("LoadPresetSettings");
            savePresetGO.transform.SetParent(Main.PresetManager.transform);
            loadedPresetGO.transform.SetParent(Main.PresetManager.transform);
            savePreset = savePresetGO.AddComponent<PresetSettings>();
            loadedPreset = loadedPresetGO.AddComponent<PresetSettings>();
            DontDestroyOnLoad(savePresetGO);
            DontDestroyOnLoad(loadedPresetGO);
        }

        public void SavePreset()
        {
            
            savePreset.DeckLocalScale_x = Main.settings.DeckLocalScale_x;
            savePreset.DeckLocalScale_y = Main.settings.DeckLocalScale_y;
            savePreset.DeckLocalScale_z = Main.settings.DeckLocalScale_z;
            savePreset.BackTruckHangerLocalScale_x = Main.settings.BackTruckLocalScale_x;
            savePreset.FrontTruckHangerLocalScale_x = Main.settings.FrontTruckLocalScale_x;
            savePreset.Wheel1LocalScale_x = Main.settings.Wheel1LocalScale_x;
            savePreset.Wheel1Radius = Main.settings.Wheel1Radius;
            savePreset.Wheel2LocalScale_x = Main.settings.Wheel2LocalScale_x;
            savePreset.Wheel2Radius = Main.settings.Wheel2Radius;
            savePreset.Wheel3LocalScale_x = Main.settings.Wheel3LocalScale_x;
            savePreset.Wheel3Radius = Main.settings.Wheel3Radius;
            savePreset.Wheel4LocalScale_x = Main.settings.Wheel4LocalScale_x;
            savePreset.Wheel4Radius = Main.settings.Wheel4Radius;

            string json = JsonUtility.ToJson(savePreset);
            File.WriteAllText(mainPath + "DeckPresets\\" + $"{PresetName}.json", json);

            MessageSystem.QueueMessage(MessageDisplayData.Type.Success, $"{PresetName} Preset Created", 2.5f);

            PresetName = "";
        }

        public void ResetPresetList()
        {
            PresetToLoad = "Select Preset to Load";
            LastPresetLoaded = "Select Preset to Load";
        }

        private void LoadPreset()
        {
            if (LastPresetLoaded != PresetToLoad)
            {
                string json = File.ReadAllText(mainPath + "DeckPresets\\" + $"{PresetToLoad}.json");
                JsonUtility.FromJsonOverwrite(json, loadedPreset);

                MessageSystem.QueueMessage(MessageDisplayData.Type.Info, $"{PresetToLoad} Preset Loaded", 2.5f);

                LastPresetLoaded = PresetToLoad;
            }
        }

        public void ApplyPreset()
        {
            if (PresetToLoad != "Select Preset to Load")
            {
               
                Main.settings.DeckLocalScale_x = loadedPreset.DeckLocalScale_x;
                Main.settings.DeckLocalScale_y = loadedPreset.DeckLocalScale_y;
                Main.settings.DeckLocalScale_z = loadedPreset.DeckLocalScale_z;
                Main.settings.BackTruckLocalScale_x = loadedPreset.BackTruckHangerLocalScale_x;
                Main.settings.FrontTruckLocalScale_x = loadedPreset.FrontTruckHangerLocalScale_x;
                Main.settings.Wheel1LocalScale_x = loadedPreset.Wheel1LocalScale_x;
                Main.settings.Wheel1Radius = loadedPreset.Wheel1Radius;
                Main.settings.Wheel2LocalScale_x = loadedPreset.Wheel2LocalScale_x;
                Main.settings.Wheel2Radius = loadedPreset.Wheel2Radius;
                Main.settings.Wheel3LocalScale_x = loadedPreset.Wheel3LocalScale_x;
                Main.settings.Wheel3Radius = loadedPreset.Wheel3Radius;
                Main.settings.Wheel4LocalScale_x = loadedPreset.Wheel4LocalScale_x;
                Main.settings.Wheel4Radius = loadedPreset.Wheel4Radius;

                MessageSystem.QueueMessage(MessageDisplayData.Type.Success, $"{PresetToLoad} Preset Applied", 2.5f);
            }
        }

        public string[] GetPresetNames()
        {
            string[] NullState = new string[] { "Select Preset to Load" };

            if (mainPath != null)
            {
                string[] jsons = Directory.GetFiles(mainPath + "DeckPresets\\", "*.json");
                string[] names = new string[jsons.Length];

                int i = 0;
                foreach (string name in jsons)
                {
                    names[i] = Path.GetFileNameWithoutExtension(name);
                    i++;
                }
                if (i > 0)
                {
                    return names;
                }
                else
                {
                    return NullState;
                }
            }
            return NullState;
        }
    }
}

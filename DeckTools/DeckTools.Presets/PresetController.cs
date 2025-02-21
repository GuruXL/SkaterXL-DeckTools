using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ModIO.UI;

namespace DeckTools.Presets
{
    public class PresetController : MonoBehaviour
    {
        private PresetSettings savePresetSettings = new PresetSettings();
        private PresetSettings loadPresetSettings = new PresetSettings();

        private string mainPath;
        public string presetName = "";
        public string presetToLoad = "Select Preset to Load";
        private string lastPresetLoaded = "Select Preset to Load";

        private void Start()
        {
            mainPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SkaterXL\\DeckTools\\");

            if (!Directory.Exists(mainPath + "DeckPresets"))
            {
                Directory.CreateDirectory(mainPath + "DeckPresets");
                MessageSystem.QueueMessage(MessageDisplayData.Type.Info, $"DeckPresets folder created", 2.5f);
                Main.Logger.Log("No DeckPresets Folder Found: new folder has been created");
            }
        }

        private void Update()
        {
            if (presetToLoad != "Select Preset to Load")
            {
                LoadPreset();
            }
        }

        public void SavePreset()
        {

            savePresetSettings.DeckLocalScale_x = Main.settings.DeckLocalScale_x;
            savePresetSettings.DeckLocalScale_y = Main.settings.DeckLocalScale_y;
            savePresetSettings.DeckLocalScale_z = Main.settings.DeckLocalScale_z;
            savePresetSettings.BackTruckHangerLocalScale_x = Main.settings.BackTruckLocalScale_x;
            savePresetSettings.FrontTruckHangerLocalScale_x = Main.settings.FrontTruckLocalScale_x;
            savePresetSettings.Wheel1LocalScale_x = Main.settings.Wheel1LocalScale_x;
            savePresetSettings.Wheel1Radius = Main.settings.Wheel1Radius;
            savePresetSettings.Wheel2LocalScale_x = Main.settings.Wheel2LocalScale_x;
            savePresetSettings.Wheel2Radius = Main.settings.Wheel2Radius;
            savePresetSettings.Wheel3LocalScale_x = Main.settings.Wheel3LocalScale_x;
            savePresetSettings.Wheel3Radius = Main.settings.Wheel3Radius;
            savePresetSettings.Wheel4LocalScale_x = Main.settings.Wheel4LocalScale_x;
            savePresetSettings.Wheel4Radius = Main.settings.Wheel4Radius;

            string json = JsonUtility.ToJson(savePresetSettings);
            File.WriteAllText(mainPath + "DeckPresets\\" + $"{presetName}.json", json);

            MessageSystem.QueueMessage(MessageDisplayData.Type.Success, $"{presetName} Preset Created", 2.5f);

            presetName = "";
        }

        public void ResetPresetList()
        {
            presetToLoad = "Select Preset to Load";
            lastPresetLoaded = "Select Preset to Load";
        }

        private void LoadPreset()
        {
            if (lastPresetLoaded != presetToLoad)
            {
                string json = File.ReadAllText(mainPath + "DeckPresets\\" + $"{presetToLoad}.json");
                JsonUtility.FromJsonOverwrite(json, loadPresetSettings);

                MessageSystem.QueueMessage(MessageDisplayData.Type.Info, $"{presetToLoad} Preset Loaded", 2.5f);

                lastPresetLoaded = presetToLoad;
            }
        }

        public void ApplyPreset()
        {
            if (presetToLoad != "Select Preset to Load")
            {
               
                Main.settings.DeckLocalScale_x = loadPresetSettings.DeckLocalScale_x;
                Main.settings.DeckLocalScale_y = loadPresetSettings.DeckLocalScale_y;
                Main.settings.DeckLocalScale_z = loadPresetSettings.DeckLocalScale_z;
                Main.settings.BackTruckLocalScale_x = loadPresetSettings.BackTruckHangerLocalScale_x;
                Main.settings.FrontTruckLocalScale_x = loadPresetSettings.FrontTruckHangerLocalScale_x;
                Main.settings.Wheel1LocalScale_x = loadPresetSettings.Wheel1LocalScale_x;
                Main.settings.Wheel1Radius = loadPresetSettings.Wheel1Radius;
                Main.settings.Wheel2LocalScale_x = loadPresetSettings.Wheel2LocalScale_x;
                Main.settings.Wheel2Radius = loadPresetSettings.Wheel2Radius;
                Main.settings.Wheel3LocalScale_x = loadPresetSettings.Wheel3LocalScale_x;
                Main.settings.Wheel3Radius = loadPresetSettings.Wheel3Radius;
                Main.settings.Wheel4LocalScale_x = loadPresetSettings.Wheel4LocalScale_x;
                Main.settings.Wheel4Radius = loadPresetSettings.Wheel4Radius;

                MessageSystem.QueueMessage(MessageDisplayData.Type.Success, $"{presetToLoad} Preset Applied", 2.5f);
            }
        }

        public string[] GetPresetNames()
        {
            string[] empty = new string[] { "Select Preset to Load" };

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
                    return empty;
                }
            }
            return empty;
        }
    }
}

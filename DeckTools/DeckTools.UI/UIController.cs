using UnityEngine;
using RapidGUI;
using UnityEngine.SceneManagement;
using GameManagement;
using UnityEngine.UI;

namespace DeckTools.UI
{
    public class UItab // UI dropdown tabs class
    {
        public bool reference;
        public string text;
        public int font;

        public UItab(bool reference, string text, int font)
        {
            this.reference = reference;
            this.text = text;
            this.font = font;
        }
    }

    public class UIController : MonoBehaviour
    {
        private Rect WindowBox = new Rect(20, 20, Screen.width / 8, 20);
        private bool showMainMenu = false;

        private UItab Settings_Tab = new UItab(true, "Settings", 14);
        private UItab Presets_Tab = new UItab(true, "Presets", 14);

        private string Grey = "#adadad";
        private string LightBlue = "#30e2e6";
        private string TabColor;

        private void Update()
        {
            if ((Input.GetKey(KeyCode.LeftControl) | Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(Main.settings.Hotkey.keyCode))
            {
                if (showMainMenu)
                {
                    Close();
                }
                else
                {
                    Open();
                }
            }

            if (showMainMenu)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        private void Open()
        {
            showMainMenu = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void Close()
        {
            showMainMenu = false;
            Cursor.visible = false;
            Main.settings.Save(Main.modEntry);
        }

        private void OnGUI()
        {
            if (!showMainMenu)
                return;
            GUI.backgroundColor = Main.settings.BGColor;
            WindowBox = GUILayout.Window(7448763, WindowBox, MainWindow, "<b> Deck Tools </b>");

        }
        // Creates the GUI window
        private void MainWindow(int windowID)
        {
            GUI.backgroundColor = Main.settings.BGColor;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            MainUI();
            if (!Main.settings.enabled)
                return;

            SettingsUI();
            PresetUI();

        }

        private void MainUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(Main.settings.enabled ? "<b><color=#7CFC00> Enabled </color></b>" : "<b><color=#b71540> Disabled </color></b>");
            if (RGUI.Button(Main.settings.enabled, ""))
            {
                Main.settings.enabled = !Main.settings.enabled;
            }
            GUILayout.EndHorizontal();
        }
        private void Tabs(UItab obj, string color = "#e6ebe8")
        {
            if (GUILayout.Button($"<size={obj.font}><color={color}>" + (obj.reference ? "-" : "<b>+</b>") + obj.text + "</color>" + "</size>", "Label"))
            {
                obj.reference = !obj.reference;
                WindowBox.height = 20;
                WindowBox.width = Screen.width / 8;

            }
        }
        private void TextSwitch(UItab Tab)
        {
            switch (Tab.reference)
            {
                case true:
                    TabColor = Grey;
                    break;

                case false:
                    TabColor = LightBlue;
                    break;
            }
        }

        private void SettingsUI()
        {
            TextSwitch(Settings_Tab);
            Tabs(Settings_Tab, TabColor);
            if (!Settings_Tab.reference)
            {
                GUILayout.BeginVertical("Box");
                GUILayout.Label("Settings");
                Main.settings.DeckLocalScale_x = RGUI.SliderFloat(Main.settings.DeckLocalScale_x, 0.0f, 2.0f, 1.0f, " Deck Width");
                Main.settings.DeckLocalScale_y = RGUI.SliderFloat(Main.settings.DeckLocalScale_y, 0.0f, 2.0f, 1.0f, " Deck Length");
                Main.settings.DeckLocalScale_z = RGUI.SliderFloat(Main.settings.DeckLocalScale_z, 0.0f, 2.0f, 1.0f, " Deck Height");
                GUILayout.Space(10);
                Main.settings.BackTruckLocalScale_x = RGUI.SliderFloat(Main.settings.BackTruckLocalScale_x, 0.0f, 2.0f, 1.0f, " Back Truck Width");
                Main.settings.FrontTruckLocalScale_x = RGUI.SliderFloat(Main.settings.FrontTruckLocalScale_x, 0.0f, 2.0f, 1.0f, " Front Truck Width");
                GUILayout.Space(10);
                //Main.settings.truckSpring = RGUI.SliderFloat(Main.settings.truckSpring, 0f, 20f, 10f, " Truck Spring");
                //Main.settings.truckDamping = RGUI.SliderFloat(Main.settings.truckDamping, 0f, 4f, 2f, " Truck Damping");
                Main.settings.truckTightness = RGUI.SliderFloat(Main.settings.truckTightness, 0f, 2.5f, 1f, " Truck Tightness");
                GUILayout.Space(10);
                Main.settings.Wheel1LocalScale_x = RGUI.SliderFloat(Main.settings.Wheel1LocalScale_x, 0.0f, 3.0f, 1.0f, " Wheel1 Width");
                Main.settings.Wheel1Radius = RGUI.SliderFloat(Main.settings.Wheel1Radius, 0.0f, 2.0f, 1.0f, " Wheel1 Radius");
                Main.settings.Wheel2LocalScale_x = RGUI.SliderFloat(Main.settings.Wheel2LocalScale_x, 0.0f, 3.0f, 1.0f, " Wheel2 Width");
                Main.settings.Wheel2Radius = RGUI.SliderFloat(Main.settings.Wheel2Radius, 0.0f, 2.0f, 1.0f, " Wheel2 Radius");
                Main.settings.Wheel3LocalScale_x = RGUI.SliderFloat(Main.settings.Wheel3LocalScale_x, 0.0f, 3.0f, 1.0f, " Wheel3 Width");
                Main.settings.Wheel3Radius = RGUI.SliderFloat(Main.settings.Wheel3Radius, 0.0f, 2.0f, 1.0f, " Wheel3 Radius");
                Main.settings.Wheel4LocalScale_x = RGUI.SliderFloat(Main.settings.Wheel4LocalScale_x, 0.0f, 2.0f, 1.0f, " Wheel4 Width");
                Main.settings.Wheel4Radius = RGUI.SliderFloat(Main.settings.Wheel4Radius, 0.0f, 2.0f, 1.0f, " Wheel4 Radius");
                GUILayout.EndVertical();
            }
        }

        private void PresetUI()
        {
            TextSwitch(Presets_Tab);
            Tabs(Presets_Tab, TabColor);
            if (!Presets_Tab.reference)
            {
                GUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.cyan;
                if (GUILayout.Button("Save Preset", RGUIStyle.button, GUILayout.Width(98)))
                {
                    Main.presetController.SavePreset();
                }
                GUI.backgroundColor = Color.cyan;
                Main.presetController.presetName = RGUI.Field(Main.presetController.presetName, "");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Apply Preset", RGUIStyle.button, GUILayout.Width(98)))
                {
                    Main.presetController.ApplyPreset();
                    Main.presetController.ResetPresetList();
                }
                Main.presetController.presetToLoad = RGUI.SelectionPopup(Main.presetController.presetToLoad, Main.presetController.GetPresetNames());
                GUILayout.EndHorizontal();
            }
        }         
    }
}

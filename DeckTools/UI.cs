﻿using UnityEngine;
using RapidGUI;
using UnityEngine.SceneManagement;
using GameManagement;
using UnityEngine.UI;

namespace DeckTools
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

    public class UI : MonoBehaviour
    {
        private Rect WindowBox = new Rect(20, 20, Screen.width / 6, 20);
        private bool showMainMenu = false;

        private UItab Settings_Tab = new UItab(true, "Settings", 14);
        private UItab Presets_Tab = new UItab(true, "Presets", 14);
        private UItab Deck_Tab = new UItab(true, "Deck", 13);
        private UItab Trucks_Tab = new UItab(true, "Trucks", 13);
        private UItab Wheels_Tab = new UItab(true, "Wheels", 13);

        //private string white = "#e6ebe8";
        private string Grey = "#adadad";
        private string LightBlue = "#30e2e6";
        //private string DarkBlue = "#3347ff";
        private string green = "#7CFC00";
        //private string red = "#b71540";
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
            WindowBox = GUILayout.Window(7448763, WindowBox, MainWindow, "<b> Deck Tools</b>");

        }
        // Creates the GUI window
        private void MainWindow(int windowID)
        {
            GUI.backgroundColor = Main.settings.BGColor;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            GUILayout.Label("<b> v0.0.2 </b>");
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

            // Resets Toggles
            if (!Main.settings.enabled)
            {

            }

        }
        private void Tabs(UItab obj, string color = "#e6ebe8")
        {
            if (GUILayout.Button($"<size={obj.font}><color={color}>" + (obj.reference ? "-" : "<b>+</b>") + obj.text + "</color>" + "</size>", "Label"))
            {
                obj.reference = !obj.reference;
                WindowBox.height = 20;
                WindowBox.width = Screen.width / 6;

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
                GUILayout.BeginVertical("Box"); // start container
                GUILayout.Label("Settings");

                TextSwitch(Deck_Tab);
                Tabs(Deck_Tab, TabColor);
                if (!Deck_Tab.reference)
                {
                    GUILayout.BeginVertical("Box");
                    string WidthRealValue = (Main.settings.DeckLocalScale_x * 7.55f).ToString("0.00") + "\"";
                    Main.settings.DeckLocalScale_x = RGUI.SliderFloat(Main.settings.DeckLocalScale_x, 0.3f, 2.0f, 1.0f, " Deck Width: " + $"<color={green}> {WidthRealValue} </color>");

                    string LengthRealValue = (Main.settings.DeckLocalScale_y * 30.86f).ToString("0.00") + "\"";
                    Main.settings.DeckLocalScale_y = RGUI.SliderFloat(Main.settings.DeckLocalScale_y, 0.8f, 2.0f, 1.0f, " Deck Length: " + $"<color={green}> {LengthRealValue} </color>");

                    Main.settings.DeckLocalScale_z = RGUI.SliderFloat(Main.settings.DeckLocalScale_z, 0.0f, 2.0f, 1.0f, " Deck Height");
                    GUILayout.EndVertical();
                }

                TextSwitch(Trucks_Tab);
                Tabs(Trucks_Tab, TabColor);
                if (!Trucks_Tab.reference)
                {
                    GUILayout.BeginVertical("Box");
                    string BTrealValue = (Main.settings.BackTruckLocalScale_x * 7.55f).ToString("0.00") + "\"";
                    Main.settings.BackTruckLocalScale_x = RGUI.SliderFloat(Main.settings.BackTruckLocalScale_x, 0.5f, 2.0f, 1.0f, " Truck 1 Width: " + $"<color={green}> {BTrealValue} </color>");

                    string FTrealValue = (Main.settings.FrontTruckLocalScale_x * 7.55f).ToString("0.00") + "\"";
                    Main.settings.FrontTruckLocalScale_x = RGUI.SliderFloat(Main.settings.FrontTruckLocalScale_x, 0.5f, 2.0f, 1.0f, " Truck 2 Width: " + $"<color={green}> {FTrealValue} </color>");
                    GUILayout.Space(4);

                    float percentage = Main.settings.truckTightness / 2f;
                    string TightnessPercent = Mathf.Round(percentage * 100f).ToString() + "%";
                    Main.settings.truckTightness = RGUI.SliderFloat(Main.settings.truckTightness, 0f, 2f, 1f, " Truck Tightness: " + $"<color={green}> {TightnessPercent} </color>");
                    GUILayout.Space(8);
                    GUILayout.EndVertical();
                }

                TextSwitch(Wheels_Tab);
                Tabs(Wheels_Tab, TabColor);
                if (!Wheels_Tab.reference)
                {
                    GUILayout.BeginVertical();
                    GUILayout.Label("<i>All wheels will scale to selection</i>");
                    GUILayout.EndVertical();
                    GUILayout.BeginHorizontal();
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Scale All Wheels", RGUIStyle.button, GUILayout.Width(120)))
                    {
                        if (Main.settings.WheelScaleTarget != "Select Wheel")
                        {
                            Main.deckTools.ScaleAllWheels();
                            Main.settings.resetWheelTarget();
                        }
                    }
                    Main.settings.WheelScaleTarget = RGUI.SelectionPopup(Main.settings.WheelScaleTarget, Main.settings.WheelList);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginVertical("Box");
                    GUILayout.Space(6);
                    GUILayout.Label("Wheel 1");
                    Main.settings.Wheel1LocalScale_x = RGUI.SliderFloat(Main.settings.Wheel1LocalScale_x, 0.2f, 3.0f, 1.0f, " Width");
                    Main.settings.Wheel1Radius = RGUI.SliderFloat(Main.settings.Wheel1Radius, 0.4f, 1.8f, 1.0f, " Size");
                    GUILayout.Space(6);
                    GUILayout.Label("Wheel 2");
                    Main.settings.Wheel2LocalScale_x = RGUI.SliderFloat(Main.settings.Wheel2LocalScale_x, 0.2f, 3.0f, 1.0f, " Width");
                    Main.settings.Wheel2Radius = RGUI.SliderFloat(Main.settings.Wheel2Radius, 0.4f, 1.8f, 1.0f, " Size");
                    GUILayout.Space(6);
                    GUILayout.Label("Wheel 3");
                    Main.settings.Wheel3LocalScale_x = RGUI.SliderFloat(Main.settings.Wheel3LocalScale_x, 0.2f, 3.0f, 1.0f, " Width");
                    Main.settings.Wheel3Radius = RGUI.SliderFloat(Main.settings.Wheel3Radius, 0.4f, 1.8f, 1.0f, " Size");
                    GUILayout.Space(6);
                    GUILayout.Label("Wheel 4");
                    Main.settings.Wheel4LocalScale_x = RGUI.SliderFloat(Main.settings.Wheel4LocalScale_x, 0.2f, 3.0f, 1.0f, " Width");
                    Main.settings.Wheel4Radius = RGUI.SliderFloat(Main.settings.Wheel4Radius, 0.4f, 1.8f, 1.0f, " Size");
                    GUILayout.Space(6);
                    GUILayout.EndVertical();
                }
                GUILayout.EndVertical(); // end container
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
                    Main.PresetCtrl.SavePreset();
                }
                GUI.backgroundColor = Color.cyan;
                Main.PresetCtrl.PresetName = GUILayout.TextField(Main.PresetCtrl.PresetName, GUILayout.Width(260));
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Apply Preset", RGUIStyle.button, GUILayout.Width(98)))
                {
                    Main.PresetCtrl.ApplyPreset();
                    Main.PresetCtrl.ResetPresetList();
                }
                Main.PresetCtrl.PresetToLoad = RGUI.SelectionPopup(Main.PresetCtrl.PresetToLoad, Main.PresetCtrl.GetPresetNames());
                GUILayout.EndHorizontal();
            }
        }
    }
}

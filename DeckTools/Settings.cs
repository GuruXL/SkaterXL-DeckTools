using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityModManagerNet;

namespace DeckTools
{
    [Serializable]
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        public static Settings Instance { get; set; }

        // ----- Start Set KeyBindings ------
        public KeyBinding Hotkey = new KeyBinding { keyCode = KeyCode.D };

        private static readonly KeyCode[] keyCodes = Enum.GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Where(k => ((int)k < (int)KeyCode.Mouse0))
            .ToArray();

        // Get Key on KeyPress
        public static KeyCode? GetCurrentKeyDown()
        {
            if (!Input.anyKeyDown)
            {
                return null;
            }

            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKey(keyCodes[i]))
                {
                    return keyCodes[i];
                }
            }
            return null;
        }
        // -------- End KeyBind ------------

        public Color BGColor = new Color(0.0f, 0.0f, 0.0f);
        public bool enabled = false;

        public float DeckLocalScale_x = 1f;
        public float DeckLocalScale_y = 1f;
        public float DeckLocalScale_z = 1f;
        public float BackTruckLocalScale_x = 1f;
        public float FrontTruckLocalScale_x = 1f;
        public float Wheel1LocalScale_x = 1f;
        public float Wheel1Radius = 1f;
        public float Wheel2LocalScale_x = 1f;
        public float Wheel2Radius = 1f;
        public float Wheel3LocalScale_x = 1f;
        public float Wheel3Radius = 1f;
        public float Wheel4LocalScale_x = 1f;
        public float Wheel4Radius = 1f;

        // Deck Settings
        public float frontTruckAttatchPoint = 0.21f;
        public float backTruckAttatchPoint = -0.21f;
        public float length = 0.82f;
        public float width = 0.21f;
        public float thickness = 0.021f;
        public float noseLength = 0.13f;
        public float tailLength = 0.13f;
        public float noseHeight = 0.04f;
        public float tailHeight = 0.04f;
        public float concaveDepth = 0.01f;

        public float FT_Angle = 145f;
        public float FT_Spring = 1f;
        public float FT_Damper = 0.02f;
        public float FT_height = 0.06f;
        public float FT_width = 1.8f;
        public float FT_depth = 0.07f;

        public float BT_Angle = 145f;
        public float BT_Spring = 1f;
        public float BT_Damper = 0.02f;
        public float BT_height = 0.058f;
        public float BT_width = 1.8f;
        public float BT_depth = 0.07f;

        public float truckTightness = 1f;
        public float truckSpring = 10f;
        public float truckDamping = 2f;

        public float WheelRadius = 0.0275f;

        public void OnChange()
        {
            throw new NotImplementedException();
        }
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}

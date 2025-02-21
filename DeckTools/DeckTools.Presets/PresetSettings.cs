using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace DeckTools.Presets
{
    [Serializable]
    public class PresetSettings
    {
        [SerializeField]
        public float DeckLocalScale_x;
        [SerializeField]
        public float DeckLocalScale_y;
        [SerializeField]
        public float DeckLocalScale_z;
        [SerializeField]
        public float BackTruckHangerLocalScale_x;
        [SerializeField]
        public float FrontTruckHangerLocalScale_x;
        [SerializeField]
        public float Wheel1LocalScale_x;
        [SerializeField]
        public float Wheel1Radius;
        [SerializeField]
        public float Wheel2LocalScale_x;
        [SerializeField]
        public float Wheel2Radius;
        [SerializeField]
        public float Wheel3LocalScale_x;
        [SerializeField]
        public float Wheel3Radius;
        [SerializeField]
        public float Wheel4LocalScale_x;
        [SerializeField]
        public float Wheel4Radius;
        [SerializeField]
        public float truckTightness;
    }
}

using HarmonyLib;
using System.Threading.Tasks;
using UnityEngine;
using SkaterXL.Gameplay;
using SkaterXL.Multiplayer;
using SkaterXL.Data;
using ReplayEditor;
using GameManagement;
using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;

namespace DeckTools
{
    public class DeckTools : MonoBehaviour
    {

        Transform DeckParent;

        public BoxCollider Deck_Collider1;
        public BoxCollider Deck_Collider2;
        public BoxCollider Deck_Collider3;

        Transform BackTruckHanger;
        Transform FrontTruckHanger;

        CapsuleCollider BackTruckCollider;
        CapsuleCollider FrontTruckCollider;

        private float DefaultTruckColliderHeight = 0.16f;
        //private float DefaultWheelColliderRadius = 0.0265f;

        Transform Wheel1;
        Transform Wheel2;
        Transform Wheel3;
        Transform Wheel4;

        Vector3[] DefaultWheelPos = new Vector3[4];
        Vector3[] Default_Replay_WheelPos = new Vector3[4];

        SphereCollider Wheel1Collider;
        SphereCollider Wheel2Collider;
        SphereCollider Wheel3Collider;
        SphereCollider Wheel4Collider;

        Transform Replay_DeckParent;
        Transform Replay_BackTruckHanger;
        Transform Replay_FrontTruckHanger;
        Transform Replay_Wheel1;
        Transform Replay_Wheel2;
        Transform Replay_Wheel3;
        Transform Replay_Wheel4;

        /*
        Transform Multi_DeckParent;
        Transform Multi_BackTruckHanger;
        Transform Multi_FrontTruckHanger;
        Transform Multi_Wheel1;
        Transform Multi_Wheel2;
        Transform Multi_Wheel3;
        Transform Multi_Wheel4;
        */

        private void Start()
        {
            GetDeck();
            GetReplayDeck();
        }

        private void Update()
        {
            if (!Main.settings.enabled)
            {
                return;
            }
            /*
            else if(MultiplayerManager.Instance.InRoom)
            {
                if (MultiDeckFound() == true)
                {
                    UpdateMultiSettings();
                }
            }
            */
            else
            {
                switch (GameStateMachine.Instance.CurrentState)
                {
                    case PlayState playState:
                        UpdateSettings();
                        UpdateWheelPos();
                        UpdateDeckColliders();
                        UpdateTruckCollider();
                        UpdateWheelCollider();
                        break;

                    case ReplayState replayState:
                        UpdateReplaySettings();
                        UpdateReplayWheelPos();
                        break;
                }
            }
        }

        /*
        private void FixedUpdate()
        {
            
            if (MultiplayerManager.Instance.InRoom)
            {
                if (MultiDeckFound() == false)
                {
                    GetMultiDeck();
                }
            }
            else if (!MultiplayerManager.Instance.InRoom)
            {
                if (MultiDeckFound() == true)
                {
                    ResetMultiDeck();
                }
            }
            
        }
        */

        // ------------------------ Get Components ------------------------
        private void GetDeck()
        {
            // GameStateMachine.Instance.MainPlayer.gameplay.playerData.board
            // GameStateMachine.Instance.MainPlayer.gameplay.transformReference.boardTransform.localScale
            DeckParent = GameStateMachine.Instance.MainPlayer.characterCustomizer.DeckParent;
            BackTruckHanger = GameStateMachine.Instance.MainPlayer.characterCustomizer.TruckHangerParents[0];
            FrontTruckHanger = GameStateMachine.Instance.MainPlayer.characterCustomizer.TruckHangerParents[1];
            Wheel1 = GameStateMachine.Instance.MainPlayer.gameplay.boardController._visualWheel1;
            Wheel2 = GameStateMachine.Instance.MainPlayer.gameplay.boardController._visualWheel2;
            Wheel3 = GameStateMachine.Instance.MainPlayer.gameplay.boardController._visualWheel3;
            Wheel4 = GameStateMachine.Instance.MainPlayer.gameplay.boardController._visualWheel4;

            // Wheel Colliders
            Wheel1Collider = GameStateMachine.Instance.MainPlayer.characterCustomizer.TruckHangerParents[0].parent
                .Find("Wheel1 Collider")
                .GetComponent<SphereCollider>();
            Wheel2Collider = GameStateMachine.Instance.MainPlayer.characterCustomizer.TruckHangerParents[0].parent
                .Find("Wheel2 Collider")
                .GetComponent<SphereCollider>();
            Wheel3Collider = GameStateMachine.Instance.MainPlayer.characterCustomizer.TruckHangerParents[1].parent
                .Find("Wheel3 Collider")
                .GetComponent<SphereCollider>();
            Wheel4Collider = GameStateMachine.Instance.MainPlayer.characterCustomizer.TruckHangerParents[1].parent
                .Find("Wheel4 Collider")
                .GetComponent<SphereCollider>();

            // Defualt Positions
            DefaultWheelPos[0] = Wheel1.localPosition;
            DefaultWheelPos[1] = Wheel2.localPosition;
            DefaultWheelPos[2] = Wheel3.localPosition;
            DefaultWheelPos[3] = Wheel4.localPosition;

            // Truck Colliders
            BackTruckCollider = GameStateMachine.Instance.MainPlayer.characterCustomizer.TruckHangerParents[0].parent.GetComponent<CapsuleCollider>();
            FrontTruckCollider = GameStateMachine.Instance.MainPlayer.characterCustomizer.TruckHangerParents[1].parent.GetComponent<CapsuleCollider>();

            // deck Colliders
            Transform deck = GameStateMachine.Instance.MainPlayer.gameplay.transformReference.boardTransform.Find("Deck");
            Transform colliders = deck.Find("Colliders");
            Deck_Collider1 = colliders.Find("Cube (1)") // tail
                .GetComponent<BoxCollider>();
            Deck_Collider2 = colliders.Find("Cube (2)") // nose
                .GetComponent<BoxCollider>();
            Deck_Collider3 = colliders.Find("Cube (5)") // deck
                .GetComponent<BoxCollider>();
        }

        private void GetReplayDeck()
        {
            Replay_DeckParent = ReplayEditorController.Instance.playbackController.characterCustomizer.DeckParent;
            Replay_BackTruckHanger = ReplayEditorController.Instance.playbackController.characterCustomizer.TruckHangerParents[0];
            Replay_FrontTruckHanger = ReplayEditorController.Instance.playbackController.characterCustomizer.TruckHangerParents[1];
            Replay_Wheel1 = ReplayEditorController.Instance.playbackController.transformReference.wheels[0]; // Back Left
            Replay_Wheel2 = ReplayEditorController.Instance.playbackController.transformReference.wheels[1]; // Back Right
            Replay_Wheel3 = ReplayEditorController.Instance.playbackController.transformReference.wheels[2]; // Front Left
            Replay_Wheel4 = ReplayEditorController.Instance.playbackController.transformReference.wheels[3]; // Front Right

            Default_Replay_WheelPos[0] = Replay_Wheel1.localPosition;
            Default_Replay_WheelPos[1] = Replay_Wheel2.localPosition;
            Default_Replay_WheelPos[2] = Replay_Wheel3.localPosition;
            Default_Replay_WheelPos[3] = Replay_Wheel4.localPosition;
        }

        /*
        private void GetMultiDeck()
        {
            foreach (KeyValuePair<int, NetworkPlayerController> player in MultiplayerManager.Instance.networkPlayers)
            {
                if (player.Value)
                {
                    if (player.Value.IsLocal)
                    {
                        
                        //Multi_DeckParent = player.Value.transformSyncer.livePlayback.customizer.DeckParent;
                        //Multi_BackTruckHanger = player.Value.transformSyncer.livePlayback.customizer.TruckHangerParents[0].parent;
                        //Multi_FrontTruckHanger = player.Value.transformSyncer.livePlayback.customizer.TruckHangerParents[1].parent;
                        //Multi_Wheel1 = player.Value.transformSyncer.livePlayback.customizer.WheelParents[0];
                        //Multi_Wheel2 = player.Value.transformSyncer.livePlayback.customizer.WheelParents[1];
                        //Multi_Wheel3 = player.Value.transformSyncer.livePlayback.customizer.WheelParents[2];
                        //Multi_Wheel4 = player.Value.transformSyncer.livePlayback.customizer.WheelParents[3];
                        
                        //Multi_DeckParent = player.Value.transformSyncer.transformReference.boardTransform;
                        
                        Multi_DeckParent = player.Value.GetSkateboard().FindChildRecursively("Deck Mesh Parent");
                        Multi_BackTruckHanger = player.Value.GetSkateboard().FindChildRecursively("Back Truck").Find("Hanger");
                        Multi_FrontTruckHanger = player.Value.GetSkateboard().FindChildRecursively("Front Truck").Find("Hanger");
                        Multi_Wheel1 = player.Value.GetSkateboard().FindChildRecursively("Wheel1");
                        Multi_Wheel2 = player.Value.GetSkateboard().FindChildRecursively("Wheel2");
                        Multi_Wheel3 = player.Value.GetSkateboard().FindChildRecursively("Wheel3");
                        Multi_Wheel4 = player.Value.GetSkateboard().FindChildRecursively("Wheel4");                 
                        
                    }
                }
            }
        }
        */

        /*
        private bool MultiDeckFound()
        {
            if (Multi_DeckParent != null && Multi_BackTruckHanger && Multi_FrontTruckHanger != null && Multi_Wheel1 != null && Multi_Wheel2 != null && Multi_Wheel3 != null && Multi_Wheel4 != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        */

        /*
        private void ResetMultiDeck()
        {
            Multi_DeckParent = null;
            Multi_BackTruckHanger = null;
            Multi_FrontTruckHanger = null;
            Multi_Wheel1 = null;
            Multi_Wheel2 = null;
            Multi_Wheel3 = null;
            Multi_Wheel4 = null;
        }
        */
        // ------------------------ Get Components End ------------------------

        // ------------------------ Update Settings ------------------------
        private void UpdateSettings()
        {
            // Scales Deck mesh
            if (DeckParent != null)
            {
                Vector3 deckLocalScale = DeckParent.localScale;
                Vector3 settingsDeckLocalScale = new Vector3(Main.settings.DeckLocalScale_x, Main.settings.DeckLocalScale_y, Main.settings.DeckLocalScale_z);
                if (deckLocalScale != settingsDeckLocalScale)
                {
                    DeckParent.localScale = settingsDeckLocalScale;
                }
            }

            // Scales Trucks on the X axis
            if (BackTruckHanger != null)
            {
                Vector3 backTruckLocalScale = BackTruckHanger.localScale;
                Vector3 settingsBackTruckLocalScale = new Vector3(Main.settings.BackTruckLocalScale_x, backTruckLocalScale.y, backTruckLocalScale.z);
                if (backTruckLocalScale != settingsBackTruckLocalScale)
                {
                    BackTruckHanger.localScale = settingsBackTruckLocalScale;
                }
            }
            if (FrontTruckHanger != null)
            {
                Vector3 frontTruckLocalScale = FrontTruckHanger.localScale;
                Vector3 settingsFrontTruckLocalScale = new Vector3(Main.settings.FrontTruckLocalScale_x, frontTruckLocalScale.y, frontTruckLocalScale.z);
                if (frontTruckLocalScale != settingsFrontTruckLocalScale)
                {
                    FrontTruckHanger.localScale = settingsFrontTruckLocalScale;
                }
            }

            // scales Wheels
            if (Wheel1 != null)
            {
                Vector3 wheel1LocalScale = Wheel1.localScale;
                Vector3 settingsWheel1LocalScale = new Vector3(Main.settings.Wheel1LocalScale_x, wheel1LocalScale.y, wheel1LocalScale.z);
                if (wheel1LocalScale != settingsWheel1LocalScale)
                {
                    Wheel1.localScale = settingsWheel1LocalScale;
                }

                float wheel1Radius = Main.settings.Wheel1Radius;
                if (wheel1LocalScale.y != wheel1Radius || wheel1LocalScale.z != wheel1Radius)
                {
                    Wheel1.localScale = new Vector3(wheel1LocalScale.x, wheel1Radius, wheel1Radius);
                }
            }
            if (Wheel2 != null)
            {
                Vector3 wheel2LocalScale = Wheel2.localScale;
                Vector3 settingsWheel2LocalScale = new Vector3(Main.settings.Wheel2LocalScale_x, wheel2LocalScale.y, wheel2LocalScale.z);
                if (wheel2LocalScale != settingsWheel2LocalScale)
                {
                    Wheel2.localScale = settingsWheel2LocalScale;
                }

                float wheel2Radius = Main.settings.Wheel2Radius;
                if (wheel2LocalScale.y != wheel2Radius || wheel2LocalScale.z != wheel2Radius)
                {
                    Wheel2.localScale = new Vector3(wheel2LocalScale.x, wheel2Radius, wheel2Radius);
                }
            }
            if (Wheel3 != null)
            {
                Vector3 wheel3LocalScale = Wheel3.localScale;
                Vector3 settingsWheel3LocalScale = new Vector3(Main.settings.Wheel3LocalScale_x, wheel3LocalScale.y, wheel3LocalScale.z);
                if (wheel3LocalScale != settingsWheel3LocalScale)
                {
                    Wheel3.localScale = settingsWheel3LocalScale;
                }
                float wheel3Radius = Main.settings.Wheel3Radius;
                if (wheel3LocalScale.y != wheel3Radius || wheel3LocalScale.z != wheel3Radius)
                {
                    Wheel3.localScale = new Vector3(wheel3LocalScale.x, wheel3Radius, wheel3Radius);
                }
            }
            if (Wheel4 != null)
            {
                Vector3 wheel4LocalScale = Wheel4.localScale;
                Vector3 settingsWheel4LocalScale = new Vector3(Main.settings.Wheel4LocalScale_x, wheel4LocalScale.y, wheel4LocalScale.z);
                if (wheel4LocalScale != settingsWheel4LocalScale)
                {
                    Wheel4.localScale = settingsWheel4LocalScale;
                }

                float wheel4Radius = Main.settings.Wheel4Radius;
                if (wheel4LocalScale.y != wheel4Radius || wheel4LocalScale.z != wheel4Radius)
                {
                    Wheel4.localScale = new Vector3(wheel4LocalScale.x, wheel4Radius, wheel4Radius);
                }
            }
        }
        public void ScaleAllWheels()
        {
            switch (Main.settings.WheelScaleTarget)
            {
                case "Wheel 1":
                    var blScale = Main.settings.Wheel1LocalScale_x;
                    var blRadius = Main.settings.Wheel1Radius;
                    Main.settings.Wheel2LocalScale_x = blScale;
                    Main.settings.Wheel2Radius = blRadius;
                    Main.settings.Wheel3LocalScale_x = blScale;
                    Main.settings.Wheel3Radius = blRadius;
                    Main.settings.Wheel4LocalScale_x = blScale;
                    Main.settings.Wheel4Radius = blRadius;
                    break;
                case "Wheel 2":
                    var brScale = Main.settings.Wheel2LocalScale_x;
                    var brRadius = Main.settings.Wheel2Radius;
                    Main.settings.Wheel1LocalScale_x = brScale;
                    Main.settings.Wheel1Radius = brRadius;
                    Main.settings.Wheel3LocalScale_x = brScale;
                    Main.settings.Wheel3Radius = brRadius;
                    Main.settings.Wheel4LocalScale_x = brScale;
                    Main.settings.Wheel4Radius = brRadius;
                    break;
                case "Wheel 3":
                    var flScale = Main.settings.Wheel3LocalScale_x;
                    var flRadius = Main.settings.Wheel3Radius;
                    Main.settings.Wheel1LocalScale_x = flScale;
                    Main.settings.Wheel1Radius = flRadius;
                    Main.settings.Wheel2LocalScale_x = flScale;
                    Main.settings.Wheel2Radius = flRadius;
                    Main.settings.Wheel4LocalScale_x = flScale;
                    Main.settings.Wheel4Radius = flRadius;
                    break;
                case "Wheel 4":
                    var frScale = Main.settings.Wheel4LocalScale_x;
                    var frRadius = Main.settings.Wheel4Radius;
                    Main.settings.Wheel1LocalScale_x = frScale;
                    Main.settings.Wheel1Radius = frRadius;
                    Main.settings.Wheel2LocalScale_x = frScale;
                    Main.settings.Wheel2Radius = frRadius;
                    Main.settings.Wheel3LocalScale_x = frScale;
                    Main.settings.Wheel3Radius = frRadius;
                    break;
            }
        }
        // ------------------------ Update Settings End ------------------------

        // ------------------------ Scale Colliders ------------------------
        void UpdateDeckColliders()
        {
            if (Deck_Collider1 != null)
            {
                float X = Main.settings.DefaultDeckCollider1Size.x * Main.settings.DeckLocalScale_x;
                float Y = (Main.settings.DefaultDeckCollider1Size.y * (2 - Main.settings.DeckLocalScale_y) + 6 * (Main.settings.DeckLocalScale_y - 1)) / (2 - 1);
                float Z = Deck_Collider1.size.z;
                Vector3 newSize = new Vector3(X, Y, Z);
                if (Deck_Collider1.size != newSize)
                {
                    Deck_Collider1.size = newSize;
                }
            }

            if (Deck_Collider2 != null)
            {
                float X = Main.settings.DefaultDeckCollider2Size.x * Main.settings.DeckLocalScale_x;
                float Y = (Main.settings.DefaultDeckCollider2Size.y * (2 - Main.settings.DeckLocalScale_y) + 6 * (Main.settings.DeckLocalScale_y - 1)) / (2 - 1);
                float Z = Deck_Collider2.size.z;
                Vector3 newSize = new Vector3(X, Y, Z);
                if (Deck_Collider2.size != newSize)
                {
                    Deck_Collider2.size = newSize;
                }
            }

            if (Deck_Collider3 != null)
            {
                float X = Main.settings.DefaultDeckCollider3Size.x * Main.settings.DeckLocalScale_x;
                float Y = Main.settings.DefaultDeckCollider3Size.y * Main.settings.DeckLocalScale_y;
                float Z = Deck_Collider3.size.z;
                Vector3 newSize = new Vector3(X, Y, Z);
                if (Deck_Collider3.size != newSize)
                {
                    Deck_Collider3.size = newSize;
                }
            }
        }

        private float oldBTColliderHeight, oldFTColliderHeight;
        private void UpdateTruckCollider()
        {
            if (BackTruckCollider != null)
            {
                var backTruckLocalScaleX = Main.settings.BackTruckLocalScale_x;
                if (oldBTColliderHeight != backTruckLocalScaleX)
                {
                    BackTruckCollider.height = DefaultTruckColliderHeight * backTruckLocalScaleX;
                    oldBTColliderHeight = backTruckLocalScaleX;
                }
            }

            if (FrontTruckCollider != null)
            {
                var frontTruckLocalScaleX = Main.settings.FrontTruckLocalScale_x;
                if (oldFTColliderHeight != frontTruckLocalScaleX)
                {
                    FrontTruckCollider.height = DefaultTruckColliderHeight * frontTruckLocalScaleX;
                    oldFTColliderHeight = frontTruckLocalScaleX;
                }
            }
        }

        private float oldW1Radius, oldW2Radius, oldW3Radius, oldW4Radius;
        private void UpdateWheelCollider()
        {
            if (Wheel1Collider != null)
            {
                if (oldW1Radius != Main.settings.Wheel1Radius)
                {
                    Wheel1Collider.radius = Main.settings.Wheel1Radius / 100 * 2.5f;
                    oldW1Radius = Main.settings.Wheel1Radius;
                }
            }
            if (Wheel2Collider != null)
            {
                if (oldW2Radius != Main.settings.Wheel2Radius)
                {
                    Wheel2Collider.radius = Main.settings.Wheel2Radius / 100 * 2.5f;
                    oldW2Radius = Main.settings.Wheel2Radius;
                }
            }
            if (Wheel3Collider != null)
            {
                if (oldW3Radius != Main.settings.Wheel3Radius)
                {
                    Wheel3Collider.radius = Main.settings.Wheel3Radius / 100 * 2.5f;
                    oldW3Radius = Main.settings.Wheel3Radius;
                }
            }
            if (Wheel4Collider != null)
            {
                if (oldW4Radius != Main.settings.Wheel4Radius)
                {
                    Wheel4Collider.radius = Main.settings.Wheel4Radius / 100 * 2.5f;
                    oldW4Radius = Main.settings.Wheel4Radius;
                }
            }
        }

        // ------------------------ Scale Colliders End ------------------------

        // ------------------------ Update Positions ------------------------
        private void UpdateWheelPos()
        {
            const float num = 12f;
            var newWheel1Pos = new Vector3(Main.settings.BackTruckLocalScale_x / -num, DefaultWheelPos[0].y, DefaultWheelPos[0].z);
            var newWheel2Pos = new Vector3(Main.settings.BackTruckLocalScale_x / num, DefaultWheelPos[1].y, DefaultWheelPos[1].z);
            var newWheel3Pos = new Vector3(Main.settings.FrontTruckLocalScale_x / -num, DefaultWheelPos[2].y, DefaultWheelPos[2].z);
            var newWheel4Pos = new Vector3(Main.settings.FrontTruckLocalScale_x / num, DefaultWheelPos[3].y, DefaultWheelPos[3].z);

            if (Wheel1.localPosition != newWheel1Pos)
            {
                Wheel1.localPosition = newWheel1Pos;
            }
            if (Wheel2.localPosition != newWheel2Pos)
            {
                Wheel2.localPosition = newWheel2Pos;
            }
            if (Wheel3.localPosition != newWheel3Pos)
            {
                Wheel3.localPosition = newWheel3Pos;
            }
            if (Wheel4.localPosition != newWheel4Pos)
            {
                Wheel4.localPosition = newWheel4Pos;
            }

            // Colliders position
            if (Wheel1Collider.transform.localPosition != newWheel1Pos)
            {
                Wheel1Collider.transform.localPosition = newWheel1Pos;
            }
            if (Wheel2Collider.transform.localPosition != newWheel2Pos)
            {
                Wheel2Collider.transform.localPosition = newWheel2Pos;
            }
            if (Wheel3Collider.transform.localPosition != newWheel3Pos)
            {
                Wheel3Collider.transform.localPosition = newWheel3Pos;
            }
            if (Wheel4Collider.transform.localPosition != newWheel4Pos)
            {
                Wheel4Collider.transform.localPosition = newWheel4Pos;
            }
        }

        // ------------------------ Update Positions End ------------------------

        // ------------------------ Replay Settings ------------------------
        private void UpdateReplaySettings()
        {
            // Scales Replay Deck mesh
            if (Replay_DeckParent != null)
            {
                Vector3 Replay_deckLocalScale = Replay_DeckParent.localScale;
                Vector3 settingsDeckLocalScale = new Vector3(Main.settings.DeckLocalScale_x, Main.settings.DeckLocalScale_y, Main.settings.DeckLocalScale_z);
                if (Replay_deckLocalScale != settingsDeckLocalScale)
                {
                    Replay_DeckParent.localScale = settingsDeckLocalScale;
                }
            }

            // Scales  Replay Trucks on the X axis
            if (Replay_BackTruckHanger != null)
            {
                Vector3 Replay_backTruckLocalScale = Replay_BackTruckHanger.localScale;
                Vector3 settingsBackTruckLocalScale = new Vector3(Main.settings.BackTruckLocalScale_x, Replay_backTruckLocalScale.y, Replay_backTruckLocalScale.z);
                if (Replay_backTruckLocalScale.x != Main.settings.BackTruckLocalScale_x)
                {
                    Replay_BackTruckHanger.localScale = settingsBackTruckLocalScale;
                }
            }
            if (Replay_FrontTruckHanger != null)
            {
                Vector3 Replay_frontTruckLocalScale = Replay_FrontTruckHanger.localScale;
                Vector3 settingsFrontTruckLocalScale = new Vector3(Main.settings.FrontTruckLocalScale_x, Replay_frontTruckLocalScale.y, Replay_frontTruckLocalScale.z);
                if (Replay_frontTruckLocalScale.x != Main.settings.FrontTruckLocalScale_x)
                {
                    Replay_FrontTruckHanger.localScale = settingsFrontTruckLocalScale;
                }
            }

            // scales Replay Wheels
            if (Replay_Wheel1 != null)
            {
                Vector3 Replay_wheel1LocalScale = Replay_Wheel1.localScale;
                Vector3 settingsWheel1LocalScale = new Vector3(Main.settings.Wheel1LocalScale_x, Replay_wheel1LocalScale.y, Replay_wheel1LocalScale.z);
                if (Replay_wheel1LocalScale != settingsWheel1LocalScale)
                {
                    Replay_Wheel1.localScale = settingsWheel1LocalScale;
                }

                float wheel1Radius = Main.settings.Wheel1Radius;
                if (Replay_wheel1LocalScale.y != wheel1Radius || Replay_wheel1LocalScale.z != wheel1Radius)
                {
                    Replay_Wheel1.localScale = new Vector3(Replay_wheel1LocalScale.x, wheel1Radius, wheel1Radius);
                }
            }
            if (Replay_Wheel2 != null)
            {
                Vector3 Replay_wheel2LocalScale = Replay_Wheel2.localScale;
                Vector3 settingsWheel2LocalScale = new Vector3(Main.settings.Wheel2LocalScale_x, Replay_wheel2LocalScale.y, Replay_wheel2LocalScale.z);
                if (Replay_wheel2LocalScale != settingsWheel2LocalScale)
                {
                    Replay_Wheel2.localScale = settingsWheel2LocalScale;
                }

                float wheel2Radius = Main.settings.Wheel2Radius;
                if (Replay_wheel2LocalScale.y != wheel2Radius || Replay_wheel2LocalScale.z != wheel2Radius)
                {
                    Replay_Wheel2.localScale = new Vector3(Replay_wheel2LocalScale.x, wheel2Radius, wheel2Radius);
                }
            }
            if (Replay_Wheel3 != null)
            {
                Vector3 Replay_wheel3LocalScale = Replay_Wheel3.localScale;
                Vector3 settingsWheel3LocalScale = new Vector3(Main.settings.Wheel3LocalScale_x, Replay_wheel3LocalScale.y, Replay_wheel3LocalScale.z);
                if (Replay_wheel3LocalScale != settingsWheel3LocalScale)
                {
                    Replay_Wheel3.localScale = settingsWheel3LocalScale;
                }
                float wheel3Radius = Main.settings.Wheel3Radius;
                if (Replay_wheel3LocalScale.y != wheel3Radius || Replay_wheel3LocalScale.z != wheel3Radius)
                {
                    Replay_Wheel3.localScale = new Vector3(Replay_wheel3LocalScale.x, wheel3Radius, wheel3Radius);
                }
            }
            if (Replay_Wheel4 != null)
            {
                Vector3 Replay_wheel4LocalScale = Replay_Wheel4.localScale;
                Vector3 settingsWheel4LocalScale = new Vector3(Main.settings.Wheel4LocalScale_x, Replay_wheel4LocalScale.y, Replay_wheel4LocalScale.z);
                if (Replay_wheel4LocalScale != settingsWheel4LocalScale)
                {
                    Replay_Wheel4.localScale = settingsWheel4LocalScale;
                }

                float wheel4Radius = Main.settings.Wheel4Radius;
                if (Replay_wheel4LocalScale.y != wheel4Radius || Replay_wheel4LocalScale.z != wheel4Radius)
                {
                    Replay_Wheel4.localScale = new Vector3(Replay_wheel4LocalScale.x, wheel4Radius, wheel4Radius);
                }
            }
        }

        private void UpdateReplayWheelPos()
        {
            const float num = 12f;
            var newWheel1Pos = new Vector3(Main.settings.BackTruckLocalScale_x / -num, DefaultWheelPos[0].y, DefaultWheelPos[0].z);
            var newWheel2Pos = new Vector3(Main.settings.BackTruckLocalScale_x / num, DefaultWheelPos[1].y, DefaultWheelPos[1].z);
            var newWheel3Pos = new Vector3(Main.settings.FrontTruckLocalScale_x / -num, DefaultWheelPos[2].y, DefaultWheelPos[2].z);
            var newWheel4Pos = new Vector3(Main.settings.FrontTruckLocalScale_x / num, DefaultWheelPos[3].y, DefaultWheelPos[3].z);

            if (Replay_Wheel1.localPosition != newWheel1Pos)
            {
                Replay_Wheel1.localPosition = newWheel1Pos;
            }
            if (Replay_Wheel2.localPosition != newWheel2Pos)
            {
                Replay_Wheel2.localPosition = newWheel2Pos;
            }
            if (Replay_Wheel3.localPosition != newWheel3Pos)
            {
                Replay_Wheel3.localPosition = newWheel3Pos;
            }
            if (Replay_Wheel4.localPosition != newWheel4Pos)
            {
                Replay_Wheel4.localPosition = newWheel4Pos;
            }
        }
        // ------------------------ Replay Settings End ------------------------

        /*
        private void UpdateMultiSettings()
        {

            // Scales Replay Deck mesh
            if (Multi_DeckParent != null)
            {
                Vector3 Multi_deckLocalScale = Multi_DeckParent.localScale;
                Vector3 settingsDeckLocalScale = new Vector3(Main.settings.DeckLocalScale_x, Main.settings.DeckLocalScale_y, Main.settings.DeckLocalScale_z);
                if (Multi_deckLocalScale != settingsDeckLocalScale)
                {
                    Multi_DeckParent.localScale = settingsDeckLocalScale;
                }
            }

            // Scales  Replay Trucks on the X axis
            if (Multi_BackTruckHanger != null)
            {
                Vector3 Multi_backTruckLocalScale = Multi_BackTruckHanger.localScale;
                Vector3 settingsBackTruckLocalScale = new Vector3(Main.settings.BackTruckLocalScale_x, Multi_backTruckLocalScale.y, Multi_backTruckLocalScale.z);
                if (Multi_backTruckLocalScale.x != Main.settings.BackTruckLocalScale_x)
                {
                    Multi_BackTruckHanger.localScale = settingsBackTruckLocalScale;
                }
            }
            if (Multi_FrontTruckHanger != null)
            {
                Vector3 Multi_frontTruckLocalScale = Multi_FrontTruckHanger.localScale;
                Vector3 settingsFrontTruckLocalScale = new Vector3(Main.settings.FrontTruckLocalScale_x, Multi_frontTruckLocalScale.y, Multi_frontTruckLocalScale.z);
                if (Multi_frontTruckLocalScale.x != Main.settings.FrontTruckLocalScale_x)
                {
                    Multi_FrontTruckHanger.localScale = settingsFrontTruckLocalScale;
                }
            }

            // scales Replay Wheels
            if (Multi_Wheel1 != null)
            {
                Vector3 Multi_wheel1LocalScale = Multi_Wheel1.localScale;
                Vector3 settingsWheel1LocalScale = new Vector3(Main.settings.Wheel1LocalScale_x, Multi_wheel1LocalScale.y, Multi_wheel1LocalScale.z);
                if (Multi_wheel1LocalScale != settingsWheel1LocalScale)
                {
                    Multi_Wheel1.localScale = settingsWheel1LocalScale;
                }

                float wheel1Radius = Main.settings.Wheel1Radius;
                if (Multi_wheel1LocalScale.y != wheel1Radius || Multi_wheel1LocalScale.z != wheel1Radius)
                {
                    Multi_Wheel1.localScale = new Vector3(Multi_wheel1LocalScale.x, wheel1Radius, wheel1Radius);
                }
            }
            if (Multi_Wheel2 != null)
            {
                Vector3 Multi_wheel2LocalScale = Multi_Wheel2.localScale;
                Vector3 settingsWheel2LocalScale = new Vector3(Main.settings.Wheel2LocalScale_x, Multi_wheel2LocalScale.y, Multi_wheel2LocalScale.z);
                if (Multi_wheel2LocalScale != settingsWheel2LocalScale)
                {
                    Multi_Wheel2.localScale = settingsWheel2LocalScale;
                }

                float wheel2Radius = Main.settings.Wheel2Radius;
                if (Multi_wheel2LocalScale.y != wheel2Radius || Multi_wheel2LocalScale.z != wheel2Radius)
                {
                    Multi_Wheel2.localScale = new Vector3(Multi_wheel2LocalScale.x, wheel2Radius, wheel2Radius);
                }
            }
            if (Multi_Wheel3 != null)
            {
                Vector3 Multi_wheel3LocalScale = Multi_Wheel3.localScale;
                Vector3 settingsWheel3LocalScale = new Vector3(Main.settings.Wheel3LocalScale_x, Multi_wheel3LocalScale.y, Multi_wheel3LocalScale.z);
                if (Multi_wheel3LocalScale != settingsWheel3LocalScale)
                {
                    Multi_Wheel3.localScale = settingsWheel3LocalScale;
                }
                float wheel3Radius = Main.settings.Wheel3Radius;
                if (Multi_wheel3LocalScale.y != wheel3Radius || Multi_wheel3LocalScale.z != wheel3Radius)
                {
                    Multi_Wheel3.localScale = new Vector3(Multi_wheel3LocalScale.x, wheel3Radius, wheel3Radius);
                }
            }
            if (Multi_Wheel4 != null)
            {
                Vector3 Multi_wheel4LocalScale = Multi_Wheel4.localScale;
                Vector3 settingsWheel4LocalScale = new Vector3(Main.settings.Wheel4LocalScale_x, Multi_wheel4LocalScale.y, Multi_wheel4LocalScale.z);
                if (Multi_wheel4LocalScale != settingsWheel4LocalScale)
                {
                    Multi_Wheel4.localScale = settingsWheel4LocalScale;
                }

                float wheel4Radius = Main.settings.Wheel4Radius;
                if (Multi_wheel4LocalScale.y != wheel4Radius || Multi_wheel4LocalScale.z != wheel4Radius)
                {
                    Multi_Wheel4.localScale = new Vector3(Multi_wheel4LocalScale.x, wheel4Radius, wheel4Radius);
                }
            }
        }
        */
    }
}

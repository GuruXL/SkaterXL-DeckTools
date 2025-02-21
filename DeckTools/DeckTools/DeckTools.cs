using UnityEngine;
using ReplayEditor;
using GameManagement;

namespace DeckTools
{
    public class DeckTools : MonoBehaviour
    {
        Transform DeckParent;

        public BoxCollider Deck_Collider1;
        public BoxCollider Deck_Collider2;
        public BoxCollider Deck_Collider3;

        public Vector3 DefaultDeckCollider1Size = new Vector3(1.2f, 1.5f, 1f);
        public Vector3 DefaultDeckCollider2Size = new Vector3(1.2f, 1.5f, 1f);
        public Vector3 DefaultDeckCollider3Size = new Vector3(1f, 1f, 1f);

        Transform BackTruckHanger;
        Transform FrontTruckHanger;
        Transform Wheel1;
        Transform Wheel2;
        Transform Wheel3;
        Transform Wheel4;

        //CapsuleCollider BackTruckCollider;
        //CapsuleCollider FrontTruckCollider;

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
            else
            {
                switch (GameStateMachine.Instance.CurrentState)
                {
                    case PlayState playState:
                        UpdateSettings();
                        UpdateWheelCollider();
                        UpdateWheelPos();

                        break;
                    case ReplayState replayState:
                        UpdateReplaySettings();
                        UpdateReplayWheelPos();
                        break;
                }
            }
        }
        private void FixedUpdate()
        {
        }

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
                float wheel1Radius = Main.settings.Wheel1Radius;

                if (wheel1LocalScale != settingsWheel1LocalScale)
                {
                    Wheel1.localScale = settingsWheel1LocalScale;
                }
                else if (wheel1LocalScale.y != wheel1Radius || wheel1LocalScale.z != wheel1Radius)
                {
                    Wheel1.localScale = new Vector3(wheel1LocalScale.x, wheel1Radius, wheel1Radius);
                }
            }
            if (Wheel2 != null)
            {
                Vector3 wheel2LocalScale = Wheel2.localScale;
                Vector3 settingsWheel2LocalScale = new Vector3(Main.settings.Wheel2LocalScale_x, wheel2LocalScale.y, wheel2LocalScale.z);
                float wheel2Radius = Main.settings.Wheel2Radius;

                if (wheel2LocalScale != settingsWheel2LocalScale)
                {
                    Wheel2.localScale = settingsWheel2LocalScale;
                }
                else if (wheel2LocalScale.y != wheel2Radius || wheel2LocalScale.z != wheel2Radius)
                {
                    Wheel2.localScale = new Vector3(wheel2LocalScale.x, wheel2Radius, wheel2Radius);
                }
            }
            if (Wheel3 != null)
            {
                Vector3 wheel3LocalScale = Wheel3.localScale;
                Vector3 settingsWheel3LocalScale = new Vector3(Main.settings.Wheel3LocalScale_x, wheel3LocalScale.y, wheel3LocalScale.z);
                float wheel3Radius = Main.settings.Wheel3Radius;

                if (wheel3LocalScale != settingsWheel3LocalScale)
                {
                    Wheel3.localScale = settingsWheel3LocalScale;
                }
                else if (wheel3LocalScale.y != wheel3Radius || wheel3LocalScale.z != wheel3Radius)
                {
                    Wheel3.localScale = new Vector3(wheel3LocalScale.x, wheel3Radius, wheel3Radius);
                }
            }
            if (Wheel4 != null)
            {
                Vector3 wheel4LocalScale = Wheel4.localScale;
                Vector3 settingsWheel4LocalScale = new Vector3(Main.settings.Wheel4LocalScale_x, wheel4LocalScale.y, wheel4LocalScale.z);
                float wheel4Radius = Main.settings.Wheel4Radius;

                if (wheel4LocalScale != settingsWheel4LocalScale)
                {
                    Wheel4.localScale = settingsWheel4LocalScale;
                }
                else if (wheel4LocalScale.y != wheel4Radius || wheel4LocalScale.z != wheel4Radius)
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

        public void ResetSettings()
        {
            Main.settings.DeckLocalScale_x = 1f;
            Main.settings.DeckLocalScale_y = 1f;
            Main.settings.DeckLocalScale_z = 1f;
            Main.settings.BackTruckLocalScale_x = 1f;
            Main.settings.FrontTruckLocalScale_x = 1f;
            Main.settings.Wheel1LocalScale_x = 1f;
            Main.settings.Wheel1Radius = 1f;
            Main.settings.Wheel2LocalScale_x = 1f;
            Main.settings.Wheel2Radius = 1f;
            Main.settings.Wheel3LocalScale_x = 1f;
            Main.settings.Wheel3Radius = 1f;
            Main.settings.Wheel4LocalScale_x = 1f;
            Main.settings.Wheel4Radius = 1f;
            Main.settings.truckTightness = 1f;
        }
        // ------------------------ Update Settings End ------------------------

        // ------------------------ Scale Colliders ------------------------
        void UpdateDeckColliders()
        {
            if (Deck_Collider1 != null)
            {
                float X = DefaultDeckCollider1Size.x * Main.settings.DeckLocalScale_x;
                float Y = (DefaultDeckCollider1Size.y * (2 - Main.settings.DeckLocalScale_y) + 6 * (Main.settings.DeckLocalScale_y - 1)) / (2 - 1);
                float Z = Deck_Collider1.size.z;
                Vector3 newSize = new Vector3(X, Y, Z);
                if (Deck_Collider1.size != newSize)
                {
                    Deck_Collider1.size = newSize;
                }
            }

            if (Deck_Collider2 != null)
            {
                float X = DefaultDeckCollider2Size.x * Main.settings.DeckLocalScale_x;
                float Y = (DefaultDeckCollider2Size.y * (2 - Main.settings.DeckLocalScale_y) + 6 * (Main.settings.DeckLocalScale_y - 1)) / (2 - 1);
                float Z = Deck_Collider2.size.z;
                Vector3 newSize = new Vector3(X, Y, Z);
                if (Deck_Collider2.size != newSize)
                {
                    Deck_Collider2.size = newSize;
                }
            }

            if (Deck_Collider3 != null)
            {
                float X = DefaultDeckCollider3Size.x * Main.settings.DeckLocalScale_x;
                float Y = DefaultDeckCollider3Size.y * Main.settings.DeckLocalScale_y;
                float Z = Deck_Collider3.size.z;
                Vector3 newSize = new Vector3(X, Y, Z);
                if (Deck_Collider3.size != newSize)
                {
                    Deck_Collider3.size = newSize;
                }
            }
        }
        private void UpdateTruckCollider()
        {
            if (BackTruckCollider != null)
            {
                float BTColliderHeight = BackTruckCollider.height;
                float backTruckLocalScaleX = Main.settings.BackTruckLocalScale_x;
                if (BTColliderHeight != backTruckLocalScaleX)
                {
                    BackTruckCollider.height = DefaultTruckColliderHeight * backTruckLocalScaleX;
                }
            }

            if (FrontTruckCollider != null)
            {
                float FTColliderHeight = FrontTruckCollider.height;
                float frontTruckLocalScaleX = Main.settings.FrontTruckLocalScale_x;
                if (FTColliderHeight != frontTruckLocalScaleX)
                {
                    FrontTruckCollider.height = DefaultTruckColliderHeight * frontTruckLocalScaleX;
                }
            }
        }
        private void UpdateWheelCollider()
        {
            if (Wheel1Collider != null)
            {
                float wheel1Radius = Wheel1Collider.radius;
                if (wheel1Radius != Main.settings.Wheel1Radius)
                {
                    Wheel1Collider.radius = Main.settings.Wheel1Radius / 100 * 2.5f;
                }
            }
            if (Wheel2Collider != null)
            {
                float wheel2Radius = Wheel2Collider.radius;
                if (wheel2Radius != Main.settings.Wheel2Radius)
                {
                    Wheel2Collider.radius = Main.settings.Wheel2Radius / 100 * 2.5f;
                }
            }
            if (Wheel3Collider != null)
            {
                float wheel3Radius = Wheel3Collider.radius;
                if (wheel3Radius != Main.settings.Wheel3Radius)
                {
                    Wheel3Collider.radius = Main.settings.Wheel3Radius / 100 * 2.5f;
                }
            }
            if (Wheel4Collider != null)
            {
                float wheel4Radius = Wheel4Collider.radius;
                if (wheel4Radius != Main.settings.Wheel4Radius)
                {
                    Wheel4Collider.radius = Main.settings.Wheel4Radius / 100 * 2.5f;
                }
            }
        }
        public void ResetColliders(int options)
        {
            switch (options)
            {
                case 1:
                    Deck_Collider1.size = DefaultDeckCollider1Size;
                    Deck_Collider2.size = DefaultDeckCollider1Size;
                    Deck_Collider3.size = DefaultDeckCollider1Size;
                    break;
                case 2:
                    BackTruckCollider.height = DefaultTruckColliderHeight;
                    FrontTruckCollider.height = DefaultTruckColliderHeight;
                    break;
                case 3:
                    Wheel1Collider.radius = DefaultWheelColliderRadius;
                    Wheel2Collider.radius = DefaultWheelColliderRadius;
                    Wheel3Collider.radius = DefaultWheelColliderRadius;
                    Wheel4Collider.radius = DefaultWheelColliderRadius;
                    break;
            }
        }

        // ------------------------ Scale Colliders End ------------------------

        // ------------------------ Update Positions ------------------------
        private void UpdateWheelPos()
        {
            const float num = 12f;

            Vector3[] newWheelPos = new Vector3[4];
            newWheelPos[0] = new Vector3(Main.settings.BackTruckLocalScale_x / -num, DefaultWheelPos[0].y, DefaultWheelPos[0].z);
            newWheelPos[1] = new Vector3(Main.settings.BackTruckLocalScale_x / num, DefaultWheelPos[1].y, DefaultWheelPos[1].z);
            newWheelPos[2] = new Vector3(Main.settings.FrontTruckLocalScale_x / -num, DefaultWheelPos[2].y, DefaultWheelPos[2].z);
            newWheelPos[3] = new Vector3(Main.settings.FrontTruckLocalScale_x / num, DefaultWheelPos[3].y, DefaultWheelPos[3].z);

            Transform[] wheels = { Wheel1, Wheel2, Wheel3, Wheel4 };
            Transform[] colliders = { Wheel1Collider.transform, Wheel2Collider.transform, Wheel3Collider.transform, Wheel4Collider.transform };

            for (int i = 0; i < 4; i++)
            {
                if (wheels[i].localPosition != newWheelPos[i])
                {
                    wheels[i].localPosition = newWheelPos[i];
                }
                else if (colliders[i].localPosition != newWheelPos[i])
                {
                    colliders[i].localPosition = newWheelPos[i];
                }
            }
        }
        private void UpdateReplayWheelPos()
        {
            const float num = 12f;

            Vector3[] newWheelPos = new Vector3[4];
            newWheelPos[0] = new Vector3(Main.settings.BackTruckLocalScale_x / -num, DefaultWheelPos[0].y, DefaultWheelPos[0].z);
            newWheelPos[1] = new Vector3(Main.settings.BackTruckLocalScale_x / num, DefaultWheelPos[1].y, DefaultWheelPos[1].z);
            newWheelPos[2] = new Vector3(Main.settings.FrontTruckLocalScale_x / -num, DefaultWheelPos[2].y, DefaultWheelPos[2].z);
            newWheelPos[3] = new Vector3(Main.settings.FrontTruckLocalScale_x / num, DefaultWheelPos[3].y, DefaultWheelPos[3].z);

            Transform[] Replay_wheels = { Replay_Wheel1, Replay_Wheel2, Replay_Wheel3, Replay_Wheel4 };

            for (int i = 0; i < 4; i++)
            {
                if (Replay_wheels[i].localPosition != newWheelPos[i])
                {
                    Replay_wheels[i].localPosition = newWheelPos[i];
                }
            }
        }

        private void UpdateMultiWheelPos()
        {
            const float num = 12f;
            Vector3[] newWheelPos = new Vector3[4];
            newWheelPos[0] = new Vector3(Main.settings.BackTruckLocalScale_x / -num, DefaultWheelPos[0].y, DefaultWheelPos[0].z);
            newWheelPos[1] = new Vector3(Main.settings.BackTruckLocalScale_x / num, DefaultWheelPos[1].y, DefaultWheelPos[1].z);
            newWheelPos[2] = new Vector3(Main.settings.FrontTruckLocalScale_x / -num, DefaultWheelPos[2].y, DefaultWheelPos[2].z);
            newWheelPos[3] = new Vector3(Main.settings.FrontTruckLocalScale_x / num, DefaultWheelPos[3].y, DefaultWheelPos[3].z);

            Transform[] Multi_wheels = { Multi_Wheel1, Multi_Wheel2, Multi_Wheel3, Multi_Wheel4 };

            for (int i = 0; i < 4; i++)
            {
                if (Multi_wheels[i].localPosition != newWheelPos[i])
                {
                    Multi_wheels[i].localPosition = newWheelPos[i];
                }
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
                float wheel1Radius = Main.settings.Wheel1Radius;

                if (Replay_wheel1LocalScale != settingsWheel1LocalScale)
                {
                    Replay_Wheel1.localScale = settingsWheel1LocalScale;
                }
                else if (Replay_wheel1LocalScale.y != wheel1Radius || Replay_wheel1LocalScale.z != wheel1Radius)
                {
                    Replay_Wheel1.localScale = new Vector3(Replay_wheel1LocalScale.x, wheel1Radius, wheel1Radius);
                }
            }
            if (Replay_Wheel2 != null)
            {
                Vector3 Replay_wheel2LocalScale = Replay_Wheel2.localScale;
                Vector3 settingsWheel2LocalScale = new Vector3(Main.settings.Wheel2LocalScale_x, Replay_wheel2LocalScale.y, Replay_wheel2LocalScale.z);
                float wheel2Radius = Main.settings.Wheel2Radius;

                if (Replay_wheel2LocalScale != settingsWheel2LocalScale)
                {
                    Replay_Wheel2.localScale = settingsWheel2LocalScale;
                }
                else if (Replay_wheel2LocalScale.y != wheel2Radius || Replay_wheel2LocalScale.z != wheel2Radius)
                {
                    Replay_Wheel2.localScale = new Vector3(Replay_wheel2LocalScale.x, wheel2Radius, wheel2Radius);
                }
            }
            if (Replay_Wheel3 != null)
            {
                Vector3 Replay_wheel3LocalScale = Replay_Wheel3.localScale;
                Vector3 settingsWheel3LocalScale = new Vector3(Main.settings.Wheel3LocalScale_x, Replay_wheel3LocalScale.y, Replay_wheel3LocalScale.z);
                float wheel3Radius = Main.settings.Wheel3Radius;

                if (Replay_wheel3LocalScale != settingsWheel3LocalScale)
                {
                    Replay_Wheel3.localScale = settingsWheel3LocalScale;
                }
                else if (Replay_wheel3LocalScale.y != wheel3Radius || Replay_wheel3LocalScale.z != wheel3Radius)
                {
                    Replay_Wheel3.localScale = new Vector3(Replay_wheel3LocalScale.x, wheel3Radius, wheel3Radius);
                }
            }
            if (Replay_Wheel4 != null)
            {
                Vector3 Replay_wheel4LocalScale = Replay_Wheel4.localScale;
                Vector3 settingsWheel4LocalScale = new Vector3(Main.settings.Wheel4LocalScale_x, Replay_wheel4LocalScale.y, Replay_wheel4LocalScale.z);
                float wheel4Radius = Main.settings.Wheel4Radius;

                if (Replay_wheel4LocalScale != settingsWheel4LocalScale)
                {
                    Replay_Wheel4.localScale = settingsWheel4LocalScale;
                }
                else if (Replay_wheel4LocalScale.y != wheel4Radius || Replay_wheel4LocalScale.z != wheel4Radius)
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

            Replay_Wheel1.localPosition = newWheel1Pos;
            Replay_Wheel2.localPosition = newWheel2Pos;
            Replay_Wheel3.localPosition = newWheel3Pos;
            Replay_Wheel4.localPosition = newWheel4Pos;
        }
    }
}

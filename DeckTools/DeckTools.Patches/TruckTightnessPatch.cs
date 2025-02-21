using HarmonyLib;
using Rewired;
using UnityEngine;
using GameManagement;
using SkaterXL.Gameplay;

namespace DeckTools.Patches
{
    [HarmonyPatch(typeof(GameplayController), "Update")]
    internal static class TruckTightnessPatch
    {
        [HarmonyPriority(200)]
        private static void Postfix()
        {
            if (!Main.enabled || GameStateMachine.Instance.CurrentState.GetType() != typeof(PlayState) ||
                GameStateMachine.Instance.MainPlayer.gameplay.playerData.currentState == PlayerStateEnum.Manual)
                return;
            JointDrive jointDrive = new JointDrive();
            //float defaultSpring = 1f;
            //float defaultDamping = 0.02f;
            float spring = Main.settings.truckTightness;
            float damping = Main.settings.truckTightness / 100 * 2;
            if ((GameStateMachine.Instance.MainPlayer.input.GetAxis("LT") >= 0.05 ? 0 : GameStateMachine.Instance.MainPlayer.input.GetAxis("RT") < 0.05 ? 1 : 0) != 0)
            {
                spring = 1.0f;
                damping = 0.025f;
            }
            jointDrive.positionSpring = spring;
            jointDrive.positionDamper = damping;
            jointDrive.maximumForce = 3.402823E+23f;
            GameStateMachine.Instance.MainPlayer.gameplay.boardController.backTruckJoint.angularXDrive = jointDrive;
            GameStateMachine.Instance.MainPlayer.gameplay.boardController.frontTruckJoint.angularXDrive = jointDrive;
        }
    }
}

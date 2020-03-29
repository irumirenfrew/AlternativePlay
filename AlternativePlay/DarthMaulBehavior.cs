﻿using UnityEngine;

namespace AlternativePlay
{
    public class DarthMaulBehavior : MonoBehaviour
    {
        private PlayerController playerController;
        private InputManager inputManager;

        public static bool Split { get; set; }

        /// <summary>
        /// To be invoked every time when starting the GameCore scene.
        /// </summary>
        public void BeginGameCoreScene(InputManager inputManager)
        {
            this.inputManager = inputManager;
        }

        private void Awake()
        {
            this.playerController = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            if (ConfigOptions.instance.PlayMode != PlayMode.DarthMaul || playerController == null)
            {
                // Do nothing if we aren't playing Darth Maul
                return;
            }

            if (ConfigOptions.instance.UseTriggerToSeparate)
            {
                // Check to see if the trigger has been pressed
                bool leftTriggerPressed = inputManager.GetLeftTriggerClicked();
                bool rightTriggerPressed = inputManager.GetRightTriggerClicked();

                if (leftTriggerPressed || rightTriggerPressed)
                {
                    Split = !Split;
                }

                if (Split) return;  // When you split Darth Maul it's just regular two sabers so do nothing
            }

            float sep = 1.0f * ConfigOptions.instance.SeparationAmount / 100.0f;
            switch (ConfigOptions.instance.DarthMaulControllerCount)
            {
                case ControllerCountEnum.One:
                    var baseSaber = ConfigOptions.instance.UseLeftController ? playerController.leftSaber : playerController.rightSaber;
                    var otherSaber = ConfigOptions.instance.UseLeftController ? playerController.rightSaber : playerController.leftSaber;
                    var rotateSaber = ConfigOptions.instance.ReverseMaulDirection ? baseSaber : otherSaber;

                    otherSaber.transform.localPosition = baseSaber.transform.localPosition;
                    otherSaber.transform.localRotation = baseSaber.transform.localRotation;

                    rotateSaber.transform.Rotate(0.0f, 180.0f, 180.0f);
                    rotateSaber.transform.Translate(0.0f, 0.0f, sep * 2.0f, Space.Self);
                    break;

                case ControllerCountEnum.Two:
                    Vector3 leftHandPos = playerController.leftSaber.transform.position;
                    Vector3 rightHandPos = playerController.rightSaber.transform.position;
                    Vector3 middlePos = (rightHandPos + leftHandPos) * 0.5f;
                    Vector3 forward = (rightHandPos - leftHandPos).normalized;

                    var forwardSaber = ConfigOptions.instance.ReverseMaulDirection ? playerController.leftSaber : playerController.rightSaber;
                    var backwardSaber = ConfigOptions.instance.ReverseMaulDirection ? playerController.rightSaber : playerController.leftSaber;

                    forwardSaber.transform.position = middlePos + forward * sep;
                    backwardSaber.transform.position = middlePos + -forward * sep;
                    forwardSaber.transform.rotation = Quaternion.LookRotation(forward, backwardSaber.transform.up);
                    backwardSaber.transform.rotation = Quaternion.LookRotation(-forward, -backwardSaber.transform.up);
                    break;

                case ControllerCountEnum.None:
                default:
                    // Do nothing
                    break;
            }

        }
    }
}

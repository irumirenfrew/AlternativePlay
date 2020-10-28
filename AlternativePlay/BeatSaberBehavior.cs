﻿using AlternativePlay.Models;
using System;
using UnityEngine;

namespace AlternativePlay
{
    public class BeatSaberBehavior : MonoBehaviour
    {
        private PlayerController playerController;

        /// <summary>
        /// To be invoked every time when starting the GameCore scene.
        /// </summary>
        public void BeginGameCoreScene()
        {
            // Do nothing if we aren't playing Beat Saber
            if (Configuration.instance.ConfigurationData.PlayMode != PlayMode.BeatSaber) { return; }

            Utilities.CheckAndDisableForTrackerTransforms(Configuration.instance.ConfigurationData.LeftSaberTracker);
            Utilities.CheckAndDisableForTrackerTransforms(Configuration.instance.ConfigurationData.RightSaberTracker);
        }

        private void Awake()
        {
            this.playerController = FindObjectOfType<PlayerController>();
        }

        private void Update()
        {
            if (Configuration.instance.ConfigurationData.PlayMode != PlayMode.BeatSaber || playerController == null)
            {
                // Do nothing if we aren't playing Beat Saber
                return;
            }

            var config = Configuration.instance.ConfigurationData;

            // Check and set the left tracker
            if (!String.IsNullOrWhiteSpace(config.LeftSaberTracker?.Serial))
            {
                Pose? trackerPose = TrackedDeviceManager.instance.GetPoseFromSerial(config.LeftSaberTracker.Serial);
                if (trackerPose != null)
                {
                    Utilities.TransformSaberFromTrackerData(playerController.leftSaber.transform, config.LeftSaberTracker,
                        trackerPose.Value.rotation, trackerPose.Value.position);
                }
            }

            // Check for right tracker
            if (!String.IsNullOrWhiteSpace(config.RightSaberTracker?.Serial))
            {
                Pose? trackerPose = TrackedDeviceManager.instance.GetPoseFromSerial(config.RightSaberTracker.Serial);
                if (trackerPose != null)
                {
                    Utilities.TransformSaberFromTrackerData(playerController.rightSaber.transform, config.RightSaberTracker,
                        trackerPose.Value.rotation, trackerPose.Value.position);
                }
            }

            if (Configuration.instance.ConfigurationData.ReverseLeftSaber)
            {
                playerController.leftSaber.transform.Rotate(0.0f, 180.0f, 180.0f);
            }

            if (Configuration.instance.ConfigurationData.ReverseRightSaber)
            {
                playerController.rightSaber.transform.Rotate(0.0f, 180.0f, 180.0f);
            }
        }
    }
}

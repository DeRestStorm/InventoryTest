using System;
using UnityEngine;

namespace SuperGame
{
    [Serializable]
    public class ThirdPersonCameraSettings
    {
        [Header("Follow")]
        public float Distance = 5f;
        public float HeightOffset = 2f;

        [Header("Rotation")]
        public float Sensitivity = 3f;
        public float MinPitch = -30f;
        public float MaxPitch = 60f;

        [Header("Smoothing")]
        public float FollowSmoothing = 10f;
        public float RotationSmoothing = 10f;
    }
}

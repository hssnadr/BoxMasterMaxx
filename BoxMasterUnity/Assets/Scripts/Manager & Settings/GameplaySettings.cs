// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace CRI.HitBox.Settings
{
    [Serializable]
    public struct GameplaySettings
    {
        /// <summary>
        /// Game duration.
        /// </summary>
        [XmlElement("game_duration")]
        public int gameDuration;
        /// <summary>
        /// The starting value of the combo multiplier.
        /// </summary>
        [XmlElement("combo_min")]
        public int comboMin;
        /// <summary>
        /// The duration of the combo bar.
        /// </summary>
        [XmlElement("combo_duration")]
        public float comboDuration;
        /// <summary>
        /// Multiplier of the combo
        /// </summary>
        [XmlElement("combo_duration_multiplier")]
        public float comboDurationMultiplier;
        /// <summary>
        /// How much of the combo bar is incremented whenever a player hits a target.
        /// </summary>
        [XmlElement("combo_increment")]
        public float comboIncrement;
        /// <summary>
        /// The max value of the combo multiplier.
        /// </summary>
        [XmlElement("combo_max")]
        public int comboMax;
        /// <summary>
        /// Rotation speed.
        /// </summary>
        [XmlElement("rotation_speed")]
        public float rotationSpeed;
        /// <summary>
        /// Z rotation speed.
        /// </summary>
        [XmlElement("z_rotation_speed")]
        public float zRotationSpeed;
        /// <summary>
        /// Max angular velocity.
        /// </summary>
        [XmlElement("max_angular_velocity")]
        public float maxAngularVelocity;
        /// <summary>
        /// The min amount of points a player can get while hitting a sphere.
        /// </summary>
        [XmlElement("min_points")]
        public int minPoints;
        /// <summary>
        /// The max amount of points a player can get while hitting a sphere.
        /// </summary>
        [XmlElement("max_points")]
        public int maxPoints;
        /// <summary>
        /// The max distance from the center of the target to get the maximum of points.
        /// </summary>
        [XmlElement("tolerance")]
        public float tolerance;
        /// <summary>
        /// Delay before the target is activated (in seconds).
        /// </summary>
        [XmlElement("target_activation_delay")]
        public float targetActivationDelay;
        /// <summary>
        /// Time until a target is ready to be activated again after its last deactivation (in seconds).
        /// </summary>
        [XmlElement("target_cooldown")]
        public float targetCooldown;

        public GameplaySettings(int gameDuration,
            int comboMin,
            int comboMax,
            float comboDuration,
            float comboDurationMultiplier,
            float comboIncrement,
            float rotationSpeed,
            float zRotationSpeed,
            float maxAngularVelocity,
            int minPoints,
            int maxPoints,
            float maxDistance,
            float targetActivationDelay,
            float targetCooldown)
        {
            this.gameDuration = gameDuration;
            this.comboMin = comboMin;
            this.comboMax = comboMax;
            this.comboDuration = comboDuration;
            this.comboDurationMultiplier = comboDurationMultiplier;
            this.comboIncrement = comboIncrement;
            this.rotationSpeed = rotationSpeed;
            this.zRotationSpeed = zRotationSpeed;
            this.maxAngularVelocity = maxAngularVelocity;
            this.minPoints = minPoints;
            this.maxPoints = maxPoints;
            this.tolerance = Mathf.Clamp(maxDistance, 0.0f, 0.999f);
            this.targetActivationDelay = targetActivationDelay;
            this.targetCooldown = targetCooldown;
        }
    }
}
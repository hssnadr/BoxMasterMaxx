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
        [XmlElement("target_rotation_speed")]
        public float targetRotationSpeed;
        /// <summary>
        /// Z rotation speed.
        /// </summary>
        [XmlElement("target_z_rotation_speed")]
        public float targetZRotationSpeed;
        /// <summary>
        /// Max angular velocity.
        /// </summary>
        [XmlElement("target_max_angular_velocity")]
        public float targetMaxAngularVelocity;
        /// <summary>
        /// Delay before the target is activated in P1 mode (in seconds).
        /// </summary>
        [XmlElement("target_activation_delay")]
        public float targetP1ActivationDelay;
        /// <summary>
        /// Time until a target is ready to be activated again after its last deactivation (in seconds).
        /// </summary>
        [XmlElement("target_cooldown")]
        public float targetCooldown;
        /// <summary>
        /// The min amount of points a player can get while hitting a sphere.
        /// </summary>
        [XmlElement("hit_min_points")]
        public int hitMinPoints;
        /// <summary>
        /// The max amount of points a player can get while hitting a sphere.
        /// </summary>
        [XmlElement("hit_max_points")]
        public int hitMaxPoints;
        /// <summary>
        /// The max distance from the center of the target to get the maximum of points.
        /// </summary>
        [XmlElement("hit_tolerance")]
        public float hitTolerance;
        /// <summary>
        /// Threshold for increasing the number of targets.
        /// </summary>
        [XmlArray("target_count_threshold")]
        [XmlArrayItem(typeof(int), ElementName = "threshold")]
        public int[] targetCountThreshold;
        /// <summary>
        /// Precision required to get the max number of stars.
        /// </summary>
        [XmlElement("max_precision_rating")]
        public float maxPrecisionRating;
        /// <summary>
        /// Under this precision value, the player will get 0 star.
        /// </summary>
        [XmlElement("min_precision_rating")]
        public float minPrecisionRating;
        /// <summary>
        /// Speed required to get the max number of stars. The speed is the average time between hits in seconds.
        /// </summary>
        [XmlElement("max_speed_rating")]
        public float maxSpeedRating;
        /// <summary>
        /// Speed required to get the min number of stars. The speed is the average time between hits in seconds.
        /// </summary>
        [XmlElement("min_speed_rating")]
        public float minSpeedRating;

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
            float targetP1ActivationDelay,
            float targetCooldown,
            int[] sphereCountThreshold,
            float maxPrecision,
            float minPrecision,
            float maxSpeed,
            float minSpeed)
        {
            this.gameDuration = gameDuration;
            this.comboMin = comboMin;
            this.comboMax = comboMax;
            this.comboDuration = comboDuration;
            this.comboDurationMultiplier = comboDurationMultiplier;
            this.comboIncrement = comboIncrement;
            this.targetRotationSpeed = rotationSpeed;
            this.targetZRotationSpeed = zRotationSpeed;
            this.targetMaxAngularVelocity = maxAngularVelocity;
            this.hitMinPoints = minPoints;
            this.hitMaxPoints = maxPoints;
            this.hitTolerance = Mathf.Clamp(maxDistance, 0.0f, 0.999f);
            this.targetP1ActivationDelay = targetP1ActivationDelay;
            this.targetCooldown = targetCooldown;
            this.targetCountThreshold = sphereCountThreshold;
            this.maxPrecisionRating = maxPrecision;
            this.minPrecisionRating = minPrecision;
            this.maxSpeedRating = maxSpeed;
            this.minSpeedRating = minSpeed;
        }
    }
}
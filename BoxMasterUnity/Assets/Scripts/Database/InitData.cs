using CRI.HitBox.Extensions;
using CRI.HitBox.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class InitData : DataEntry
    {
        /// <summary>
        /// Index of the init in the database.
        /// </summary>
        [Field("id"), AutoIncrement, PrimaryKey]
        public int id { get; set; }
        /// <summary>
        /// Time until the application displays the timeout screen.
        /// </summary>
        [Field("timeout_screen")]
        public int timeoutScreen { get; set; }
        /// <summary>
        /// Time until the application returns to the home screen.
        /// </summary>
        [Field("timeout")]
        public int timeout { get; set; }
        /// <summary>
        /// Duration of the game.
        /// </summary>
        [Field("game_duration")]
        public int gameDuration { get; set; }
        /// <summary>
        /// The starting value of the combo multiplier.
        /// </summary>
        [Field("combo_min")]
        public int comboMin { get; set; }
        /// <summary>
        /// The max value of the combo multiplier.
        /// </summary>
        [Field("combo_max")]
        public int comboMax { get; set; }
        /// <summary>
        /// The duration of the combo bar (in seconds).
        /// </summary>
        [Field("combo_duration")]
        public float comboDuration { get; set; }
        /// <summary>
        /// The multiplier of the duration of the combo bar according to the combo value.
        /// </summary>
        [Field("combo_duration_multiplier")]
        public float comboDurationMultiplier { get; set; }
        /// <summary>
        /// How much of the combo bar is incremented whenever a player hits a target.
        /// The value goes from 0.0 (nothing) to 1.0 (the full bar).
        /// </summary>
        [Field("combo_increment")]
        public float comboIncrement { get; set; }
        /// <summary>
        /// Rotation speed of the target when hit by a player.
        /// </summary>
        [Field("target_rotation_speed")]
        public float targetRotationSpeed { get; set; }
        /// <summary>
        /// The Z rotation speed of the movement controller.
        /// </summary>
        [Field("target_z_rotation_speed")]
        public float targetZRotationSpeed { get; set; }
        /// <summary>
        /// The horizontal movement speed of the movement controller when the difficulty level is high enough.
        /// </summary>
        [Field("target_horizontal_movement_speed")]
        public float targetHorizontalMovementSpeed { get; set; }
        /// <summary>
        /// Max angular velocity of the targets.
        /// </summary>
        [Field("target_max_angular_velocity")]
        public float targetMaxAngularVelocity { get; set; }
        /// <summary>
        /// Delay before the next target is activated in P1 mode (in seconds).
        /// </summary>
        [Field("target_p1_activation_delay")]
        public float targetP1ActivationDelay { get; set; }
        /// <summary>
        /// Time until a target is ready to be activated again after its last hit.
        /// </summary>
        [Field("target_cooldown")]
        public float targetCooldown { get; set; }
        /// <summary>
        /// The min amount of points a player can get while hitting a sphere.
        /// </summary>
        [Field("hit_min_points")]
        public int hitMinPoints { get; set; }
        /// <summary>
        /// The min amount of points a player can get while hitting a sphere.
        /// </summary>
        [Field("hit_max_points")]
        public int hitMaxPoints { get; set; }
        /// <summary>
        /// The max distance from the center of the target to get the maximum amount of points.
        /// The distance goes from 0.0 (the center of the target) to 1.0 (the border of the target).
        /// </summary>
        [Field("hit_tolerance")]
        public float hitTolerance { get; set; }
        /// <summary>
        /// Precision required to get the max number of stars.
        /// The precision goes from 0.0 (the player missed all their hits) to 1.0 (the player missed none of their hits).
        /// </summary>
        [Field("max_precision_rating")]
        public float maxPrecisionRating { get; set; }
        /// <summary>
        /// Precision required to get the min number of stars.
        /// The precision goes from 0.0 (the player missed all their hits) to 1.0 (the player missed none of their hits).
        /// </summary>
        [Field("min_precision_rating")]
        public float minPrecisionRating { get; set; }
        /// <summary>
        /// Speed required to get the max number of stars.
        /// The speed is the average time between hits in seconds.
        /// </summary>
        [Field("max_speed_rating")]
        public float maxSpeedRating { get; set; }
        /// <summary>
        /// Speed required to get the min number of stars.
        /// The speed is the average time between hits in seconds.
        /// </summary>
        [Field("min_speed_rating")]
        public float minSpeedRating { get; set; }
        /// <summary>
        /// Min value to detect impact.
        /// </summary>
        [Field("impact_threshold")]
        public float impactThreshold { get; set; }
        /// <summary>
        /// Minimum time (in ms) between 2 impacts to be validated (minimum 50ms)
        /// </summary>
        [Field("delay_off_hit")]
        public int delayOffHit { get; set; }

        public const string name = "init";
        public const string tableName = "inits";

        /// <summary>
        /// Creates an instance of InitData from an instance of ApplicationSettings
        /// </summary>
        /// <param name="settings">An instance of ApplicationSettings</param>
        /// <returns>An instance of InitData</returns>
        public static InitData CreateFromApplicationSettings(ApplicationSettings settings)
        {
            var res = new InitData();
            res.timeoutScreen = settings.menuSettings.timeoutScreen;
            res.timeout = settings.menuSettings.timeout;
            res.gameDuration = settings.gameSettings.gameDuration;
            res.comboMin = settings.gameSettings.comboMin;
            res.comboMax = settings.gameSettings.comboMax;
            res.comboDuration = settings.gameSettings.comboDuration;
            res.comboDurationMultiplier = settings.gameSettings.comboDurationMultiplier;
            res.comboIncrement = settings.gameSettings.comboIncrement;
            res.targetRotationSpeed = settings.gameSettings.targetRotationSpeed;
            res.targetZRotationSpeed = settings.gameSettings.targetZRotationSpeed;
            res.targetHorizontalMovementSpeed = settings.gameSettings.targetHorizontalMovementSpeed;
            res.targetMaxAngularVelocity = settings.gameSettings.targetMaxAngularVelocity;
            res.targetP1ActivationDelay = settings.gameSettings.targetP1ActivationDelay;
            res.targetCooldown = settings.gameSettings.targetCooldown;
            res.hitMinPoints = settings.gameSettings.hitMinPoints;
            res.hitMaxPoints = settings.gameSettings.hitMaxPoints;
            res.hitTolerance = settings.gameSettings.hitTolerance;
            res.maxPrecisionRating = settings.gameSettings.maxPrecisionRating;
            res.minPrecisionRating = settings.gameSettings.minPrecisionRating;
            res.maxSpeedRating = settings.gameSettings.maxSpeedRating;
            res.minSpeedRating = settings.gameSettings.minSpeedRating;
            res.impactThreshold = settings.serialSettings.impactThreshold;
            res.delayOffHit = settings.serialSettings.delayOffHit;
            return res;
        }

        /// <summary>
        /// Compares the init data to an application settings.
        /// </summary>
        /// <param name="settings">Application settings.</param>
        /// <returns>True if the init data has the same values as the application settings.</returns>
        public bool Equals(ApplicationSettings settings)
        {
            return (
                timeoutScreen == settings.menuSettings.timeoutScreen
                && timeout == settings.menuSettings.timeout
                && gameDuration == settings.gameSettings.gameDuration
                && comboMin == settings.gameSettings.comboMin
                && comboMax == settings.gameSettings.comboMax
                && comboDuration == settings.gameSettings.comboDuration
                && comboDurationMultiplier == settings.gameSettings.comboDurationMultiplier
                && comboIncrement == settings.gameSettings.comboIncrement
                && targetRotationSpeed == settings.gameSettings.targetRotationSpeed
                && targetZRotationSpeed == settings.gameSettings.targetZRotationSpeed
                && targetHorizontalMovementSpeed == settings.gameSettings.targetHorizontalMovementSpeed
                && targetMaxAngularVelocity == settings.gameSettings.targetMaxAngularVelocity
                && targetP1ActivationDelay == settings.gameSettings.targetP1ActivationDelay
                && targetCooldown == settings.gameSettings.targetCooldown
                && hitMinPoints == settings.gameSettings.hitMinPoints
                && hitMaxPoints == settings.gameSettings.hitMaxPoints
                && hitTolerance == settings.gameSettings.hitTolerance
                && maxPrecisionRating == settings.gameSettings.maxPrecisionRating
                && minPrecisionRating == settings.gameSettings.minPrecisionRating
                && maxSpeedRating == settings.gameSettings.maxSpeedRating
                && minSpeedRating == settings.gameSettings.minSpeedRating
                && impactThreshold == settings.serialSettings.impactThreshold
                && delayOffHit == settings.serialSettings.delayOffHit
                );
        }

        public override string GetTypeName()
        {
            return name;
        }

        public override string GetTableName()
        {
            return tableName;
        }

        public InitData(int id, int timeoutScreen, int timeout, int gameDuration, int comboMin,
            int comboMax, float comboDuration, float comboDurationMultiplier, float comboIncrement,
            float targetRotationSpeed, float targetZRotationSpeed, float targetHorizontalMovementSpeed,
            float targetMaxAngularVelocity, float targetP1ActivationDelay, float targetCooldown,
            int hitMinPoints, int hitMaxPoints, float hitTolerance, float maxPrecisionRating,
            float minPrecisionRating, float maxSpeedRating, float minSpeedRating,
            int impactThreshold, int delayOffHit)
        {
            this.id = id;
            this.timeoutScreen = timeoutScreen;
            this.timeout = timeout;
            this.gameDuration = gameDuration;
            this.comboMin = comboMin;
            this.comboMax = comboMax;
            this.comboDuration = comboDuration;
            this.comboDurationMultiplier = comboDurationMultiplier;
            this.comboIncrement = comboIncrement;
            this.targetRotationSpeed = targetRotationSpeed;
            this.targetZRotationSpeed = targetZRotationSpeed;
            this.targetHorizontalMovementSpeed = targetHorizontalMovementSpeed;
            this.targetMaxAngularVelocity = targetMaxAngularVelocity;
            this.targetP1ActivationDelay = targetP1ActivationDelay;
            this.targetCooldown = targetCooldown;
            this.hitMinPoints = hitMinPoints;
            this.hitMaxPoints = hitMaxPoints;
            this.hitTolerance = hitTolerance;
            this.maxPrecisionRating = maxPrecisionRating;
            this.minPrecisionRating = minPrecisionRating;
            this.maxSpeedRating = maxSpeedRating;
            this.minSpeedRating = minSpeedRating;
            this.impactThreshold = impactThreshold;
            this.delayOffHit = delayOffHit;
        }

        public InitData() { }
    }
}

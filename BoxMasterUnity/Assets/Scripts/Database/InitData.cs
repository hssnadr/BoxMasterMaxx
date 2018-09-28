using CRI.HitBox.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRI.HitBox.Database
{
    public class InitData
    {
        /// <summary>
        /// Index of the init in the database.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Time until the application displays the timeout screen.
        /// </summary>
        public int timeoutScreen { get; set; }
        /// <summary>
        /// Time until the application returns to the home screen.
        /// </summary>
        public int timeout { get; set; }
        /// <summary>
        /// Duration of the game.
        /// </summary>
        public int gameDuration { get; set; }
        /// <summary>
        /// The starting value of the combo multiplier.
        /// </summary>
        public int comboMin { get; set; }
        /// <summary>
        /// The max value of the combo multiplier.
        /// </summary>
        public int comboMax { get; set; }
        /// <summary>
        /// The duration of the combo bar (in seconds).
        /// </summary>
        public float comboDuration { get; set; }
        /// <summary>
        /// The multiplier of the duration of the combo bar according to the combo value.
        /// </summary>
        public float comboDurationMultiplier { get; set; }
        /// <summary>
        /// How much of the combo bar is incremented whenever a player hits a target.
        /// The value goes from 0.0 (nothing) to 1.0 (the full bar).
        /// </summary>
        public float comboIncrement { get; set; }
        /// <summary>
        /// Rotation speed of the target when hit by a player.
        /// </summary>
        public float targetRotationSpeed { get; set; }
        /// <summary>
        /// The Z rotation speed of the movement controller.
        /// </summary>
        public float targetZRotationSpeed { get; set; }
        /// <summary>
        /// The horizontal movement speed of the movement controller when the difficulty level is high enough.
        /// </summary>
        public float targetHorizontalMovementSpeed { get; set; }
        /// <summary>
        /// Max angular velocity of the targets.
        /// </summary>
        public float targetMaxAngularVelocity { get; set; }
        /// <summary>
        /// Delay before the next target is activated in P1 mode (in seconds).
        /// </summary>
        public float targetP1ActivationDelay { get; set; }
        /// <summary>
        /// Time until a target is ready to be activated again after its last hit.
        /// </summary>
        public float targetCooldown { get; set; }
        /// <summary>
        /// The min amount of points a player can get while hitting a sphere.
        /// </summary>
        public int hitMinPoints { get; set; }
        /// <summary>
        /// The min amount of points a player can get while hitting a sphere.
        /// </summary>
        public int hitMaxPoints { get; set; }
        /// <summary>
        /// The max distance from the center of the target to get the maximum amount of points.
        /// The distance goes from 0.0 (the center of the target) to 1.0 (the border of the target).
        /// </summary>
        public float hitTolerance { get; set; }
        /// <summary>
        /// Precision required to get the max number of stars.
        /// The precision goes from 0.0 (the player missed all their hits) to 1.0 (the player missed none of their hits).
        /// </summary>
        public float maxPrecisionRating { get; set; }
        /// <summary>
        /// Precision required to get the min number of stars.
        /// The precision goes from 0.0 (the player missed all their hits) to 1.0 (the player missed none of their hits).
        /// </summary>
        public float minPrecisionRating { get; set; }
        /// <summary>
        /// Speed required to get the max number of stars.
        /// The speed is the average time between hits in seconds.
        /// </summary>
        public float maxSpeedRating { get; set; }
        /// <summary>
        /// Speed required to get the min number of stars.
        /// The speed is the average time between hits in seconds.
        /// </summary>
        public float minSpeedRating { get; set; }
        /// <summary>
        /// Min value to detect impact.
        /// </summary>
        public float impactThreshold { get; set; }
        /// <summary>
        /// Minimum time (in ms) between 2 impacts to be validated (minimum 50ms)
        /// </summary>
        public int delayOffHit { get; set; }

        /// <summary>
        /// Creates an instance of InitData from an instance of ApplicationSettings
        /// </summary>
        /// <param name="settings">An instance of ApplicationSettings</param>
        /// <returns>An instance of InitData</returns>
        public static InitData CreateFromApplicationSettings(ApplicationSettings settings)
        {
            var data = new InitData();
            data.timeoutScreen = settings.menuSettings.timeoutScreen;
            data.timeout = settings.menuSettings.timeout;
            data.gameDuration = settings.gameSettings.gameDuration;
            data.comboMin = settings.gameSettings.comboMin;
            data.comboMax = settings.gameSettings.comboMax;
            data.comboDuration = settings.gameSettings.comboDuration;
            data.comboDurationMultiplier = settings.gameSettings.comboDurationMultiplier;
            data.comboIncrement = settings.gameSettings.comboIncrement;
            data.targetRotationSpeed = settings.gameSettings.targetRotationSpeed;
            data.targetZRotationSpeed = settings.gameSettings.targetZRotationSpeed;
            data.targetHorizontalMovementSpeed = settings.gameSettings.targetHorizontalMovementSpeed;
            data.targetMaxAngularVelocity = settings.gameSettings.targetMaxAngularVelocity;
            data.targetP1ActivationDelay = settings.gameSettings.targetP1ActivationDelay;
            data.targetCooldown = settings.gameSettings.targetCooldown;
            data.hitMinPoints = settings.gameSettings.hitMinPoints;
            data.hitMaxPoints = settings.gameSettings.hitMaxPoints;
            data.hitTolerance = settings.gameSettings.hitTolerance;
            data.maxPrecisionRating = settings.gameSettings.maxPrecisionRating;
            data.minPrecisionRating = settings.gameSettings.minPrecisionRating;
            data.maxSpeedRating = settings.gameSettings.maxSpeedRating;
            data.minSpeedRating = settings.gameSettings.minSpeedRating;
            return data;
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
                );
        }
    }
}

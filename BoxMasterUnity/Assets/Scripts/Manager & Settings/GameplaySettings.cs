using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

[Serializable]
public struct GameplaySettings
{
    /// <summary>
    /// Game duration.
    /// </summary>
    [XmlElement("game_duration")]
    public int gameDuration;
    /// <summary>
    /// The max value of the combo multiplier.
    /// </summary>
    [XmlElement("combo_multiplier_max_value")]
    public int comboMultiplierMaxValue;
    /// <summary>
    /// How many hits until the combo multiplier increases.
    /// </summary>
    [XmlElement("combo_multiplier_threshold")]
    public int comboMultiplierThreshold;

    public GameplaySettings(int gameDuration, int comboMultiplierMaxValue, int comboMultiplierThreshold)
    {
        this.gameDuration = gameDuration;
        this.comboMultiplierMaxValue = comboMultiplierMaxValue;
        this.comboMultiplierThreshold = comboMultiplierThreshold;
    }
}

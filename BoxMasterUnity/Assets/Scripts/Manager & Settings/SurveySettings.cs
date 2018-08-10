using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

/// <summary>
/// An answer for the survey.
/// </summary>
public struct SurveyAnswer
{
    /// <summary>
    /// Which action the button will do when clicked on.
    /// None = no action.
    /// Show = shows the rest of the questions.
    /// End = ends the survey.
    /// </summary>
    public enum ButtonAction
    {
        [XmlEnum(Name = "none")]
        None = 0,
        [XmlEnum(Name = "show")]
        Show = 1,
        [XmlEnum(Name = "end")]
        End = 2,
    }

    /// <summary>
    /// Which action the button will do when clicked on.
    /// None = no action.
    /// Show = shows the rest of the questions.
    /// End = ends the survey.
    /// </summary>
    [XmlAttribute("action")]
    public ButtonAction buttonAction;
    /// <summary>
    /// The text key of the answer.
    /// </summary>
    [XmlText]
    public string key;
}

/// <summary>
/// A survey question.
/// </summary>
[System.Serializable]
public struct SurveyQuestion
{
    /// <summary>
    /// The textkey of the question that will be answered.
    /// </summary>
    [XmlAttribute("key")]
    public string key;
    /// <summary>
    /// All the possible answers to the question. 
    /// </summary>
    [XmlArray("answers")]
    [XmlArrayItem(typeof(SurveyAnswer), ElementName = "answer")]
    public SurveyAnswer[] answers;
}

[Serializable]
public class SurveySettings
{
    /// <summary>
    /// The textkey of the title of the P1 part of the survey.
    /// </summary>
    [XmlAttribute("p1_title_key")]
    public string p1titlekey;
    /// <summary>
    /// The textkey of the title of the P2 part of the survey.
    /// </summary>
    [XmlAttribute("p2_title_key")]
    public string p2titlekey;
    /// <summary>
    /// The list of questions the player will have to answer before the game begins.
    /// </summary>
    [XmlArray(ElementName = "survey_questions")]
    [XmlArrayItem(typeof(SurveyQuestion), ElementName = "survey_question")]
    public SurveyQuestion[] surveyQuestions;

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UISurveyScreen : UIPage {
    [System.Serializable]
    public class QuestionAnswerKeys
    {
        /// <summary>
        /// The text key of a question of the survey.
        /// </summary>
        public string questionKey;
        /// <summary>
        /// The corresponding text key of the answer.
        /// </summary>
        public string answerKey;

        public QuestionAnswerKeys(string questionKey, string answerKey)
        {
            this.questionKey = questionKey;
            this.answerKey = answerKey;
        }
    }

    [SerializeField]
    protected UISurveyQuestion _surveyQuestionPrefab;
    [SerializeField]
    protected Color _buttonColor1;
    [SerializeField]
    protected Color _buttonColor2;
    [SerializeField]
    protected CanvasGroup _panelP1;
    [SerializeField]
    protected CanvasGroup _panelP2;

    private List<UISurveyQuestion> _surveyQuestionsP1;
    private List<UISurveyQuestion> _surveyQuestionsP2;

    [SerializeField]
    private QuestionAnswerKeys[] _answersP1;
    [SerializeField]
    private QuestionAnswerKeys[] _answersP2;

    private bool _skipP1;
    private bool _skipP2;

    private bool _surveyStarted;

    public bool surveyEndedP1
    {
        get
        {
            return (_skipP1 || (_answersP1 != null && _answersP1.All(x => x != null)));
        }
    }

    public bool surveyEndedP2
    {

        get
        {
            return (_skipP2 || (_answersP1 != null && _answersP2.All(x => x != null)));
        }
    }

    private void OnEnable()
    {
        UISurveyQuestion.onAnswer += OnAnswer;
    }

    private void OnDisable()
    {
        UISurveyQuestion.onAnswer -= OnAnswer;
    }

    private void Start()
    {
        _surveyQuestionsP1 = new List<UISurveyQuestion>();
        _surveyQuestionsP2 = new List<UISurveyQuestion>();
        var surveyQuestions = GameManager.instance.gameSettings.surveyQuestions;

        for (int i = 0; i < surveyQuestions.Length; i++)
        {
            var question = surveyQuestions[i];
            UISurveyQuestion go1 = GameObject.Instantiate(_surveyQuestionPrefab, _panelP1.transform);
            UISurveyQuestion go2 = GameObject.Instantiate(_surveyQuestionPrefab, _panelP2.transform);
            go1.Init(question, i, 0, _buttonColor1, _buttonColor2);
            go2.Init(question, i, 1, _buttonColor1, _buttonColor2);
            go1.interactable = go2.interactable = (i == 0);
            _surveyQuestionsP1.Add(go1);
            _surveyQuestionsP2.Add(go2);
        }

        _answersP1 = new QuestionAnswerKeys[_surveyQuestionsP1.Count];
        _answersP2 = new QuestionAnswerKeys[_surveyQuestionsP2.Count];
    }

    public void Update()
    {
        displayNext = _surveyStarted && surveyEndedP1 && surveyEndedP2;
    }

    public override void Show()
    {
        base.Show();
        _surveyStarted = true;
    }

    public override void Hide()
    {
        base.Hide();
        _surveyStarted = false;
    }

    private void OnAnswer(string questionKey, string answerKey, SurveyAnswer.ButtonAction action, int index, int playerIndex)
    {
        List<UISurveyQuestion> survey = null;
        if (playerIndex == 0)
        {
            _answersP1[index] = new QuestionAnswerKeys(questionKey, answerKey);
            survey = _surveyQuestionsP1;
        }
        else if (playerIndex == 1)
        {
            _answersP2[index] = new QuestionAnswerKeys(questionKey, answerKey);
            survey = _surveyQuestionsP2;
        }

        if (action == SurveyAnswer.ButtonAction.Show)
        {
            foreach (var question in survey)
                question.interactable = true;
            if (playerIndex == 0)
                _skipP1 = false;
            if (playerIndex == 1)
                _skipP2 = false;
        }
        if (action == SurveyAnswer.ButtonAction.End)
        {
            for (int i = 1; i < survey.Count; i++)
                survey[i].interactable = false;
            if (playerIndex == 0)
            {
                _skipP1 = true;
                for (int i = 1; i < _answersP1.Length; i++)
                    _answersP1[i] = null;
            }
            if (playerIndex == 1)
            {
                _skipP2 = true;
                for (int i = 1; i < _answersP2.Length; i++)
                    _answersP2[i] = null;
            }
        }
    }
}

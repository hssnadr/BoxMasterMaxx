// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using CRI.HitBox.Settings;
using CRI.HitBox.Lang;

namespace CRI.HitBox.UI
{
    public class UISurveyScreen : UIPage<SurveyPageSettings>
    {
        public delegate void SurveyScreenEvent(List<string> answersP1, List<string> answersP2);
        public static event SurveyScreenEvent onSurveyEnd;
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

        private QuestionAnswerKeys[] _answersP1;
        private QuestionAnswerKeys[] _answersP2;
        [SerializeField]
        private TranslatedText _titleP1 = null;
        [SerializeField]
        private TranslatedText _titleP2 = null;
        [SerializeField]
        private Image _backgroundCanvasP1 = null;
        [SerializeField]
        private Image _backgroundCanvasP2 = null;

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
            ApplicationManager.onSetupEnd += OnSetupEnd;
            ApplicationManager.onStartPages += OnStartPages;
        }

        private void OnDisable()
        {
            UISurveyQuestion.onAnswer -= OnAnswer;
            ApplicationManager.onSetupEnd -= OnSetupEnd;
            ApplicationManager.onStartPages -= OnStartPages;
        }

        private void Start()
        {
            Init();
        }

        public void Update()
        {
            _displayNext = _surveyStarted
                && (surveyEndedP1 || (ApplicationManager.instance.gameMode == GameMode.P1 && ApplicationManager.instance.soloIndex == 1))
                && (surveyEndedP2 || (ApplicationManager.instance.gameMode == GameMode.P1 && ApplicationManager.instance.soloIndex == 0));
            _nextStyle = ApplicationManager.instance.gameMode == GameMode.P1 && ApplicationManager.instance.soloIndex == 0 ? 2 : 3;
        }

        public override void Show()
        {
            base.Show();

            bool p2Mode = ApplicationManager.instance.gameMode == GameMode.P2;
            var p1ModeLeft = ApplicationManager.instance.gameMode == GameMode.P1 && ApplicationManager.instance.soloIndex == 0;

            _panelP2.alpha = p2Mode || !p1ModeLeft ? 1.0f : 0.0f;
            _panelP2.interactable = p2Mode || !p1ModeLeft;
            _panelP2.blocksRaycasts = p2Mode || !p1ModeLeft;

            _panelP1.alpha = p2Mode || p1ModeLeft ? 1.0f : 0.0f;
            _panelP1.interactable = p2Mode || p1ModeLeft;
            _panelP1.blocksRaycasts = p2Mode || p1ModeLeft;

            _surveyStarted = true;
        }

        public override void Hide()
        {
            base.Hide();
            _surveyStarted = false;
        }

        private void Init()
        {
            Color p1Color = Color.white;
            Color p2Color = Color.white;
            _surveyQuestionsP1 = new List<UISurveyQuestion>();
            _surveyQuestionsP2 = new List<UISurveyQuestion>();
            var surveyQuestions = ApplicationManager.instance.menuSettings.surveySettings.surveyQuestions;

            _titleP1.InitTranslatedText(ApplicationManager.instance.menuSettings.surveySettings.p1titlekey);
            _titleP2.InitTranslatedText(ApplicationManager.instance.menuSettings.surveySettings.p2titlekey);

            if (ColorUtility.TryParseHtmlString(ApplicationManager.instance.appSettings.p1Color, out p1Color))
                _backgroundCanvasP1.color = p1Color;
            if (ColorUtility.TryParseHtmlString(ApplicationManager.instance.appSettings.p2Color, out p2Color))
                _backgroundCanvasP2.color = p2Color;

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

        private void OnStartPages(bool switchLanguages)
        {
            for (int i = 0; i < _surveyQuestionsP1.Count; i++)
            {
                _surveyQuestionsP1[i].Reset();
                _surveyQuestionsP2[i].Reset();
                _surveyQuestionsP1[i].interactable = _surveyQuestionsP2[i].interactable = (i == 0);
                _answersP1[i] = null;
                _answersP2[i] = null;
                _skipP1 = false;
                _skipP2 = false;
            }
            _surveyStarted = false;
        }

        private void OnSetupEnd()
        {
            if (ApplicationManager.instance.gameMode == GameMode.P1)
            {
                List<string> answersP1 = _answersP1.Where(x => x != null).Select(x => x.answerKey).ToList();
                List<string> answersP2 = _answersP2.Where(x => x != null).Select(x => x.answerKey).ToList();
                if (onSurveyEnd != null)
                    onSurveyEnd(answersP1, answersP2);
            }
            else
            {
                List<string> answersP1 = _answersP1.Where(x => x != null).Select(x => x.answerKey).ToList();
                List<string> answersP2 = _answersP2.Where(x => x != null).Select(x => x.answerKey).ToList();
                if (onSurveyEnd != null)
                    onSurveyEnd(answersP1, answersP2);
            }
        }
    }
}
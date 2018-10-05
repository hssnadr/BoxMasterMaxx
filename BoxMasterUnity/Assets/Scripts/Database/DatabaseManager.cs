using CRI.HitBox.Game;
using CRI.HitBox.Lang;
using CRI.HitBox.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CRI.HitBox.Database
{
    public class DatabaseManager : MonoBehaviour
    {
        private static DatabaseManager s_instance;

        public static DatabaseManager instance
        {
            get
            {
                return s_instance;
            }
        }

        public SessionData currentSession;

        public List<PlayerData> currentPlayers;

        public List<SurveyData> currentSurveys;

        public CrashData currentCrash;

        public InitData currentInit;

        private void OnEnable()
        {
            ApplicationManager.onReturnToHome += OnReturnToHome;
            ApplicationManager.onTimeOutScreen += OnTimeOutScreen;
            ApplicationManager.onTimeOut += OnTimeOut;
            ApplicationManager.onStartPages += OnStartPages;
            ApplicationManager.onGameStart += OnGameStart;
            ApplicationManager.onGameEnd += OnGameEnd;
            ApplicationManager.onSwitchLanguages += OnSwitchLanguages;
            UISurveyScreen.onSurveyEnd += OnSurveyEnd;
            GameManager.onPlayerSetup += OnPlayerSetup;
        }

        private void OnDisable()
        {
            ApplicationManager.onReturnToHome -= OnReturnToHome;
            ApplicationManager.onTimeOutScreen -= OnTimeOutScreen;
            ApplicationManager.onTimeOut -= OnTimeOut;
            ApplicationManager.onStartPages -= OnStartPages;
            ApplicationManager.onGameStart -= OnGameStart;
            ApplicationManager.onGameEnd -= OnGameEnd;
            UISurveyScreen.onSurveyEnd -= OnSurveyEnd;
            GameManager.onPlayerSetup -= OnPlayerSetup;
        }

        private void Awake()
        {
            s_instance = this;
        }

        private async void OnSurveyEnd(List<string> answersP1, List<string> answersP2)
        {
            var player1 = currentPlayers.FirstOrDefault(x => x.playerIndex == 0);
            var player2 = currentPlayers.FirstOrDefault(x => x.playerIndex == 1);
            if (answersP1 != null && player1 != null)
            {
                foreach (var answer in answersP1)
                {
                    await DataService.InsertData(new SurveyData(player1, answer));
                }
            }
            if (answersP2 != null && player2 != null)
            {
                foreach (var answer in answersP2)
                {
                    await DataService.InsertData(new SurveyData(player2, answer));
                }
            }
        }

        private async void OnPlayerSetup(Vector2 position, int playerIndex)
        {
            var player = currentPlayers.First(x => x.playerIndex == playerIndex);
            player.setupHitPosition = position;
            await DataService.UpdateData(player); 
        }

        private async void OnGameStart(GameMode gameMode, int soloIndex)
        {
            currentSession.gameMode = gameMode;
            currentSession.timeSpentOnMenu = (currentSession.time - DateTime.Now).Seconds;
            if (gameMode == GameMode.P1)
            {
                var player = new PlayerData(0, currentSession, soloIndex, Vector3.zero);
                currentPlayers.Add(player);
                await DataService.InsertData(player);
            }
            if (gameMode == GameMode.P2)
            {
                var player1 = new PlayerData(0, currentSession, 0, Vector3.zero);
                var player2 = new PlayerData(0, currentSession, 1, Vector3.zero);
                currentPlayers.Add(player1);
                currentPlayers.Add(player2);
                await DataService.InsertData(player1);
                await DataService.InsertData(player2);
            }
        }

        private async void OnTimeOut()
        {
            currentSession.timeout = false;
            await DataService.UpdateData(currentSession);
        }

        private async void OnTimeOutScreen()
        {
            currentSession.timeoutScreenCount++;
            await DataService.UpdateData(currentSession);
        }

        private async void OnStartPages()
        {
            currentSession = new SessionData(0, currentInit, DateTime.Now, TextManager.instance.currentLang.code);
            await DataService.InsertData(currentSession); 
        }

        private void OnSwitchLanguages()
        {
            OnReturnToHome();
        }

        private async void OnReturnToHome()
        {
            var currentSession = this.currentSession;
            ResetSession();
            if (currentSession != null)
            {
                currentSession.timeSpentTotal = (DateTime.Now - currentSession.time).Seconds;
                // If the game has started
                if (currentSession.timeSpentOnMenu != null)
                {
                    currentSession.score = ApplicationManager.instance.GetComponent<GameManager>().playerScore;
                    currentSession.speedRating = ApplicationManager.instance.GetComponent<GameManager>().speed;
                    currentSession.precisionRating = ApplicationManager.instance.GetComponent<GameManager>().precision;
                    currentSession.highestComboMultiplier = ApplicationManager.instance.GetComponent<GameManager>().highestComboMultiplier;
                }
                else
                {
                    currentSession.timeSpentOnMenu = (DateTime.Now - currentSession.time).Seconds;
                }
                await DataService.UpdateData(currentSession);
            }
        }

        public void ResetSession()
        {
            currentSession = null;
            currentPlayers = new List<PlayerData>();
            currentSurveys = null;
            currentCrash = null;
        }

        private async void OnGameEnd()
        {
            currentSession.score = ApplicationManager.instance.GetComponent<GameManager>().playerScore;
            currentSession.speedRating = ApplicationManager.instance.GetComponent<GameManager>().speed;
            currentSession.precisionRating = ApplicationManager.instance.GetComponent<GameManager>().precision;
            currentSession.highestComboMultiplier = ApplicationManager.instance.GetComponent<GameManager>().highestComboMultiplier;
            await DataService.UpdateData(currentSession);
        }

        public PlayerData GetPlayer(int playerIndex)
        {
            return this.currentPlayers.FirstOrDefault(x => x.playerIndex == playerIndex);
        }
    }
}

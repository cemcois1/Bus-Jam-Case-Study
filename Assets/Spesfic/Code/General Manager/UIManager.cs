using System;
using _SpesficCode.Timer;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spesfic.Code.UI_Manager
{
    [DefaultExecutionOrder(-50)]
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup outOfTimeCanvasGroup;
        [SerializeField] private CanvasGroup LevelFailedCanvasGroup;
        [SerializeField] private CanvasGroup levelComplatedCanvasGroup;
        [SerializeField] private CanvasGroup levelHolderCanvasGroup;
        [SerializeField] private CanvasGroup tapToStartCanvasGroup;
        [SerializeField] private TextMeshProUGUI levelText;

        private void OnEnable()
        {
            PuzzleGameEvents.LevelFailed += ShowFailedUI;
            PuzzleGameEvents.LevelComplated += ShowLevelComplatedUI;
            PuzzleGameEvents.levelStarted += UpdateLevel;
        }

        private void OnDisable()
        {
            PuzzleGameEvents.LevelFailed -= ShowFailedUI;
            PuzzleGameEvents.LevelComplated -= ShowLevelComplatedUI;
            PuzzleGameEvents.levelStarted -= UpdateLevel;

        }
        public void UpdateLevel()
        {
            levelHolderCanvasGroup.OpenPanel();
            levelText.text ="Level "+ (1+LevelManager.Instance.LevelCount);
            tapToStartCanvasGroup.OpenPanel();
        }
        
        public void ShowLevelComplatedUI()
        {
            levelComplatedCanvasGroup.OpenPanel();
        }
        public void HideLevelComplatedUI()
        {
            levelComplatedCanvasGroup.ClosePanel();
        }

        //used in editor 
        public void HideFailedUIs()
        {
            LevelFailedCanvasGroup.ClosePanel();
            outOfTimeCanvasGroup.ClosePanel();
        }



        private void ShowFailedUI(int index)
        {
            if (index==1)
            {
                LevelFailedCanvasGroup.OpenPanel();
            }
            else
            {
                outOfTimeCanvasGroup.OpenPanel();
            }

        }
    }
}
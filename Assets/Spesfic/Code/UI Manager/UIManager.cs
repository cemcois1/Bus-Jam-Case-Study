using System;
using _SpesficCode.Timer;
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

        private void OnEnable()
        {
            PuzzleGameEvents.LevelFailed += ShowFailedUI;
            PuzzleGameEvents.LevelComplated += ShowLevelComplatedUI;
        }

        private void OnDisable()
        {
            PuzzleGameEvents.LevelFailed -= ShowFailedUI;
            PuzzleGameEvents.LevelComplated -= ShowLevelComplatedUI;
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
using Generic.Code.Timer_System;
using UnityEngine;
using UnityEngine.Serialization;

namespace _SpesficCode.Timer
{
    [DefaultExecutionOrder(-50)]
    public class PuzzleTimer : AdvancedTimer
    {
        public static PuzzleTimer Instance;
        [SerializeField] private bool timerStarted=false;
        
        private void OnEnable()
        {
            Instance = this;
            
            PuzzleGameEvents.levelStarted += ResetValues;
            PuzzleGameEvents.LevelComplated += StopTimer;
            PuzzleGameEvents.LevelFailed += StopTimer;
            
            OnFinished.AddListener(LevelFailed);
        }
        private void OnDisable()
        {
            PuzzleGameEvents.levelStarted -= ResetValues;
            PuzzleGameEvents.LevelComplated -= StopTimer;
            PuzzleGameEvents.LevelFailed -= StopTimer;
            
            OnFinished.RemoveAllListeners();
        }
        private void LevelFailed()
        {
            Debug.Log("LeveL Failed".Red());
            PuzzleGameEvents.LevelFailed?.Invoke(1);
        }
        public void ResetValues()
        {
            ResetTimer();
            UpdateTimertext();
            SetIsWorking(false);
            timerStarted = false;
        }
        private void StopTimer(int i)
        {
            SetIsWorking(false);
        }
        private void StopTimer()
        {
            SetIsWorking(false);
        }
        public void GainTime(float time)
        {
            currentTime += time;
            UpdateTimertext();
        }
    }
}
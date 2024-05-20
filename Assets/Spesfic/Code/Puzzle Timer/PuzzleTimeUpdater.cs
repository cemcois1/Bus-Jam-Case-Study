using System;
using _SpesficCode.Timer;
using UnityEngine;

namespace Spesfic.Code.Puzzle_Timer
{
    public class PuzzleTimeUpdater : MonoBehaviour
    {
        [SerializeField] private int seconds=180;
        
        private void OnEnable()
        {
            var puzzleTimer = PuzzleTimer.Instance;
            puzzleTimer.currentTime = seconds;
            puzzleTimer.time = seconds;
            puzzleTimer.ResetValues();
        }
    }
}
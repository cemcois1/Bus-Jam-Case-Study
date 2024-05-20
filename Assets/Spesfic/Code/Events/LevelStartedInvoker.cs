using System;
using _SpesficCode.Timer;
using UnityEngine;

namespace Spesfic.Code.Events
{
    public class LevelStartedInvoker : MonoBehaviour
    {
        private void OnEnable()
        {
            PuzzleGameEvents.levelStarted.Invoke();
        }
    }
}
using System;
using UnityEngine;

namespace _SpesficCode.Timer
{
    [DefaultExecutionOrder(-150)]
    public class PuzzleGameEvents:MonoBehaviour
    {
        public static Action levelStarted;
        public static Action LevelComplated;
        /// <summary>
        /// 1 is timer fail 0 is  no more space fail
        /// </summary>
        public static Action<int> LevelFailed;
    }
}
using UnityEngine;

namespace Spesfic.Code.Color_Data
{
    [CreateAssetMenu(fileName = "MatchableColorData", menuName = "Color Data/Matchable Color Data")]
    public class MatchableColorData : ScriptableObject
    {
        public Color humanColor;
        public Color BusColor;
    }
}
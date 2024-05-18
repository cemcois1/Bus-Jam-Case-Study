using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Spesfic.Code.Grid_System
{
    public class Tile : MonoBehaviour
    {
        public static int UnkownStepCount = 10000;
        public static int ObstacleStepCount = 100000;
        public bool IsFull => holdingItem != null;
        public bool isObstacle=> stepCount==ObstacleStepCount;
        public bool isUnknownTile => stepCount == UnkownStepCount;
        [SerializeField] private Transform holdingItem;

        public int StepCount => stepCount;
        [SerializeField,ReadOnly]private int stepCount=10000;
        public TextMeshPro text;

        public void SetItem(Transform item)
        {
            holdingItem = item;
        }

        public void RemoveItem()
        {
#if UNITY_EDITOR
            if (holdingItem != null)
            {
                DestroyImmediate(holdingItem.gameObject);
            }
#endif
            holdingItem = null;
        }

        private void OnDrawGizmos()
        {
            var color = isObstacle/*|| IsFull*/ ? Color.red : Color.green;
            color.a= .5f;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, .2f);
        }

        public void UpdateStepCount(int stepCount)
        {
            this.stepCount = stepCount;
            if (stepCount == 10000)
            {
                text.text = "U";

            }
            else if (stepCount==100000)
            {
                text.text = "X";
            }
            else
            {
                text.text = stepCount.ToString();
            }
        }


        [Button]
        public void ObsctacleSetOperation()
        {
            stepCount = isObstacle ? UnkownStepCount : ObstacleStepCount;
        }
    }
}
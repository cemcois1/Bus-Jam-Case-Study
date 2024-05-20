using System;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spesfic.Code.Grid_System
{
    public class Tile : MonoBehaviour
    {
        public static int UnkownStepCount = 10000;
        public static int ObstacleStepCount = 100000;
        public bool IsFull => holdingItem != null;
        public bool isObstacle=> stepCount==ObstacleStepCount;
        public bool isUnknownTile => stepCount == UnkownStepCount;
        [SerializeField] public Transform holdingItem;

        public int StepCount => stepCount;
        [FindInParent]public DefaultItemColorData DefaultColorData;

        [SerializeField,ReadOnly]private int stepCount=10000;
        public TextMeshPro text;
        [SerializeField] private MeshRenderer meshRenderer;

        public Color BaseColor;
            
        private void OnEnable()
        {
            BaseColor= meshRenderer.material.color;
            if (isObstacle)
            {
                meshRenderer.enabled = false;
            }
        }

        public Human GetHoldedHuman()
        {
            if (holdingItem == null) return null;
            return holdingItem.GetComponent<Human>();
        }
        public void SetItem(Transform item)
        {
            holdingItem = item;
        }
        public void SetItem(Human human)
        {
            holdingItem = human.transform;
            human.PlaceToTile(this);
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
            var color = isObstacle/*|| IsFull*/ ? Color.red : Color.black;
            color.a= .5f;

            if (DefaultColorData.matchableColorData != null)
            {
                color = DefaultColorData.matchableColorData.humanColor;

            }

            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, .2f);
        }

        public void UpdateStepCount(int stepCount)
        {
            this.stepCount = stepCount;
            if (isUnknownTile)
            {
                text.text = "U";

            }
            else if (isObstacle)
            {
                text.text = "X";
            }
            else
            {
                text.text = stepCount.ToString();
                UpdateItemClickability();
            }
        }

        private void UpdateItemClickability()
        {
            if (holdingItem == null) return;
            if (!isObstacle&&!isUnknownTile)
            {
                holdingItem.GetComponent<Human>().humanClickArea.MakeClickable();
            }
            else if (isUnknownTile)
            {
                holdingItem.GetComponent<Human>().humanClickArea.MakeUnClickable();
            }
        }


        [Button]
        public void ObsctacleSetOperation()
        {
            stepCount = isObstacle ? UnkownStepCount : ObstacleStepCount;
        }
    }
}
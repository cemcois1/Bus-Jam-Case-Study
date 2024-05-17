using System;
using _GenericPackageStart.Core.CustomAttributes;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spesfic.Code
{
    public class HumanClickArea : MonoBehaviour
    {
        public Action OnHumanClicked;
        [FindInChildren][SerializeField] private Collider collider;
        [SerializeField] private Outline outline;
        
        private void OnMouseDown()
        {
            Debug.Log("HumanTriggerArea OnMouseDown");
            enabled = false;
            collider.enabled = false;
            outline.enabled = false;
        }
        [Button]
        public void MakeClickable()
        {
            outline.enabled = true;
            enabled = true;
            collider.enabled = true;
        }
    }
}
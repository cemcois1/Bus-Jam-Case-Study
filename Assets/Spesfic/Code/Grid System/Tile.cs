using System;
using UnityEngine;

namespace Spesfic.Code.Grid_System
{
    public class Tile : MonoBehaviour
    {
        public bool IsFull => holdingItem != null;
        public bool isObstacle=false;
        [SerializeField] private Transform holdingItem;
        
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
            var color = isObstacle|| IsFull ? Color.red : Color.green;
            color.a= .5f;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, .2f);
        }
    }
}
using System;
using UnityEngine;

namespace Spesfic.Code.Grid_System
{
    public class Tile : MonoBehaviour
    {
        public bool IsFull { get; private set;}
        public bool isObstacle=false;
        [SerializeField] private Transform holdingItem;
        
        public void SetItem(Transform item)
        {
            holdingItem = item;
            IsFull = true;
        }
        public void RemoveItem()
        {
            holdingItem = null;
            IsFull = false;
        }

        private void OnDrawGizmos()
        {
            var color = isObstacle ? Color.red : Color.green;
            color.a= .5f;
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, .2f);
        }
    }
}
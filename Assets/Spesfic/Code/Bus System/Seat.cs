using UnityEngine;

namespace Spesfic.Code.Bus_System
{
    public class Seat : MonoBehaviour
    {
        public bool IsFull;
        [SerializeField] private Transform human;
        
        public void SetColor(Color color)
        {
            human.GetComponent<MeshRenderer>().material.color = color;
        }
    }
}
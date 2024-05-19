using UnityEngine;

namespace Spesfic.Code.Bus_System
{
    public class Seat : MonoBehaviour
    {
        public bool IsFull=> human != null;
        [SerializeField] private Transform human;
        
        public void SetColor(Color color)
        {
            human.GetComponent<MeshRenderer>().material.color = color;
        }

        public void SetHuman(Human human)
        {
            this.human = human.transform;
            human.SitToSeat(this); 
        }
    }
}
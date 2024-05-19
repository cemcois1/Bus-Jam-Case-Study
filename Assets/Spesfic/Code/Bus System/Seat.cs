using UnityEngine;

namespace Spesfic.Code.Bus_System
{
    public class Seat : MonoBehaviour
    {
        public bool IsFull=> human != null;
        public Vector3 SitRotation=new Vector3(0,90,0);

        [SerializeField] private Transform human;
        
        public void SetColor(Color color)
        {
            if (human == null) return;
            human.GetComponent<MeshRenderer>().material.color = color;
        }

        public void SetHuman(Human human)
        {
            this.human = human.transform;
            human.SitToSeat(this); 
        }
    }
}
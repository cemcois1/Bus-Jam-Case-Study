using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spesfic.Code.Bus_System
{
    public class Bus : MonoBehaviour
    {
        public Action OnBussFull;

        [SerializeField] private List<Seat> seats;
        public bool allSeatsFull=> seats.TrueForAll(x => x.IsFull);        
        [SerializeField] private List<MeshRenderer> meshRenderers;
        [SerializeField] private Color busColor=Color.grey;
        
        public void SetColor(Color color)
        {
            meshRenderers.ForEach(x => x.material.color = color);
            busColor = color;
            seats.ForEach(x=>x.SetColor(color));
        }
    }
}
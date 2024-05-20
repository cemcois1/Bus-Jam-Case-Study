using System;
using System.Collections.Generic;
using Spesfic.Code.Color_Data;
using UnityEngine;

namespace Spesfic.Code.Bus_System
{
    public class Bus : MonoBehaviour
    {
        public Action OnBussFull;

        [SerializeField] private List<Seat> seats;
        public bool allSeatsFull=> seats.TrueForAll(x => x.IsFull);        
        [SerializeField] private List<MeshRenderer> meshRenderers;

        public MatchableColorData BusColor => busColor;
        [SerializeField] private MatchableColorData busColor;


        public void SetColor(MatchableColorData color)
        {
            meshRenderers.ForEach(x => x.material.color = color.BusColor);
            busColor = color;
            seats.ForEach(x=>x.SetColor(color));
        }

        public void AddHuman(Human human)
        {
            var emptySeat = seats.Find(x => !x.IsFull);
            if (emptySeat != null)
            {
                emptySeat.SetHuman(human);
                if (allSeatsFull)
                {
                    OnBussFull?.Invoke();
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using DG.Tweening;
using Spesfic.Code.Color_Data;
using UnityEngine;

namespace Spesfic.Code.Bus_System
{
    public class Bus : MonoBehaviour
    {
        public int LoadedTotal = 0;
        public Action OnBussFull;

        [SerializeField] private List<Seat> seats;
        public bool allSeatsFull=> seats.TrueForAll(x => x.IsFull);        
        [SerializeField] private List<MeshRenderer> meshRenderers;

        public MatchableColorData BusColor => busColor;
        [SerializeField] private MatchableColorData busColor;

        [Header( "Visual Polish")]
        [SerializeField] private Transform yPivotTransform;
        private float maxYDistance = .05f;
        private float YAnimDuration=.45f;
        
        private void OnEnable()
        {
            yPivotTransform.DOLocalMoveY(-maxYDistance/2, YAnimDuration).From(maxYDistance / 2).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }

        public void SetColor(MatchableColorData color)
        {
#if UNITY_EDITOR
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial)
                {
                    color = color.BusColor
                };
            }
            meshRenderers.ForEach(x => x.sharedMaterial.color = color.BusColor);
       
#else
            meshRenderers.ForEach(x => x.material.color = color.BusColor);
#endif
            
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
                    Debug.Log("Bus is Full!".Red());
                    OnBussFull?.Invoke();
                }
                else
                {
                    Debug.Log("Bus is not Full!".Green());
                }
            }
        }
    }
}
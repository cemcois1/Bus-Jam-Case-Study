using System;
using System.Collections.Generic;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using DG.Tweening;
using Generic.Code.PoolBase;
using Sirenix.OdinInspector;
using Spesfic.Code.Color_Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spesfic.Code.Bus_System
{
    public class BusQueue : MonoBehaviour
    {
        public Action AllBussesFinished;
        public static Action<Bus> NewBusArrived;
        
        [SerializeField] private List<Bus> buses;
        public Bus ActiveBus => (buses != null && buses.Count > 0) ? buses[0] : null;
        

        [CreateInChild] public Transform busLoadablePoint;
        [CreateInChild] [SerializeField] private Transform busFinishPoint;
        public Transform busGatePoint;
        [SerializeField] private float xDistanceBetweenBusses=1f;


        [CreateInChild][SerializeField] private PoolBase BusPool;


        [Header("Polish")]
        [SerializeField] float busMovementTime = 1f;

        [SerializeField] private AnimationCurve busMovementCurve;

        private void OnEnable()
        {

            /*var sequence = DOTween.Sequence();
            sequence.AppendInterval(5);

            sequence.Append(ShiftBussesAnimation(new List<Bus>(buses)));
            ShiftBussesLogic();

            sequence.AppendInterval(5);
            for (int i = 0; i < 4; i++)
            {
                sequence.Append(ShiftBussesAnimation(new List<Bus>(buses)));
                ShiftBussesLogic();
            }*/


        }

        [Button]
        public void CreateBusList(IList<MatchableColorData> colors)
        {
            //her renk için bir bus çek pooldan ve listeye ekle
            for (int i = 0; i < colors.Count; i++)
            {
                GameObject busObj = BusPool.GetGameobjectFromPool();
                busObj.transform.SetParent(transform);
                busObj.transform.SetPositionAndRotation(busLoadablePoint.position + new Vector3(i * xDistanceBetweenBusses, 0, 0), busLoadablePoint.rotation);
                busObj.gameObject.SetActive(true);
                var bus= busObj.GetComponent<Bus>();
                bus.OnBussFull += () =>
                {
                    ShiftBussesAnimation(buses);
                    ShiftBussesLogic();
                };
                bus.SetColor(colors[i]);
                buses.Add(bus);

            }
        }
        
        public void ShiftBussesLogic()
        {
            //listenin ilk elemanını sil
            buses.RemoveAt(0);
            if (buses.Count == 0)
            {
                Debug.Log("All busses finished".Red());
                AllBussesFinished?.Invoke();
            }
        }
        [Button]
        public Sequence ShiftBussesAnimation(List<Bus> shiftableBuses)
        {
            var busMovementSequence = DOTween.Sequence();
            busMovementSequence.AppendInterval(.3f);
            //busses listesindeki tüm busları hedef noktaya doğru hareket ettir
            for (int i = 0; i < shiftableBuses.Count; i++)
            {
                if (i==0)//finish noktasına git
                {
                    busMovementSequence.Append(shiftableBuses[i].transform.DOMove(busFinishPoint.position, busMovementTime).SetEase(busMovementCurve));
                    continue;                    
                }
                busMovementSequence.Join(shiftableBuses[i].transform
                    .DOMove(busLoadablePoint.position + new Vector3((i - 1) * xDistanceBetweenBusses, 0, 0), busMovementTime)
                    .SetEase(busMovementCurve));
            }

            busMovementSequence.OnComplete(() =>
            {
                if (shiftableBuses.Count > 0)
                {
                    NewBusArrived?.Invoke(shiftableBuses[0]);
                }
            });
            return busMovementSequence;
        }
        

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using UnityEngine;
using UnityEngine.Serialization;

public class BusQueue : MonoBehaviour
{
    public Action AllBussesFinished;
    
    [SerializeField] private List<Bus> buses;

    [CreateInChild][SerializeField] private Transform busLoadablePoint;
    [CreateInChild] [SerializeField] private Transform busFinishPoint;
    [SerializeField] private float xDistanceBetweenBusses=1f;
    
    

}
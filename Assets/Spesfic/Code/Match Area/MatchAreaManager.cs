using System;
using System.Collections.Generic;
using System.Linq;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using Sirenix.OdinInspector;
using Spesfic.Code.Bus_System;
using Spesfic.Code.Grid_System;
using UnityEngine;

namespace Spesfic.Code.MatchArea
{
    //sadece boş olan tilelere insanları yerleştireceğiz tamamen dolu olduğunda match alanı oluşturulacak
    public class MatchAreaManager:Singleton<MatchAreaManager>
    {
        [SerializeField] private List<Tile> tiles = new();
        [FindInParent][SerializeField] private BusQueue busQueue;
        
        public bool IsFull => tiles.All(tile => tile.IsFull);

        private void OnEnable()
        {
            BusQueue.NewBusArrived += CheckMatchArea;
        }

        private void OnDisable()
        {
            BusQueue.NewBusArrived -= CheckMatchArea;
        }

        private void CheckMatchArea(Bus bus)
        {
            foreach (var tile in tiles)
            {
                if (tile.IsFull)
                {
                    var human = tile.GetHoldedHuman();
                    var isLoadable = human.Color == bus.BusColor;
                    if (isLoadable)
                    {
                        human.MoveToBus(busQueue.busGatePoint.position, busQueue.ActiveBus);
                        tile.SetItem((Transform) null);
                    }
                }
            }
        }


        public Tile GetEmptyTile()
        {
            return tiles.Find(tile => !tile.IsFull);
        }
        
        [Button]
        public void GetTiles()
        {
            tiles = transform.GetComponentsInChildren<Tile>().OrderBy(tile => tile.transform.position.x).ToList();
        }
        

    }
}
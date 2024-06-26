using System;
using System.Collections.Generic;
using System.Linq;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using _SpesficCode.Timer;
using Sirenix.OdinInspector;
using Spesfic.Code.Bus_System;
using Spesfic.Code.Grid_System;
using UnityEngine;

namespace Spesfic.Code.MatchArea
{
    //sadece boş olan tilelere insanları yerleştireceğiz tamamen dolu olduğunda match alanı oluşturulacak
    public class MatchAreaManager: MonoBehaviour
    {
        public static MatchAreaManager Instance;
        [SerializeField] private List<Tile> tiles = new();
        [FindInParent][SerializeField] private BusQueue busQueue;
        
        public bool IsFull => tiles.All(tile => tile.IsFull);
        public bool IsLastEmptyTile => tiles.Count(tile => !tile.IsFull) == 1;

        private void OnEnable()
        {
            BusQueue.NewBusArrived += CheckMatchArea;
            Instance = this;
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
            //eğer bütün tilelar dolu ise Game over
            if (IsFull)
            {

                PuzzleGameEvents.LevelFailed.Invoke(0);


                Debug.Log("Game Over");
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


        public bool FindTile(Tile holdedTile)
        {
            return tiles.Contains(holdedTile);
            
        }
    }
}
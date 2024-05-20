using System;
using System.Collections.Generic;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using _SpesficCode.Timer;
using DG.Tweening;
using Generic.Code.PoolBase;
using Spesfic.Code.Bus_System;
using Spesfic.Code.Color_Data;
using Spesfic.Code.MatchArea;
using UnityEngine;

namespace Spesfic.Code.Grid_System
{
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField,FindInParent] private GridManager gridManager;
        [SerializeField,FindInParent] private MatchAreaManager matchAreaManager;
        [SerializeField,FindInParent] private BusQueue busQueue;
        
        [SerializeField,FindInParent] private PoolBase humanBase;
        [SerializeField] private List<MatchableColorData> colors;
        [SerializeField] private Vector3 additionalPlacementOffset;
        
        private void OnEnable()
        {
            PuzzleGameEvents.LevelFailed +=StopLevel;
            CreateLevel();
        }

        private void StopLevel(int obj)
        {
            
        }

        private void CreateLevel()
        {
            if (CheckGridPlaceability(out var placeableTiles)) return;
            //bütün renklerden 3 tane insan oluştur ve random bir şekilde yerleştir
            foreach (var colorData in colors)
            {
                for (int i = 0; i < 3; i++)
                {
                    CreateColoredHumans(placeableTiles, colorData);
                }
            }

            busQueue.CreateBusList(colors);

            
            //placeableTiles içerisine random bir şekilde insanları yerleştir
        }

        private void CreateColoredHumans(List<Tile> placeableTiles, MatchableColorData colorData)
        {
            var humanObj = humanBase.GetGameobjectFromPool();
            var humanComponent = humanObj.GetComponent<Human>();

            var randomValue = UnityEngine.Random.Range(0, placeableTiles.Count);
            humanObj.transform.position = placeableTiles[randomValue]
                .transform.position + additionalPlacementOffset;
            placeableTiles[randomValue].SetItem(humanComponent);
            placeableTiles.Remove(placeableTiles[randomValue]);
            humanObj.SetActive(true);
            humanComponent.SetColor(colorData);
            humanComponent.humanClickArea.OnHumanClicked += human =>
            {
                PuzzleTimer.Instance.SetIsWorking(true);
                bool SitableOnBus = busQueue.ActiveBus.BusColor == human.Color &&
                                    !busQueue.ActiveBus.allSeatsFull;
                if (!SitableOnBus)
                {
                    var emptyTile = matchAreaManager.GetEmptyTile();
                    emptyTile.SetItem(human);
                }
            };
            humanComponent.humanClickArea.HumanExitedGrid += human =>
            {
                //mümkünse bus queue'ya ekle değilse matcing area'ya ekle
                bool SitableOnBus = busQueue.ActiveBus.BusColor == human.Color && !busQueue.ActiveBus.allSeatsFull;
                if (SitableOnBus)
                {
                    Debug.Log("Add Human to bus".Blue());
                    var loadablePosition = busQueue.busGatePoint.position + additionalPlacementOffset;
                    human.MoveToBus(loadablePosition, busQueue.ActiveBus);
                }
                else
                {
                    var position = human.humanClickArea.holdedTile.transform.position +
                                   additionalPlacementOffset;
                    human.MoveToTile(position);
                }
            };
        }

        private bool CheckGridPlaceability(out List<Tile> placeableTiles)
        {
            gridManager.ClearAllPlacedItems();
            placeableTiles = gridManager.GetPlaceableTiles();
            if (placeableTiles.Count < colors.Count * 3)
            {
                Debug.LogError("Not enough placeable tiles for humans");
                return true;
            }

            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
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
            CreateLevel();
        }

        private void CreateLevel()
        {
            gridManager.ClearAllPlacedItems();
            var placeableTiles = gridManager.GetPlaceableTiles();
            if (placeableTiles.Count<colors.Count*3)
            {
                Debug.LogError("Not enough placeable tiles for humans");
                return;
            }
            //bütün renklerden 3 tane insan oluştur ve random bir şekilde yerleştir
            foreach (var colorData in colors)
            {
                for (int i = 0; i < 3; i++)
                {
                    var humanObj = humanBase.GetGameobjectFromPool();
                    var humanComponent = humanObj.GetComponent<Human>();

                    var randomValue = UnityEngine.Random.Range(0, placeableTiles.Count);
                    humanObj.transform.position = placeableTiles[randomValue]
                        .transform.position+ additionalPlacementOffset;
                    placeableTiles[randomValue].SetItem(humanComponent);
                    placeableTiles.Remove(placeableTiles[randomValue]);
                    humanObj.SetActive(true);
                    humanComponent.SetColor(colorData);
                    humanComponent.humanClickArea.OnHumanClicked += human =>
                    {
                        bool SitableOnBus = busQueue.ActiveBus.BusColor == human.Color&& 
                                            !busQueue.ActiveBus.allSeatsFull;
                        if (!SitableOnBus)
                        {
                            var emptyTile = matchAreaManager.GetEmptyTile();
                            emptyTile.SetItem(human);
                        }
                        Debug.Log("TODO Fill tiles before Human Go".Red());
                    };
                    humanComponent.humanClickArea.HumanExitedGrid += human =>
                    {
                        Debug.Log("Clicked".Red());

                        //mümkünse bus queue'ya ekle değilse matcing area'ya ekle
                        Debug.Log("TODO General Color Data Oluştur".Red());
                        bool SitableOnBus = busQueue.ActiveBus.BusColor == human.Color&&!busQueue.ActiveBus.allSeatsFull;
                        if (SitableOnBus)
                        {
                            Debug.Log("Add Human to bus".Blue());
                            var loadablePosition = busQueue.busGatePoint.position+ additionalPlacementOffset;
                            human.MoveToBus(loadablePosition ,busQueue.ActiveBus);
                            
                            
                        }
                        else
                        {
                            
                            var position = human.humanClickArea.holdedTile.transform.position+
                                           additionalPlacementOffset;
                            human.transform.DOLookAt(position, .05f);
                            human.transform.DOMove(position    ,
                                    Vector3.Distance(position,human.transform.position)/human.walkSpeed)
                                .OnComplete(() =>
                                {
                                    human.IdleAnim();
                                    human.transform.DOLookAt(new Vector3(0,0, 100000f), .05f);

                                });
                            
                        }
                    };
                }
            }

            
            //placeableTiles içerisine random bir şekilde insanları yerleştir
        }
    }
}
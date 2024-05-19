using System;
using System.Collections.Generic;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using DG.Tweening;
using Generic.Code.PoolBase;
using Spesfic.Code.Bus_System;
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
        [SerializeField] private List<Color> colors;
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
            foreach (var humanColor in colors)
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
                    humanComponent.SetColor(humanColor);
                    humanComponent.humanClickArea.OnHumanClicked += human =>
                    {
                        Debug.Log("TODO Fill tiles before Human Go".Red());
                    };
                    humanComponent.humanClickArea.HumanExitedGrid += human =>
                    {
                        Debug.Log("Clicked".Red());

                        //mümkünse bus queue'ya ekle değilse matcing area'ya ekle
                        var walkBaseTime = .25f;
                        if (busQueue.ActiveBus.BusColor == human.Color&&!busQueue.ActiveBus.allSeatsFull)
                        {
                            Debug.Log("Add Human to bus".Blue());
                            var loadablePosition = busQueue.busLoadablePoint.position;
                            human.transform.DOLookAt(loadablePosition, .05f);
                            human.transform.DOMove(loadablePosition,
                                Vector3.Distance(loadablePosition,human.transform.position) /human.walkSpeed).OnComplete(
                                    ()=>busQueue.ActiveBus.AddHuman(human)
                                );
                        }
                        else
                        {
                            var emptyTile = matchAreaManager.GetEmptyTile();
                            emptyTile.SetItem(human);
                            var position = emptyTile.transform.position;
                            human.transform.DOLookAt(position, .05f);
                            human.transform.DOMove(position    ,
                                Vector3.Distance(position,human.transform.position)/human.walkSpeed)
                                .OnComplete(human.IdleAnim);
                            
                        }
                    };
                }
            }

            
            //placeableTiles içerisine random bir şekilde insanları yerleştir
        }
    }
}
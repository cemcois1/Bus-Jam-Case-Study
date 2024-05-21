using System;
using System.Collections.Generic;
using System.Linq;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using _SpesficCode.Timer;
using DG.Tweening;
using Generic.Code.PoolBase;
using Sirenix.OdinInspector;
using Spesfic.Code.Bus_System;
using Spesfic.Code.Color_Data;
using Spesfic.Code.MatchArea;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

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

        [SerializeField] private int seed;
        private void OnEnable()
        {
            CreateLevel();
        }
        

        private void CreateLevel()
        {
            if (CheckGridPlaceability(out var placeableTiles)) return;
            //bütün renklerden 3 tane insan oluştur ve random bir şekilde yerleştir
            foreach (var placeableTile in placeableTiles)
            {
                if (placeableTile.DefaultColorData.matchableColorData!=null)
                {
                    CreateColoredHuman(placeableTile);
                }
                else
                {
                    Debug.Log("tile Color is null".Red());
                }
            }
            busQueue.CreateBusList(colors);

            
            //placeableTiles içerisine random bir şekilde insanları yerleştir
        }

        private void CreateColoredHuman(Tile placeableTile)
        {
            var humanObj = humanBase.GetGameobjectFromPool();
            var humanComponent = humanObj.GetComponent<Human>();
            humanObj.transform.position = placeableTile.transform.position + additionalPlacementOffset;
            placeableTile.SetItem(humanComponent);
            humanObj.SetActive(true);
            humanComponent.SetColor(placeableTile.DefaultColorData.matchableColorData);
            BindClickedEvents(humanComponent);
        }


        private void BindClickedEvents(Human humanComponent)
        {
            humanComponent.humanClickArea.OnHumanClicked += human =>
            {
                PuzzleTimer.Instance.SetIsWorking(true);
                bool SitableOnBus = busQueue.ActiveBus.BusColor == human.Color &&
                                    !busQueue.ActiveBus.allSeatsFull&&busQueue.ActiveBus.LoadingStatedTotal<=2;
                if (!SitableOnBus)
                {
                    Debug.Log("Load to Match area".Blue());
                    var emptyTile = matchAreaManager.GetEmptyTile();
                    emptyTile.SetItem(human);
                }

                if (SitableOnBus)
                {
                    Debug.Log("Load to bus area".Blue());

                    busQueue.ActiveBus.LoadingStatedTotal += 1;
                    if (busQueue.ActiveBus.LoadingStatedTotal<=3)
                    {

                        human.canGoBus = true;
                    }
                }
            };
            humanComponent.humanClickArea.HumanExitedGrid += human =>
            {
                //mümkünse bus queue'ya ekle değilse matcing area'ya ekle
                Debug.Log(busQueue.ActiveBus.LoadingStatedTotal);
                bool SitableOnBus = (busQueue.ActiveBus.BusColor == human.Color) && !busQueue.ActiveBus.allSeatsFull && human.canGoBus;
                if (SitableOnBus)
                {
                    var loadablePosition = busQueue.busGatePoint.position + additionalPlacementOffset;
                    human.MoveToBus(loadablePosition, busQueue.ActiveBus);
                }
                else
                {
                    Debug.Log("Add Human to Match".Red());

                    //eğer holdedtile matchareaya ait değilse matchareadan boş bir alan bul ekle
                    if (matchAreaManager.FindTile(human.humanClickArea.holdedTile))
                    {
                        Debug.Log("Human Tile Changed".Red());
                        var emptyTile = matchAreaManager.GetEmptyTile();
                        emptyTile.SetItem(human);
                        human.MoveToTile(emptyTile.transform.position+additionalPlacementOffset);
                    }
                    else
                    {
                        var position = human.humanClickArea.holdedTile.transform.position +
                                       additionalPlacementOffset;
                        human.MoveToTile(position);
                    }
                    

                }
            };
        }

        /*private void CreateColoredHumans(List<Tile> placeableTiles, MatchableColorData colorData)
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
            BindClickedEvents(humanComponent);
        }*/

        private bool CheckGridPlaceability(out List<Tile> placeableTiles)
        {
            placeableTiles = gridManager.GetPlaceableTiles();
            if (placeableTiles.Count < colors.Count * 3)
            {
                Debug.LogError("Not enough placeable tiles for humans");
                return true;
            }

            return false;
        }
        
        
        [Button]
        public void InitializeSeed()
        {
            Random.InitState(seed);
        }
        [Button]
        public void CreateLevelInEditor()
        {
            Random.InitState(seed);
            ClearTiles();
            ThrowColorsToTiles();
        }

        [Button]
        public void CreateRandomLevelInEditor()
        {
            seed = DateTime.Now.Millisecond;
            Random.InitState(seed);
            ClearTiles();
            ThrowColorsToTiles();
        }

        private void ThrowColorsToTiles()
        {
            //listedeki her renk için 3 tane
            var tilesRandomlyListed = gridManager.GetPlaceableTiles().OrderBy(tile => Random.Range(0,100)).ToList();
            for (var index = 0; index < colors.Count; index++)
            {
                var color = colors[index];
                for (int i = 0; i < 3; i++)
                {
                    var random = tilesRandomlyListed.Random();
                    random.DefaultColorData.matchableColorData = color;


#if UNITY_EDITOR
                    EditorUtility.SetDirty(random.DefaultColorData);
                    EditorUtility.SetDirty(random.DefaultColorData.matchableColorData);
#endif
                    tilesRandomlyListed.Remove(random);
                }
            }
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }

        [Button]
        private void ClearTiles()
        {
            foreach (var tile in gridManager.GetPlaceableTiles())
            {
                if (tile.holdingItem != null)
                {
                    DestroyImmediate(tile.holdingItem);
                }
                
                
                tile.DefaultColorData.matchableColorData = null;
                tile.SetItem((Transform) null);
            }
        }
    }
}
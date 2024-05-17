using System;
using System.Collections.Generic;
using _GenericPackageStart.Code._Mechanic.CustomAttributes.FinInParentAttribute;
using Generic.Code.PoolBase;
using UnityEngine;

namespace Spesfic.Code.Grid_System
{
    public class LevelCreator : MonoBehaviour
    {
        [SerializeField,FindInParent] private GridManager gridManager;
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
                    var randomValue = UnityEngine.Random.Range(0, placeableTiles.Count);
                    humanObj.transform.position = placeableTiles[randomValue]
                        .transform.position+ additionalPlacementOffset;
                    placeableTiles.Remove(placeableTiles[randomValue]);
                    humanObj.SetActive(true);
                    humanObj.GetComponent<Human>().SetColor(humanColor);
                }
            }

            
            //placeableTiles içerisine random bir şekilde insanları yerleştir
        }
    }
}
using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spesfic.Code.Grid_System
{
    public class GridManager : MonoBehaviour
    {
        public List<Tile> tiles;
        public int rowCount=5;



        [Button]
        protected void CheckAllTileOrders()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(1f);
            for (int i = 0; i < tiles.Count; i++)
            {
                //bütün tileları 1er 1 er scale et 
                var i1 = i;
                sequence.AppendCallback(() =>
                {
                    GetTile(i1 / rowCount, i1 % rowCount).transform.DOPunchScale(transform.localScale * .15f, .25f, 3, .5f);
                });
                sequence.AppendInterval(.3f);
            }
        }


        //sıralı olan listeyi  2 boyutlu diziymiş gibi kullanmak için
        public Tile GetTile(int rowindex, int coloumnIndex)
        {
            return tiles[rowindex * rowCount + coloumnIndex];
        }

        public List<Tile> GetPlaceableTiles()
        {
            return tiles.FindAll(tile => !tile.isObstacle);
        }
        


        #region Editor Methods

        [Button]
        public void GetTiles()
        {
            tiles = new List<Tile>(GetComponentsInChildren<Tile>());
        }
        [Button]
        public void SortTiles()
        {
            //pozisyonlarına bak eğer z eşitse x'e göre sırala
            tiles.Sort((a, b) =>
            {
                if (a.transform.position.z == b.transform.position.z)
                {
                    return a.transform.position.x.CompareTo(b.transform.position.x);
                }
                return -a.transform.position.z.CompareTo(b.transform.position.z);
            });
        }

        #endregion

        public void ClearAllPlacedItems()
        {
            foreach (var tile in tiles)
            {
                tile.RemoveItem();
            }
        }
    }
}
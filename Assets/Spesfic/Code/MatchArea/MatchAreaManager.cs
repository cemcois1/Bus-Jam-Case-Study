using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Spesfic.Code.Grid_System;
using UnityEngine;

namespace Spesfic.Code.MatchArea
{
    //sadece boş olan tilelere insanları yerleştireceğiz tamamen dolu olduğunda match alanı oluşturulacak
    public class MatchAreaManager:Singleton<MatchAreaManager>
    {
        [SerializeField] private List<Tile> tiles = new();
        
        public bool IsFull => tiles.All(tile => tile.IsFull);
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
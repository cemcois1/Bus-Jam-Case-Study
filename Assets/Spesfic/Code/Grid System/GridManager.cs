using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Spesfic.Code.Grid_System
{
    public class GridManager : Singleton<GridManager>
    {
        public List<Tile> tiles;
        public int rowCount=5;


        private void Start()
        {
            StartCoroutine(CalculateAllStepCounts());

        }
            
        public IEnumerator CalculateAllStepCounts()
        {
            var unknownTileList = new List<Tile>();
            //rowcounta göre tileları  küçük listelere ayır her row için bir liste
            for (int i = 0; i < tiles.Count - 1; i += rowCount)
            {
                //Debug.Log("Rowcount is " + rowCount + " i is " + i);
                var row = tiles.GetRange(i, rowCount);
                for (var j = 0; j < row.Count; j++)
                {
                    var TileIndex = i + j;
                    if (i == 0) //ilk sıra tileların count değerini 1 yap eğer obstacle değilse
                    {
                        if (!tiles[TileIndex].isObstacle)
                        {
                            tiles[TileIndex].UpdateStepCount(1);
                        }
                        else
                        {
                            tiles[TileIndex].UpdateStepCount(Tile.ObstacleStepCount);
                        }
                    }
                    else
                    {
                        if (!tiles[TileIndex].isObstacle)
                        {
                            var neighbours = GetNeighbours(tiles[TileIndex]);
                            //en yakın komşuların stepcountlarını al ve en küçüğüne 1 ekle
                            var validatedNeighbors = neighbours.FindAll(tile => !tile.isObstacle && !tile.isUnknownTile&&!tile.IsFull);
                            if (validatedNeighbors.Count == 0)
                            {
                                unknownTileList.Add(tiles[TileIndex]);
                            }
                            else
                            {
                                var minimumNeighborStepCount = validatedNeighbors
                                    .Min(tile => tile.StepCount);
                                // Önceki adımın tamamlanmasını beklemek için bir gecikme ekleyin
                                if (minimumNeighborStepCount>1000)
                                {
                                    Debug.Log("Catch");
                                    
                                }
                                tiles[TileIndex].UpdateStepCount(minimumNeighborStepCount + 1);
                                // Aralarında 0.5 saniyelik bir gecikme ekleyin
                            }
                        }
                        else
                        {
                            tiles[TileIndex].UpdateStepCount(Tile.ObstacleStepCount);
                        }
                    }

                    yield return null;

                }

                yield return null;
                yield return StartCoroutine(CalculateUnknownStepCounts(unknownTileList));
            }
//            Debug.Log("Unknown tile count is " + unknownTileList.Count);
        }

        private IEnumerator CalculateUnknownStepCounts(List<Tile> unknownTileList)
        {
            //her tile 'ın komşularına unknown veya obstacle olmayan varsa o objeyi update et
            //eğer her turda en az 1 obje update etmezsen döngüyü kır
            while (unknownTileList.Count>0)
            {
                var UpdateableTile = unknownTileList
                    .FirstOrDefault(tile => GetNeighbours(tile).Any(tile1 => !tile1.isObstacle && !tile1.isUnknownTile&& !tile1.IsFull));
                if (UpdateableTile!=null)
                {
                    var neighbours = GetNeighbours(UpdateableTile);
                    var validatedNeighbors = neighbours.FindAll(tile => !tile.isObstacle && !tile.isUnknownTile&&!tile.IsFull);
                    var minimumNeighborStepCount = validatedNeighbors
                        .Min(tile => tile.StepCount);
                    UpdateableTile.UpdateStepCount(minimumNeighborStepCount + 1);
                    unknownTileList.Remove(UpdateableTile);
                }
                else
                {
                    break;
                }
                yield return null;
            }
            
        }


        public List<Tile> GetNeighbours(Tile tile)
        {
            List<Tile> neighbours = new List<Tile>();
            int index = tiles.IndexOf(tile);
            //sağa bak
            if (index % rowCount != rowCount - 1)
            {
                neighbours.Add(tiles[index + 1]);
            }
            //sola bak
            if (index % rowCount != 0)
            {
                neighbours.Add(tiles[index - 1]);
            }
            //yukarı bak
            if (index / rowCount != 0)
            {
                neighbours.Add(tiles[index - rowCount]);
            }
            //aşağı bak
            if (index / rowCount != tiles.Count / rowCount - 1)
            {
                neighbours.Add(tiles[index + rowCount]);
            }
            return neighbours;
        } 

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
        

        public void ClearAllPlacedItems()
        {
            foreach (var tile in tiles)
            {
                tile.RemoveItem();
            }
        }

        public async Task<List<Vector3>> DrawPathAsync(Tile holdedTile)
        {
            List<Vector3> positions = new List<Vector3>();
            positions.Add(holdedTile.transform.position);
            var currentTile = holdedTile;

            while (currentTile.StepCount != 1)
            {
                var neighbours = GetNeighbours(currentTile);
                var minStepCount = currentTile.StepCount;
                Tile nextTile = null;

                foreach (var neighbour in neighbours)
                {
                    if (!neighbour.IsFull && neighbour.StepCount < minStepCount)
                    {
                        minStepCount = neighbour.StepCount;
                        nextTile = neighbour;
                    }
                }

                if (nextTile != null)
                {
                    positions.Add(nextTile.transform.position);
                    currentTile = nextTile;
                }
                else
                {
                    break;
                }

                // Coroutine adımı tamamlandıktan sonra bir çerçeve bekleyin
                await Task.Yield();
            }

            return positions;
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

    }
    
}
using System;
using _GenericPackageStart.Core.CustomAttributes;
using _SpesficCode.Timer;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spesfic.Code.Bus_System;
using Spesfic.Code.Color_Data;
using Spesfic.Code.Grid_System;
using Spesfic.Code.MatchArea;
using UnityEngine;

namespace Spesfic.Code
{
    /// <summary>
    /// seatlere oturan da bu human olacak
    /// </summary>
    public class Human : MonoBehaviour
    {
        [FindInChildren][SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private ParticleSystem SeatLoadParticle;
        
        public HumanClickArea humanClickArea;
        public MatchableColorData Color => color;
        private MatchableColorData color;
        public float walkSpeed=1f;

        public void SetColor(MatchableColorData color)
        {
            this.color = color;
#if UNITY_EDITOR
            //yeni material oluşturup atamak gerekiyor yoksa diğer humanlar da etkileniyor
            skinnedMeshRenderer.sharedMaterial = new Material(skinnedMeshRenderer.sharedMaterial)
            {
                color = color.humanColor
            };
#else
                        skinnedMeshRenderer.material.color = color.humanColor;

#endif
        }
        public void PlaceToTile(Tile tile)
        {
            humanClickArea.holdedTile = tile;
        }


        public void SitToSeat(Seat seat)
        {
            transform.SetParent(seat.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(seat.SitRotation);
            humanClickArea.animator.SetTrigger("Sit");
            SeatLoadParticle.Play();
            //scale et 0 dan
            //transform.DOScale(transform.localScale,.5f).From(Vector3.zero).SetEase(Ease.OutBounce);
        }

        public TweenerCore<Vector3, Vector3, VectorOptions> MoveToBus(Vector3 loadablePosition, Bus ActiveBus)
        {
            humanClickArea.animator.SetTrigger("Run");
            ActiveBus.LoadedTotal += 1;
            transform.DOLookAt(loadablePosition, .05f);
            return transform.DOMove(loadablePosition,
                Vector3.Distance(loadablePosition, transform.position) / walkSpeed).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    ActiveBus.AddHuman(this);
                    Debug.Log("Human added to bus!");
                });
        }
        public void IdleAnim()
        {
            humanClickArea.animator.SetTrigger("Idle");
        }


        public void MoveToTile(Vector3 position)
        {
            transform.DOLookAt(position, .05f);
            var isFailed = MatchAreaManager.Instance.IsFull && BusQueue.Instance.ActiveBus.LoadedTotal != 3;
            transform.DOMove(position,
                    Vector3.Distance(position, transform.position) / walkSpeed)
                .OnComplete(() =>
                {
                    IdleAnim();
                    transform.DOLookAt(new Vector3(0, 0, 100000f), .05f);
                    if (MatchAreaManager.Instance.IsFull)
                    {
                        PuzzleGameEvents.LevelFailed.Invoke(0);
                        if (BusQueue.Instance.ActiveBus.allSeatsFull)
                        {
                            Debug.Log("Match area is full but no bus is available".Red());
                            if (isFailed)
                            {
                                PuzzleGameEvents.LevelFailed.Invoke(0);
                            }
                        }

                        return;
                    }
                });
        }
    }
}
using System;
using _GenericPackageStart.Core.CustomAttributes;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spesfic.Code.Bus_System;
using Spesfic.Code.Color_Data;
using Spesfic.Code.Grid_System;
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
            skinnedMeshRenderer.material.color = color.humanColor;
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
            transform.DOMove(position,
                    Vector3.Distance(position, transform.position) / walkSpeed)
                .OnComplete(() =>
                {
                    IdleAnim();
                    transform.DOLookAt(new Vector3(0, 0, 100000f), .05f);
                });
        }
    }
}
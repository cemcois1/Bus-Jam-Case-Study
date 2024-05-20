using System;
using _GenericPackageStart.Core.CustomAttributes;
using DG.Tweening;
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
            //scale et 0 dan
            transform.DOScale(transform.localScale,.5f).From(Vector3.zero).SetEase(Ease.OutBounce);
        }

        public void IdleAnim()
        {
            humanClickArea.animator.SetTrigger("Idle");
        }
    }
}
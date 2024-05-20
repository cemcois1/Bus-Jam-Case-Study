using System;
using _GenericPackageStart.Core.CustomAttributes;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spesfic.Code.Grid_System;
using Spesfic.Code.MatchArea;
using UnityEngine;

namespace Spesfic.Code
{
    public class HumanClickArea : MonoBehaviour
    {
        public Action<Human> OnHumanClicked;
        public Action<Human> HumanExitedGrid;
        [FindInChildren][SerializeField] private Collider collider;
        [SerializeField] private Outline outline;
        [SerializeField] private Human human;
        
        
        public Animator animator;
        
        public Tile holdedTile;

        private async void OnMouseDown()
        {
            if (!holdedTile.isUnknownTile&&!MatchAreaManager.Instance.IsFull)
            {
                var positions = await GridManager.Instance.DrawPathAsync(holdedTile);
                holdedTile.SetItem((Transform)null);
                
                StartCoroutine(GridManager.Instance.CalculateAllStepCounts());

//                Debug.Log("Path drawned with " + positions.Count + " points");
                if (positions.Count > 0) // line'ı editörde çiz ve sequence oluştur
                {
                    Sequence moveSequence = DOTween.Sequence();
                    moveSequence.PrependCallback(()=>
                    {
                        animator.SetTrigger("Run");
                        MakeUnClickable();
                    });
                    for (int i = 1; i < positions.Count; i++)
                    {
                        // İki nokta arasında yeşil bir çizgi çiz
                        Debug.DrawLine(positions[i - 1], positions[i], Color.green, 10);

                        // Sequence'e pozisyon ekle
                        moveSequence.Append(human.transform.DOMove(positions[i], .25f).SetEase(Ease.Linear)); // 1 saniyede hareket
                        moveSequence.Join(human.transform.DOLookAt(positions[i], .05f).SetEase(Ease.Linear)); // 1 saniyede hareket
                    }

                    OnHumanClicked?.Invoke(human);
                    moveSequence.OnComplete(()=>HumanExitedGrid?.Invoke(human));
                }
                else
                {
                    OnHumanClicked?.Invoke(human);
                    HumanExitedGrid?.Invoke(human);
                    MakeUnClickable();
                }
            }
            else
            {
                Debug.Log("Unknown tile clicked");
                holdedTile.transform.DOKill(true);
                var meshRenderer = holdedTile.GetComponent<MeshRenderer>();
                meshRenderer.DOKill(true);
                meshRenderer.material.DOColor(Color.red, .5f).From();
                holdedTile.transform.DOShakeRotation(.5f, 10, 10);
            }
        }

        [Button]
        public void MakeUnClickable()
        {
            enabled = false;
            collider.enabled = false;
            outline.OutlineWidth = 1f;
        }

        [Button]
        public void MakeClickable()
        {
            outline.enabled = true;
            enabled = true;
            collider.enabled = true;
        }
    }
}
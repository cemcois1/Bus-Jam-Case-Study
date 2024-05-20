using UnityEngine;

namespace Spesfic.Code.Tutorials
{
    public class TapToPlayTutorial : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        
        
        public void ShowTutorial()
        {
            canvasGroup.OpenPanel();
        }
        //used in editor
        public void HideTutorial()
        {
            canvasGroup.ClosePanel();
        }
        
    }
}
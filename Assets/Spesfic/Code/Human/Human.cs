using _GenericPackageStart.Core.CustomAttributes;
using UnityEngine;

namespace Spesfic.Code
{
    /// <summary>
    /// seatlere oturan da bu human olacak
    /// </summary>
    public class Human : MonoBehaviour
    {
        [FindInChildren][SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private HumanClickArea humanClickArea;
        
        public void SetColor(Color color)
        {
            skinnedMeshRenderer.material.color = color;
        }

        
    }
}
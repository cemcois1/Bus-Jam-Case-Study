using System.Collections.Generic;
using UnityEngine;

namespace Spesfic.Code.Bus_System
{
    public class Bus : MonoBehaviour
    {
        [SerializeField] private List<MeshRenderer> meshRenderers;
                
        public void SetColor(Color color)
        {
            meshRenderers.ForEach(x => x.material.color = color);
        }
    }
}

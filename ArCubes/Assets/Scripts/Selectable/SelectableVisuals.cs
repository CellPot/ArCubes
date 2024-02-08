using System;
using UnityEngine;

namespace Selectable
{
    public class SelectableVisuals: MonoBehaviour
    {
        [SerializeField] private Renderer renderer;
        [SerializeField] private int materialIndex;
        [SerializeField] private Color secondColor = Color.cyan;
        private Material initialMaterial;
        private Color initialColor;


        private void Awake()
        {
            initialMaterial = renderer.materials[materialIndex];
            initialColor = initialMaterial.color;
        }

        public void SwitchColor()
        {
            initialMaterial.color = initialMaterial.color == initialColor ? secondColor : initialColor;
        }

        public void ResetMaterial()
        {
            initialMaterial.color = initialColor;
        }
    }
}
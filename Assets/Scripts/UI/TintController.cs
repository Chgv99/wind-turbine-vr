using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static PlasticGui.LaunchDiffParameters;

namespace WindTurbineVR.UI
{
    public class TintController : XRTintInteractableVisual
    {
        protected override void SetTint(bool on)
        {
            Debug.Log("SetTint(" + on + ")");
            SetLayer(on ? 18 : 0);
            base.SetTint(on);
        }

        void SetLayer(int layer) => SetLayer(layer, transform);

        void SetLayer(int layer, Transform transform)
        {
            transform.gameObject.layer = layer;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                // Checks if SEETHROUGH is enabled before changing the layer to "Highlight".
                // which causes a seethrough behaviour.
                //if (layer == 0 || PlayerPrefs.HasKey("SEETHROUGH")) 
                    child.gameObject.layer = layer;

                if (child.childCount > 0) SetLayer(layer, child);
            }
        }
    }

}
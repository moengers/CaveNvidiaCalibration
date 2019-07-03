using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Htw.Cave.Calibration.Utility
{
    /// <summary>
    /// Provides a centralized location for referencing the CAVE game object.
    /// </summary>
    [AddComponentMenu("Htw.Cave/Calibration/Utility/Virtual Environment Link")]
    public class VirtualEnvironmentLink : MonoBehaviour
    {
        [SerializeField]
        private GameObject virtualEnvironment;
        public GameObject VirtualEnvironment
        {
            get => this.virtualEnvironment;
            set => this.virtualEnvironment = value;
        }
    }
}


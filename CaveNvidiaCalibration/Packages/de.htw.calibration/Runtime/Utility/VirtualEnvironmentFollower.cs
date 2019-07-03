using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Htw.Cave.Projectors;

namespace Htw.Cave.Calibration.Utility
{
    /// <summary>
    /// Updates the position and rotation at the end of every frame to the one of the CAVE.
    /// </summary>
    [AddComponentMenu("Htw.Cave/Calibration/Utility/Virtual Environment Follower")]
    [RequireComponent(typeof(VirtualEnvironmentLink))]
    public class VirtualEnvironmentFollower : MonoBehaviour
    {
        private Transform targetTransform;

        public void Awake()
        {
            this.targetTransform = GetComponent<VirtualEnvironmentLink>().VirtualEnvironment.transform;
        }

        public void LateUpdate()
        {
            transform.position = this.targetTransform.position;
            transform.rotation = this.targetTransform.rotation;
        }
    }
}

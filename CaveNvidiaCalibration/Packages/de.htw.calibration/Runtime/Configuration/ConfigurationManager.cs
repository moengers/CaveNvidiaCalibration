﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Htw.Cave.Projectors;
using Htw.Cave.Calibration.Utility;

namespace Htw.Cave.Calibration.Configuration
{
    [AddComponentMenu("Htw.Cave/Calibration/Configuration/Configuration Manager")]
    [RequireComponent(typeof(VirtualEnvironmentLink))]
    public class ConfigurationManager : MonoBehaviour
    {
        private List<ConfigurationMesh> configurationMeshes;
        public List<ConfigurationMesh> ConfigurationMeshes
        {
            get => this.configurationMeshes;
            set => this.configurationMeshes = value;
        }

        private VirtualEnvironmentLink virtualEnvironmentLink;
        private Queue<ProjectorCamera> cameras;

        public void Awake()
        {
            this.virtualEnvironmentLink = base.GetComponent<VirtualEnvironmentLink>();
            this.configurationMeshes = new List<ConfigurationMesh>();
        }

        public void Start()
        {
            transform.rotation = this.virtualEnvironmentLink.VirtualEnvironment.transform.rotation;
            transform.position = this.virtualEnvironmentLink.VirtualEnvironment.transform.position;

            this.cameras = new Queue<ProjectorCamera>(
                this.virtualEnvironmentLink.VirtualEnvironment
                .GetComponentInChildren<ProjectorMount>().Cameras
            );

            SpawnMeshes();
        }

        private void SpawnMeshes()
        {
            GameObject go;
            for (int i = 0; i < this.cameras.Count; ++i)
            {
                go = new GameObject();
                go.transform.parent = transform;
                go.AddComponent<ConfigurationMesh>();
            }
        }

        public void Register(ConfigurationMesh configurationMesh, out ProjectorCamera camera)
        {
            this.configurationMeshes.Add(configurationMesh);
            camera = this.cameras.Dequeue();
        }
    }
}

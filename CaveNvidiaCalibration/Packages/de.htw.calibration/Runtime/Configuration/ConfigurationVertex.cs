using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Htw.Cave.Calibration.Configuration
{
    /// <summary>
    /// Each Configuration Vertex is part of a Configuration Mesh. Change the vertex position
    /// to respectivaely change the mash.
    /// </summary>
    public class ConfigurationVertex : MonoBehaviour
    {
        private int index;
        public int Index
        {
            get => this.index;
            set => this.index = value;
        }

        private ConfigurationMesh configurationMesh;
        public ConfigurationMesh ConfigurationMesh
        {
            get => this.configurationMesh;
            set => this.configurationMesh = value;
        }

        public void Awake()
        {
            this.configurationMesh = base.GetComponentInParent<ConfigurationMesh>();
        }

        public void Start()
        {
            this.configurationMesh.Register(this, out this.index);
            gameObject.name = "Vertex " + ResolveVertexName(this.index);
            transform.localRotation = Quaternion.identity;
            transform.position = this.configurationMesh.Vertices[this.index];
        }

        public void Update()
        {
            this.configurationMesh.Vertices[this.index] = transform.localPosition;
            this.configurationMesh.Build();
        }

        private static string ResolveVertexName(int index)
        {
            switch (index)
            {
                case 0:
                    return "Top Left";
                case 1:
                    return "Top Right";
                case 2:
                    return "Bottom Left";
                case 3:
                    return "Bottom Right";
                default:
                    return "Unknown";
            }
        }

        public void Move(Vector3 deltaPosition)
        {
            transform.localPosition += deltaPosition;
            this.configurationMesh.Vertices[this.index] = transform.localPosition;
            this.configurationMesh.Build();
        }
    }
}

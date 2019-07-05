using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Htw.Cave.Projectors;

namespace Htw.Cave.Calibration.Configuration
{
    /// <summary>
    /// The Calibration Mesh is a visual representation how the post warped screen will look like without
    /// the need to apply the warp for every change made. It consists of four vertices that define it's size.
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ConfigurationMesh : MonoBehaviour
    {
        private ConfigurationManager configurationManager;
        public ConfigurationManager ConfigurationManager
        {
            get => this.configurationManager;
            set => this.configurationManager = value;
        }

        private ConfigurationVertex[] configurationVertices;
        public ConfigurationVertex[] ConfigurationVertices
        {
            get => this.configurationVertices;
            set => this.configurationVertices = value;
        }

        private Vector3[] vertices;
        public Vector3[] Vertices
        {
            get => this.vertices;
            set => this.vertices = value;
        }

        private int nextIndex;
        private ProjectorCamera cam;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        public void Awake()
        {
            this.configurationManager = base.GetComponentInParent<ConfigurationManager>();
            this.nextIndex = 0;
            this.configurationVertices = new ConfigurationVertex[4];
            this.vertices = new Vector3[4];
            this.meshFilter = base.GetComponent<MeshFilter>();
            this.meshFilter.mesh = new Mesh();
            this.meshRenderer = base.GetComponent<MeshRenderer>();
        }

        public void Start()
        {
            this.configurationManager.Register(this, out this.cam);
            gameObject.name = "Mesh " + cam.Configuration.DisplayName;
            transform.rotation = this.cam.Plane.transform.rotation;
            transform.position = this.cam.Plane.transform.position;

            CalculateVertexStartPosition();
            SpawnVertices();

            this.meshFilter.mesh.name = "Configuration Mesh";
            this.meshRenderer.receiveShadows = false;
            this.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            this.meshRenderer.material = Resources.Load<Material>("Material " + (this.cam.Configuration.DisplayId + 1));
            Build(true);
        }

        private void SpawnVertices()
        {
            GameObject go;
            for (int i = 0; i < 4; ++i)
            {
                go = new GameObject();
                go.transform.parent = transform;
                go.AddComponent<ConfigurationVertex>();
            }
        }

        public void Register(ConfigurationVertex configurationVertex, out int index)
        {
            this.configurationVertices[nextIndex] = configurationVertex;
            index = nextIndex++;
        }

        private void CalculateVertexStartPosition()
        {
            Vector3[] corners = this.cam.TransformPlanePoints();
            this.vertices[0] = corners[0];
            this.vertices[1] = corners[1];
            this.vertices[2] = corners[3];
            this.vertices[3] = corners[2];
        }

        public void Build(bool force = false)
        {
            this.meshFilter.mesh.vertices = this.vertices;

            if (force)
            {
                this.meshFilter.mesh.MarkDynamic();
                this.meshFilter.mesh.uv = new Vector2[4] {
                    new Vector2(0, 0), new Vector2(0, 1),
                    new Vector2(1, 0), new Vector2(1, 1)
                };
                this.meshFilter.mesh.triangles = new int[6] { 3, 2, 0, 3, 0, 1 };
            }

            this.meshFilter.mesh.RecalculateNormals();
            this.meshFilter.mesh.RecalculateBounds();
            this.meshFilter.mesh.RecalculateTangents();
        }
    }
}

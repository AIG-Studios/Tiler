using UnityEngine;

namespace Tiler
{

    public class LayoutTile : MonoBehaviour
    {
        public float Length = 1.0f;
        public LayoutEntry.LayoutMode OverrideLayoutMode = LayoutEntry.LayoutMode.None;

        // used by runtime instantiator to check if this is instance of prefab that we want
        [HideInInspector]
        public LayoutTile ParentPrefab;

        public Mesh Mesh
        {
            get
            {
                return GetComponentInChildren<MeshFilter>().sharedMesh;
            }
        }

        public MeshRenderer MeshRenderer
        {
            get
            {
                return GetComponentInChildren<MeshRenderer>();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            DrawMarkerAt(Vector3.zero);
            DrawMarkerAt(Vector3.right * Length);
        }

        void DrawMarkerAt(Vector3 point)
        {
            float size = 1.0f;
            Gizmos.DrawCube(point, new Vector3(0, size, size));
        }
    }
}

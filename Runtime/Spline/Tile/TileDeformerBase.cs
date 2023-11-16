using UnityEngine;
using static Tiler.LayoutEntry;

namespace Tiler
{

    public class TileDeformerBase : ITileDeformer
    {
        private GameObject _resultObject;
        private Mesh _result;
        private Mesh _source;
        private Transform _sourceTransform;
        protected TilerOptions _options;
        protected float _intervalStart, _intervalEnd, _meshLength;


        public void Deform(TilerOptions options, LayoutEntry entry, GameObject obj)
        {
            _options = options;

            Mesh mesh = null;
            if (obj.TryGetComponent<MeshFilter>(out var filter))
                mesh = filter.sharedMesh;
            else
            {
                Debug.Assert(!WantsToDeformMesh(entry.Mode), "We need mesh to deform!");
            }

            var proposed = entry.Tile;
            DeformMesh(obj, mesh, proposed.Mesh, proposed.transform, entry.From, entry.To, entry.Tile.Length, entry.Mode);
        }

        /// <summary>
        /// Sets a spline's interval along which the mesh will be bent.
        /// If interval end is absent or set to 0, the interval goes from start to spline length.
        /// The mesh will be update if any of the curve changes on the spline, including curves
        /// outside the given interval.
        /// </summary>
        /// <param name="spline">The <see cref="SplineMesh"/> to bend the source mesh along.</param>
        /// <param name="intervalStart">Distance from the spline start to place the mesh minimum X.<param>
        /// <param name="intervalEnd">Distance from the spline start to stop deforming the source mesh.</param>
        void DeformMesh(GameObject resultObject, Mesh result, Mesh source, Transform sourceTransform, float intervalStart, float intervalEnd, float meshLength, LayoutMode mode)
        {
            _resultObject = resultObject;
            _sourceTransform = sourceTransform;
            _intervalStart = intervalStart;
            _intervalEnd = intervalEnd;
            _meshLength = meshLength;
            _source = source;

            if (WantsToDeformMesh(mode))
            {
                _result = result;
                PrepareResult();
            }
            else
            {
                _result = null;
            }

            switch (mode)
            {
                case LayoutMode.DeformAndStretch:
                    FillStretch();
                    break;
                case LayoutMode.Deform:
                    FillOnce();
                    break;
                case LayoutMode.OnlyAlign:
                    OnlyAlign();
                    break;
                case LayoutMode.None:
                    break;
            }
        }

        void PrepareResult()
        {

        }

        bool WantsToDeformMesh(LayoutMode mode)
        {
            return mode switch
            {
                LayoutMode.DeformAndStretch or LayoutMode.Deform => true,
                _ => false,
            };
        }

        private void OnlyAlign()
        {
            var length = _intervalEnd - _intervalStart;
            var startPoint = GetPositionAtDistance(0.0f);
            var endPoint = GetPositionAtDistance(length);
            var direction = (endPoint - startPoint).normalized;

            Vector3 position;
            if (!_options.flipTiles)
            {
                position = _options.root.transform.InverseTransformPoint(startPoint);
            }
            else
            {
                position = _options.root.transform.InverseTransformPoint(endPoint);
                direction = -direction;
            }

            Vector3 axis = GetNormalAtDistance(length / 2);
            _resultObject.transform.localPosition = position;
            _resultObject.transform.localRotation = Quaternion.LookRotation(direction, axis);
            _resultObject.transform.Rotate(Vector3.up, -90);

            _resultObject.transform.Translate(_sourceTransform.transform.position, Space.Self);
        }

        protected virtual Vector3 GetPositionAtDistance(float d)
        {
            return Vector3.zero;
        }

        protected virtual Vector3 GetNormalAtDistance(float distance)
        {
            return Vector3.zero;
        }

        private void FillOnce()
        {
            // for each mesh vertex, we found its projection on the curve

            Vector3[] vertices = _source.vertices;
            Vector3[] normals = _source.normals;

            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                var normal = normals[i];

                vertex = _sourceTransform.TransformPoint(vertex);
                normal = _sourceTransform.TransformPointUnscaled(normal);

                float distance = vertex.x * _options.globalScale;

                BendVertex(ref vertex, ref normal, distance);
                vertices[i] = vertex;
                normals[i] = normal;
            }

            PostProcess(vertices, normals);
        }

        private void FillStretch()
        {
            // for each mesh vertex, we found its projection on the curve

            Vector3[] vertices = _source.vertices;
            Vector3[] normals = _source.normals;

            for (int i = 0; i < vertices.Length; i++)
            {
                var vertex = vertices[i];
                var normal = normals[i];

                vertex = _sourceTransform.TransformPoint(vertex);
                normal = _sourceTransform.TransformPointUnscaled(normal);

                float distanceRate = (_intervalEnd - _intervalStart) / (_meshLength * _options.globalScale);
                float distance = vertex.x * distanceRate * _options.globalScale;

                BendVertex(ref vertex, ref normal, distance);
                vertices[i] = vertex;
                normals[i] = normal;
            }

            PostProcess(vertices, normals);
        }

        void PostProcess(Vector3[] vertices, Vector3[] normals)
        {
            _result.indexFormat = _source.indexFormat;
            _result.vertices = vertices;
            _result.normals = normals;
            _result.uv = _source.uv;
            _result.triangles = _source.triangles;
            _result.RecalculateBounds();
            _result.RecalculateTangents();
        }

        protected virtual void BendVertex(ref Vector3 vertex, ref Vector3 normal, float distance)
        {

        }

    }

}


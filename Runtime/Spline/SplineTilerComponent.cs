using BezierSolution;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tiler
{
    /// <summary>
    /// Deform a mesh and place it along a spline, given various parameters.
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class SplineTilerComponent : MonoBehaviour
    {
        [Tooltip("Layouter to add tiles to the spline.")]
        public ScriptableTileLayouter Layouter;
        public int Seed = 0;
        public float Scaling = 1;
        public GameObject GenerationRoot;

        BezierSpline _bezierSpline;

        void SetBezierSpline(BezierSpline spline)
        {
            if (_bezierSpline != null)
                _bezierSpline.onSplineChanged -= SplineChangeDelegate;
            _bezierSpline = spline;
            if (_bezierSpline)
                _bezierSpline.onSplineChanged += SplineChangeDelegate;
        }

        private void OnEnable()
        {
            SetBezierSpline(GetComponent<BezierSpline>());
            Rebake();
        }

        private void OnDisable()
        {
            SetBezierSpline(null);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // we cannot remove gameObjects during validation
            EditorApplication.delayCall += Rebake;
        }
#endif

        void SplineChangeDelegate(BezierSpline spline, DirtyFlags dirtyFlags)
        {
            Rebake();
        }

        public void Rebake()
        {
            if (!_bezierSpline)
                return;
            if (!Layouter)
                return;

            var options = new TilerOptions();
            options.owner = gameObject;
            options.root = GenerationRoot;
            options.layouter = Layouter;
            options.seed = Seed;
            options.bakeToDisk = false;
            options.globalScale = Scaling;

            SplineRuntimeTiler.Instance.CreateTiles(_bezierSpline, options);
        }

        public void Clear()
        {
            if (GenerationRoot)
                GenerationRoot.DestroyChildrenSafe();
        }

        public void CenterPivot()
        {
            if (!_bezierSpline)
                return;
            var cache = _bezierSpline.GeneratePointCache();
            var center = cache.GetPoint(0.5f);
            var delta = center - transform.position;

            transform.position += delta;
            foreach (var point in _bezierSpline)
            {
                point.transform.position -= delta;
            }
        }
    }
}

using BezierSolution;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
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

        private void OnValidate()
        {
            Rebake();
        }

        void SplineChangeDelegate(BezierSpline spline, DirtyFlags dirtyFlags)
        {
            Rebake();
        }

        [Button]
        void Rebake()
        {
            if (!_bezierSpline)
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

        [Button]
        void CenterPivot()
        {
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

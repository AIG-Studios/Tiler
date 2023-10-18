using BezierSolution;
using UnityEngine;
using System.Numerics;
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
        [Header("Layout")]
        [Tooltip("Layouter to add tiles to the spline.")]
        public ScriptableTileLayouter Layouter;

        [Header("Layout options")]
        public int Seed = 0;
        public float Scaling = 1;
        public bool FlipTiles = false;

        [Header("Settings")]
        public GameObject GenerationRoot;

        public bool IsBaked {  get { return isBaked; } }
        public int BakedVersion { get { return bakedVersion; } }

        BezierSpline _bezierSpline;

        [SerializeField, HideInInspector]
        private bool isBaked = false;
        [SerializeField, HideInInspector]
        private int bakedVersion = 0;


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

        public void ForceRebake()
        {
            Clear();
            Rebake();
        }

        public void Rebake()
        {
            if (!_bezierSpline)
                return;
            if (!Layouter)
                return;
            if (isBaked)
                return;

            var options = new TilerOptions();
            options.owner = gameObject;
            options.root = GenerationRoot;
            options.layouter = Layouter;
            options.seed = Seed;
            options.bakeToDisk = false;
            options.globalScale = Scaling;
            options.flipTiles = FlipTiles;

            SplineRuntimeTiler.Instance.CreateTiles(_bezierSpline, options);
        }

        private void Clear()
        {
            if (GenerationRoot)
                GenerationRoot.DestroyChildrenSafe();

            Transform bakeRoot = GetBakeRoot();
            if (bakeRoot)
                bakeRoot.gameObject.DestroyChildrenSafe();
            isBaked = false;
        }

        public void AssignBaked(GameObject baked, int version = 0)
        {
            Clear();

            isBaked = true;
            baked.transform.parent = CreateOrGetBakeRoot();
            bakedVersion = version;

            EditorUtility.SetDirty(gameObject);
            EditorUtility.SetDirty(this);
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

        private Transform GetBakeRoot()
        {
            return transform.Find(BAKE_ROOT_NAME);
        }

        private Transform CreateOrGetBakeRoot()
        {
            Transform root = GetBakeRoot();
            if (root)
                return root;

            GameObject rootObject = new GameObject(BAKE_ROOT_NAME);
            rootObject.transform.SetParent(transform, false);
            return rootObject.transform;
        }

        private const string BAKE_ROOT_NAME = "Baked";
    }
}

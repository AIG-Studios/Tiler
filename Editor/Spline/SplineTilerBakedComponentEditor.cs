using BezierSolution;
using UnityEditor;
using UnityEngine;

namespace Tiler
{

    [CustomEditor(typeof(SplineTilerBakedComponent))]
    public class SplineTilerBakedComponentEditor : Editor
    {
        Object _currentTarget;
        SplineTilerBakedComponent _splineTarget;
        GameObject _bakeRoot;
        BezierSpline _spline;
        bool _bakeDirty = false;
        bool _pendingSave = false;

        [InitializeOnLoadMethod]
        static void ConnectListeners()
        {
            SplineTilerBakedComponent.OnDestroyedInEditor.AddListener(OnObjectDestroyed);
        }

        static void OnObjectDestroyed(SplineTilerBakedComponent component)
        {
            // if object is removed, destroy it's baked content
            SplineBakerTiler.Instance.DestroyBakedContent(component.gameObject);
        }

        public override void OnInspectorGUI()
        {
            SetTarget(target);
            DrawDefaultInspector();
            if (GUILayout.Button("Randomize"))
            {
                RandomizeSeed();
            }

            if (GUILayout.Button("Bake"))
            {
                Bake(true);
            }

            if (_bakeDirty)
            {
                BakeToMemory();
            }
        }

        private void OnEnable()
        {
            SetTarget(target);
        }

        private void OnDisable()
        {
            SetTarget(null);
        }

        void BakeToMemory()
        {
            Bake(false);
            _pendingSave = true;
            _bakeDirty = false;
        }

        void RandomizeSeed()
        {
            if (_splineTarget == null)
                return;
            _splineTarget.Seed = Random.Range(0, int.MaxValue);
            BakeToMemory();
        }

        void Bake(bool saveToDisk)
        {
            if (!CanBake(_splineTarget)) return;

            if (_splineTarget.Layouter == null)
            {
                Debug.LogWarning("Layouter not set!");
                return;
            }

            var options = new TilerOptions();
            options.owner = _splineTarget.gameObject;
            options.root = _bakeRoot;
            options.layouter = _splineTarget.Layouter;
            options.seed = _splineTarget.Seed;
            options.bakeToDisk = saveToDisk;

            SplineBakerTiler.Instance.CreateTiles(_spline, options);

            if (saveToDisk && _pendingSave)
                _pendingSave = false;
        }

        bool CanBake(Object obj)
        {
            if (obj == null)
                return false;

            // disallow baking instances of prefab, so instances will share mesh with root
            if (PrefabUtility.GetPrefabInstanceStatus(obj) != PrefabInstanceStatus.NotAPrefab)
                return false;

            return true;
        }

        void SetTarget(Object target)
        {
            if (_currentTarget == target)
                return;

            if (_pendingSave)
                Bake(true);

            _bakeDirty = false;
            _currentTarget = target;
            _splineTarget = (SplineTilerBakedComponent)target;

            if (_splineTarget)
            {
                if (CanBake(target))
                {
                    _bakeRoot = GetRoot();
                    _spline = _splineTarget.GetComponent<BezierSpline>();
                    _spline.onSplineChanged += OnSplineChanged;
                }
            }
            else
            {
                if (_spline)
                    _spline.onSplineChanged += OnSplineChanged;
                _spline = null;
                _bakeRoot = null;
            }

        }

        void OnSplineChanged(BezierSpline spline, DirtyFlags dirtyFlags)
        {
            SetBakeDirty();
        }

        void SetBakeDirty()
        {
            _bakeDirty = true;
        }

        GameObject GetRoot()
        {
            var str = GlobalObjectId.GetGlobalObjectIdSlow(_splineTarget).ToString();

            string prefix = ">";
            string rootName = prefix + str;

            // remove other generated children - could happen if we are duplicating an object
            _splineTarget.gameObject.DestroyChildrenImmediateWhere(gameObject =>
            {
                return rootName != gameObject.name && gameObject.name.StartsWith(prefix);
            });

            var generatedTranform = _splineTarget.transform.Find(rootName);
            var root = generatedTranform != null ? generatedTranform.gameObject : TilerUtils.CreateGameObject(rootName, _splineTarget.transform);
            return root;
        }

    }
}

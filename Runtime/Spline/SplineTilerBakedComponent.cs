using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
#endif

namespace Tiler
{
    /// <summary>
    /// Deform a mesh and place it along a spline, given various parameters.
    /// 
    /// Cases to check:
    /// 1. This should work in prefab mode
    /// 2. Instances of prefab should share mesh with parent prefab (currently baking of prefab instance is disallowed)
    /// 3/ There shouldn't be a spontaneus rebake of mesh
    /// </summary>
    [ExecuteAlways]
    [DisallowMultipleComponent]
    public class SplineTilerBakedComponent : MonoBehaviour
    {
#if UNITY_EDITOR
        [Tooltip("Layouter to add tiles to the spline.")]
        public ScriptableTileLayouter Layouter;
        public int Seed = 0;

        public static UnityEvent<SplineTilerBakedComponent> OnDestroyedInEditor = new();

        void OnDestroy()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            // check if this was destroyed because scene was unloaded
            if (!gameObject.scene.isLoaded)
                return;

            // check if destroy was called because we are loading new scene
            if (Time.frameCount == 0 || Time.renderedFrameCount == 0)
                return;

            OnDestroyedInEditor.Invoke(this);
        }
#endif
    }

}

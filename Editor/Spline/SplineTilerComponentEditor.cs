using BezierSolution;
using UnityEditor;
using UnityEngine;

namespace Tiler
{

    [CustomEditor(typeof(SplineTilerComponent))]
    public class SplineTilerComponentEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var spline = target as SplineTilerComponent;

            if (!spline.IsBaked)
            {
                if (GUILayout.Button("Rebake"))
                    Rebake(false);

                if (GUILayout.Button("Center Pivot"))
                    CenterPivot();
            }
            else
            {
                GUILayout.Space(10);
                GUILayout.Label($"<b>Baked</b> (v {spline.BakedVersion})", EditorStyles.boldLabel);
            }

            if (GUILayout.Button("Force rebake"))
                Rebake(true);
        }

        void Rebake(bool force)
        {
            var spline = target as SplineTilerComponent;
            if (force)
                spline.ForceRebake();
            else
                spline.Rebake();
        }

        void CenterPivot()
        {
            // TODO add undo
            var spline = target as SplineTilerComponent;
            spline.CenterPivot();
        }
    }
}

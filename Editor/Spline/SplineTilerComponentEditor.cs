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

            if (GUILayout.Button("Rebake"))
            {
                Rebake();
            }

            if (GUILayout.Button("Center Pivot"))
            {
                CenterPivot();
            }
        }

        void Rebake()
        {
            var spline = target as SplineTilerComponent;
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

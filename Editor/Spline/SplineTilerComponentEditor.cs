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
                Rebake(false);

            if (GUILayout.Button("Center Pivot"))
                CenterPivot();

            if (GUILayout.Button("Force rebake"))
                Rebake(true);
        }

        void Rebake(bool force)
        {
            var spline = target as SplineTilerComponent;

            // TODO currently 
            if (force)
                spline.Clear();
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

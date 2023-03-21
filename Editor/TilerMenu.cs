using BezierSolution;
using UnityEditor;
using UnityEngine;

namespace Tiler
{
    internal static class TilerMenu
    {
        const string m_CinemachineGameObjectRootMenu = "GameObject/Tiler/";
        const int m_MenuPriority = 20;

        [MenuItem(m_CinemachineGameObjectRootMenu + "Spline", false, m_MenuPriority)]
        static void CreateSpline(MenuCommand command)
        {
            GameObject go = TilerUtils.CreateGameObject("Tiled Spline",
                    null,
                    typeof(BezierSpline),
                    typeof(SplineTilerComponent));

            GameObject root = TilerUtils.CreateGameObject("Root", go.transform);
            root.transform.SetAsFirstSibling();

            go.GetComponent<SplineTilerComponent>().GenerationRoot = root;

            SetParentToMenuContextObject(go, command);

            if (SceneView.lastActiveSceneView != null)
                go.transform.position = SceneView.lastActiveSceneView.pivot;
            Undo.RegisterCreatedObjectUndo(go, "create Spline camera");
        }

        static void SetParentToMenuContextObject(GameObject child, MenuCommand command)
        {
            var go = command.context as GameObject;
            if (go != null)
                Undo.SetTransformParent(child.transform, go.transform, "set parent");
            Selection.activeObject = child;
        }
    }
}

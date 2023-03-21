using System;
using UnityEngine;
namespace Tiler
{
    public static class GameObjectExtension
    {
        public static void DestroyChildren(this GameObject obj)
        {
            foreach (Transform child in obj.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        public static void DestroyChildrenImmediate(this GameObject obj)
        {
            while (obj.transform.childCount > 0)
            {
                var child = obj.transform.GetChild(0);
                GameObject.DestroyImmediate(child.gameObject);
            }
        }

        public static void DestroyChildrenImmediateWhere(this GameObject obj, Func<GameObject, bool> filter)
        {
            while (true)
            {
            FoundMatch:
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    var child = obj.transform.GetChild(i);
                    if (filter(child.gameObject))
                    {
                        GameObject.DestroyImmediate(child.gameObject);
                        goto FoundMatch;
                    }
                }
                break;
            }
        }

        public static void DestroyChildrenSafe(this GameObject obj)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                obj.DestroyChildrenImmediate();
                return;
            }
#endif
            obj.DestroyChildren();
        }

        public static void DestroySafe(GameObject obj)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject.DestroyImmediate(obj);
                return;
            }
#endif
            GameObject.DestroyImmediate(obj);
        }

        public static Rect GetLocalRectBoundsFor(GameObject obj)
        {
            Renderer[] rr = obj.GetComponentsInChildren<Renderer>();
            if (rr.Length == 0)
            {
                Debug.LogWarning("No renderers to include!");
                return Rect.zero;
            }

            Bounds b = rr[0].bounds;
            foreach (Renderer r in rr)
            {
                var rb = r.bounds;
                rb.center -= obj.transform.position;
                b.Encapsulate(rb);
            }
            return new Rect(b.min.x, b.min.y, b.size.x, b.size.y);
        }
    }
}
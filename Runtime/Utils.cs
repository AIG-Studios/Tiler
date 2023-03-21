using System;
using UnityEngine;

namespace Tiler
{
    public static class TilerUtils
    {
        public static GameObject CreateGameObject(string name, Transform parent, params Type[] components)
        {
            var res = new GameObject(name, components);
            res.transform.parent = parent;
            return res;
        }
    }
}

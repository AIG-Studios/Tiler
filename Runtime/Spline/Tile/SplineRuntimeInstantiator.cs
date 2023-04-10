using UnityEngine;

namespace Tiler
{
    class SplineRuntimeInstantiator : ITileInstantiator
    {
        public void Instantiate(ITileDeformer deformer, TilerOptions options)
        {
            InstantiateInternal(deformer, options);
        }

        void InstantiateInternal(ITileDeformer deformer, TilerOptions options)
        {
            int i = 0;
            foreach (var entry in options.layout.Tiles)
            {
                var go = Instantiate(i, entry, options.root);
                deformer.Deform(options, entry, go);
                i++;
            }

            while (i < options.root.transform.childCount)
            {
                var child = options.root.transform.GetChild(i);

                GameObjectExtension.DestroySafe(child.gameObject);
            }
        }

        public void DestroyBakedContent(GameObject owner)
        {

        }


        private GameObject Instantiate(int index, LayoutEntry entry, GameObject root)
        {
            var proposedTile = entry.Tile;
            if (entry.Mode == LayoutEntry.LayoutMode.OnlyAlign)
                return CreatePrefabInstance(index, proposedTile, root);

            return FindOrCreateMesh(index, proposedTile, root);
        }

        private GameObject CreatePrefabInstance(int index, LayoutTile proposed, GameObject root)
        {
            var prefabInstance = "PrefabInstance";
            var res = TryToReuseGameObjectAt(index, root, prefabInstance);

            // check if prefab is correct
            if (res 
                && res.TryGetComponent<LayoutTile>(out var previousTile)
                && previousTile.ParentPrefab != proposed)

            {
                GameObjectExtension.DestroySafe(res);
                res = null;
            }
                    

            if (res == null)
            {
                var tile = GameObject.Instantiate(proposed, root.transform);
                tile.ParentPrefab = proposed;

                res = tile.gameObject;
                res.name = prefabInstance;
                res.transform.SetSiblingIndex(index);
            }

            return res;
        }

        private GameObject FindOrCreateMesh(int index, LayoutTile proposed, GameObject root)
        {
            var deformableMeshName = "Mesh";
            var res = TryToReuseGameObjectAt(index, root, deformableMeshName);

            if (res == null)
            {
                res = TilerUtils.CreateGameObject(deformableMeshName,
                    root.transform,
                    typeof(MeshFilter),
                    typeof(MeshRenderer));
                res.hideFlags = HideFlags.DontSave;
                res.transform.SetSiblingIndex(index);
            }

            // assign empty mesh to created object
            var filter = res.GetComponent<MeshFilter>();
            filter.sharedMesh = new Mesh();

            res.transform.localPosition = Vector3.zero;
            res.transform.localScale = Vector3.one;
            res.transform.localRotation = Quaternion.identity;

            // assign materials from prefab
            res.GetComponent<MeshRenderer>().sharedMaterials = proposed.MeshRenderer.sharedMaterials;
            return res;
        }

        GameObject TryToReuseGameObjectAt(int index, GameObject root, string expectedName)
        {
            GameObject res = null;

            bool indexOutsideBounds = index >= root.transform.childCount;
            if (!indexOutsideBounds)
            {
                res = root.transform.GetChild(index).gameObject;
                bool canReuse = res.name == expectedName;

                if (!canReuse)
                {
                    GameObjectExtension.DestroySafe(res);
                    res = null;
                }
            }
            return res;
        }
    }
}



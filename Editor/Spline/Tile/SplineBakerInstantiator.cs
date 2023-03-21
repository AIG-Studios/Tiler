using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tiler
{
    class SplineBakerInstantiator : ITileInstantiator
    {
        public void Instantiate(ITileDeformer deformer, TilerOptions options)
        {
            InstantiateInternal(deformer, options);
        }

        void InstantiateInternal(ITileDeformer deformer, TilerOptions options)
        {
            string bakePath = null;

            if (options.bakeToDisk)
            {
                bakePath = GetBakePath(options.owner);
                DeletePreviousBakeOnDisk(bakePath);
            }

            try
            {
                AssetDatabase.StartAssetEditing();
                int i = 0;
                foreach (var entry in options.layout.Tiles)
                {
                    var go = Instantiate(i, entry, options.root, bakePath);
                    deformer.Deform(options, entry, go);
#if UNITY_EDITOR

                    EditorUtility.SetDirty(go);
#endif
                    i++;
                }

                while (i < options.root.transform.childCount)
                {
                    var child = options.root.transform.GetChild(i);
                    GameObjectExtension.DestroySafe(child.gameObject);
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }

        public void DestroyBakedContent(GameObject owner)
        {
            var bakePath = GetBakePath(owner);
            DeletePreviousBakeOnDisk(bakePath);
        }

        string GetBakePath(GameObject owner)
        {
            var id = GlobalObjectId.GetGlobalObjectIdSlow(owner).ToString();
            return $"_Baked/{id}/SplineDeform/";
        }

        private GameObject Instantiate(int index, LayoutEntry entry, GameObject root, string bakePath)
        {
            var proposedTile = entry.Tile;
            if (entry.Mode == LayoutEntry.LayoutMode.OnlyAlign)
                return CreatePrefabInstance(index, proposedTile, root);

            return FindOrCreateMesh(index, proposedTile, root, bakePath);
        }

        private GameObject CreatePrefabInstance(int index, LayoutTile proposed, GameObject root)
        {
            var prefabInstance = "PrefabInstance";
            var res = TryToReuseGameObjectAt(index, root, prefabInstance);

            if (res)
            {
                // TODO test if this works
                bool isExpectedPrefabInstance = PrefabUtility.GetPrefabInstanceHandle(res) == proposed;
                if (!isExpectedPrefabInstance)
                {
                    GameObjectExtension.DestroySafe(res);
                    res = null;
                }

            }

            if (res == null)
            {
                res = PrefabUtility.InstantiatePrefab(proposed.gameObject, root.transform) as GameObject;
                res.name = prefabInstance;
                res.transform.SetSiblingIndex(index);
            }

            return res;
        }

        private GameObject FindOrCreateMesh(int index, LayoutTile proposed, GameObject root, string bakePath)
        {
            var deformableMeshName = "Mesh";
            var res = TryToReuseGameObjectAt(index, root, deformableMeshName);

            if (res == null)
            {
                res = TilerUtils.CreateGameObject(deformableMeshName,
                    root.transform,
                    typeof(MeshFilter),
                    typeof(MeshRenderer));
                res.transform.SetSiblingIndex(index);
            }

            // assign empty mesh to created object
            var filter = res.GetComponent<MeshFilter>();
            filter.sharedMesh = CreateMesh(index, bakePath);

            // assign materials from prefab
            res.GetComponent<MeshRenderer>().sharedMaterials = proposed.GetComponent<MeshRenderer>().sharedMaterials;
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

        private void DeletePreviousBakeOnDisk(string bakePath)
        {
            var path = $"{AssetDatabaseUtility.PathRoot()}/{bakePath}";
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.Refresh();
        }

        private Mesh CreateMesh(int index, string bakePath)
        {
            var mesh = new Mesh();
            if (bakePath != null)
                AssetDatabaseUtility.CreateBakedAsset(mesh, null, bakePath, index.ToString());
            return mesh;
        }

    }
}



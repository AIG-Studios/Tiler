using UnityEngine;

namespace Tiler
{
    public class TilerOptions
    {
        // general
        public GameObject owner;
        public GameObject root;
        public ITileLayouter layouter;
        
        // bake options
        public bool bakeToDisk = false;
        
        // layout options
        public int seed = 0;
        public bool flipTiles = false; // TODO for now, this only applies to instantiated (non deformed) meshes
        public float globalScale = 1.0f;

        // half products
        public LayoutResult layout;
    }
}

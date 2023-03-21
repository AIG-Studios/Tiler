using UnityEngine;

namespace Tiler
{
    public class TilerOptions
    {
        public GameObject owner;
        public GameObject root;
        public ITileLayouter layouter;
        public int seed = 0;
        public bool bakeToDisk = false;
        public float globalScale = 1.0f;

        // half products
        public LayoutResult layout;
    }
}

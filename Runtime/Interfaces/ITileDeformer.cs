using UnityEngine;

namespace Tiler
{
    public interface ITileDeformer
    {
        public void Deform(TilerOptions options, LayoutEntry entry, GameObject obj);
    }
}


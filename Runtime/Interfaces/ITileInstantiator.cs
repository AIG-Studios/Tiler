using UnityEngine;

namespace Tiler
{
    public interface ITileInstantiator
    {
        void Instantiate(ITileDeformer deformer, TilerOptions options);
        void DestroyBakedContent(GameObject owner);
    }
}


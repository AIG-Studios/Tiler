using UnityEngine;

namespace Tiler
{
    public abstract class ScriptableTileLayouter : ScriptableObject, ITileLayouter
    {
        public LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options)
        {
            if (_baseLayouter == null)
                _baseLayouter = CreateBaseLayouter();

            return _baseLayouter.LayoutTiles(ref layout, options);
        }

        abstract protected ITileLayouter CreateBaseLayouter();

        ITileLayouter _baseLayouter;

        void OnValidate()
        {
            _baseLayouter = null;
        }
    }
}

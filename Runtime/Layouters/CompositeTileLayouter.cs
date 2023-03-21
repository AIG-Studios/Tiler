using System.Collections.Generic;

namespace Tiler
{
    public abstract class CompositeTileLayouter : ITileLayouter
    {
        protected CompositeTileLayouter()
        {

        }

        protected CompositeTileLayouter(List<ITileLayouter> layouters)
        {
            SetLayouters(layouters);
        }

        public abstract LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options);

        public void SetLayouters(List<ITileLayouter> layouters)
        {
            _layouters = layouters;
        }

        protected List<ITileLayouter> _layouters;
    }
}

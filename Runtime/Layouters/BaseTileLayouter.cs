using System.Collections.Generic;
using System.Linq;

namespace Tiler
{
    public abstract class BaseTileLayouter : ITileLayouter
    {
        protected BaseTileLayouter()
        {

        }

        protected BaseTileLayouter(List<LayoutTile> tiles)
        {
            SetTiles(tiles);
        }

        public abstract LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options);

        public void SetTiles(List<LayoutTile> tiles)
        {
            _tiles = tiles;
        }

        protected List<LayoutTile> _tiles;
    }
}

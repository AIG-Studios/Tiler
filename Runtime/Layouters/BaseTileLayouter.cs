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

        protected static void StretchToFill(ref LayoutData layout, List<LayoutEntry> tiles)
        {
            if (tiles.Count == 0)
                return;

            var delta = layout.To - tiles.Last().To;
            if (delta <= 0)
                return;

            var increasePerElement = delta / tiles.Count;

            var currentPosition = layout.From;
            foreach (var entry in tiles)
            {
                var length = entry.Length + increasePerElement;
                entry.From = currentPosition;
                entry.To = currentPosition + length;

                if (entry.To > layout.To)
                    entry.To = layout.To;

                currentPosition = entry.To;
            }
        }

        protected float LengthOfTile(LayoutTile tile, TilerOptions options)
        {
            return tile.Length * options.globalScale;
        }

        protected List<LayoutTile> _tiles;
    }
}

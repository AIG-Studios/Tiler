using System.Collections.Generic;

namespace Tiler
{

    // fills given space with repeating elements
    public class RepeatingFillLayouter : BaseTileLayouter
    {
        public RepeatingFillLayouter(List<LayoutTile> tiles, bool stretch) : base(tiles)
        {
            _stretch = stretch;
        }

        public override LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options)
        {
            var mode = _stretch ? LayoutEntry.LayoutMode.DeformAndStretch : LayoutEntry.LayoutMode.Deform;
            return TileLayouterHelpers.LayoutTiles(ref layout, options, GetNextTile(), mode);
        }

        IEnumerable<LayoutTile> GetNextTile()
        {
            while (true)
            {
                foreach (var tile in _tiles)
                    yield return tile;
            }
        }

        bool _stretch = false;
    }
}

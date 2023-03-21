using Linq.Extensions;
using System.Collections.Generic;

namespace Tiler
{

    // fills given space with random elements
    public class RandomFillLayouter : BaseTileLayouter
    {
        public RandomFillLayouter(List<LayoutTile> tiles, bool stretch) : base(tiles)
        {
            _stretch = stretch;
        }

        public override LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options)
        {
            var result = new LayoutResult();
            if (_tiles.Count == 0)
                return result;

            var list = new List<LayoutEntry>();
            float distance = layout.From;
            var random = new System.Random(layout.Seed);

            while (true)
            {
                var proposed = _tiles.Random(random);

                var start = distance;
                var end = distance + LengthOfTile(proposed, options);
                if (layout.To < end) break;

                list.Add(new LayoutEntry()
                {
                    Tile = proposed,
                    From = start,
                    To = end,
                    ProposedMode = _stretch ? LayoutEntry.LayoutMode.DeformAndStretch : LayoutEntry.LayoutMode.Deform
                });

                distance = end;
            }

            if (_stretch)
                BaseTileLayouter.StretchToFill(ref layout, list);

            result.Tiles = list;

            return result;
        }

        bool _stretch = false;
    }
}

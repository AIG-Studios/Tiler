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
            var result = new LayoutResult();


            var list = new List<LayoutEntry>();
            var tiles = GetNextTile().GetEnumerator();
            float distance = layout.From;

            while (true)
            {
                tiles.MoveNext();
                var proposed = tiles.Current;

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

using System.Collections.Generic;
using System.Linq;

namespace Tiler
{
    public static class TileLayouterHelpers
    {

        public static void StretchToFill(ref LayoutData layout, List<LayoutEntry> tiles)
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

        public static float LengthOfTile(LayoutTile tile, TilerOptions options)
        {
            return tile.Length * options.globalScale;
        }

        static public LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options, IEnumerable<LayoutTile> tiles, LayoutEntry.LayoutMode mode)
        {
            var result = new LayoutResult();

            var list = new List<LayoutEntry>();
            float distance = layout.From;


            var enumerator = tiles.GetEnumerator();

            while (true)
            {
                if (!enumerator.MoveNext())
                    break;
                var proposed = enumerator.Current;

                var start = distance;
                var end = distance + LengthOfTile(proposed, options);
                if (layout.To < end) break;

                list.Add(new LayoutEntry()
                {
                    Tile = proposed,
                    From = start,
                    To = end,
                    ProposedMode = mode
                });

                distance = end;
            }

            if (mode == LayoutEntry.LayoutMode.DeformAndStretch)
                StretchToFill(ref layout, list);

            result.Tiles = list;

            return result;
        }
    }
}

using Linq.Extensions;
using System.Collections.Generic;

namespace Tiler
{
    // layouts exactly one element in given space
    public class RandomElementLayouter : BaseTileLayouter
    {
        public int Elements = 1;

        public RandomElementLayouter(List<LayoutTile> tiles) : base(tiles)
        {
        }

        public override LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options)
        {
            var result = new LayoutResult();
            if (_tiles.Count == 0)
                return result;

            var list = new List<LayoutEntry>();
            float distance = layout.From;
            var random = new System.Random(layout.Seed);

            for (int i = 0; i < Elements; i++)
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
                });

                distance = end;
            }


            result.Tiles = list;
            return result;
        }
    }
}

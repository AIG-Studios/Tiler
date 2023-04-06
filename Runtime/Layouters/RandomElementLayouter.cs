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
            return TileLayouterHelpers.LayoutTiles(ref layout, options, GetRandomTile(layout.Seed), LayoutEntry.LayoutMode.None);
        }

        IEnumerable<LayoutTile> GetRandomTile(int seed)
        {
            var random = new System.Random(seed);
            for (int i = 0; i < Elements; i++)
                yield return _tiles.Random(random);
        }
    }
}

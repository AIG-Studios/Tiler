using Linq.Extensions;
using System.Collections.Generic;

namespace Tiler
{
    // layouts exactly one element in given space
    public class RandomElementLayouter : BaseTileLayouter
    {
        public int Elements = 1;

        public RandomElementLayouter(List<Entry> tiles) : base(tiles)
        {
            _randomizer = new GenericWeightRandomizer<LayoutTile, Entry>(tiles);
        }

        public override LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options)
        {
            return TileLayouterHelpers.LayoutTiles(ref layout, options, GetRandomTile(layout.Seed), LayoutEntry.LayoutMode.None);
        }

        IEnumerable<LayoutTile> GetRandomTile(int seed)
        {
            var random = new System.Random(seed);
            for (int i = 0; i < Elements; i++)
                yield return _randomizer.Pick(null, random);
        }

        GenericWeightRandomizer<LayoutTile, Entry> _randomizer;
    }
}

using Linq.Extensions;
using System.Collections.Generic;

namespace Tiler
{

    // fills given space with random elements
    public class RandomFillLayouter : BaseTileLayouter
    {
        public RandomFillLayouter(List<Entry> tiles, bool stretch) : base(tiles)
        {
            _stretch = stretch;
            _randomizer = new GenericWeightRandomizer<LayoutTile, Entry>(tiles);
        }

        public override LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options)
        {
            var mode = _stretch ? LayoutEntry.LayoutMode.DeformAndStretch : LayoutEntry.LayoutMode.Deform;
            return TileLayouterHelpers.LayoutTiles(ref layout, options, GetRandomTile(layout.Seed), mode);
        }

        IEnumerable<LayoutTile> GetRandomTile(int seed)
        {
            var random = new System.Random(seed);
            while(true)
            {
                yield return _randomizer.Pick(null, random);
            }
        }

        GenericWeightRandomizer<LayoutTile, Entry> _randomizer;
        bool _stretch = false;
    }
}

namespace Tiler
{
    public class CompositeFillLayouter : ITileLayouter
    {
        public CompositeFillLayouter(ITileLayouter prefix, ITileLayouter fill, ITileLayouter postfix)
        {
            _prefix = prefix;
            _fill = fill;
            _postfix = postfix;
        }

        public LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options)
        {
            var random = new System.Random(layout.Seed);

            var result = new LayoutResult();
            result.Tiles = new();

            // use layouters to layout thinfs like that
            // [prefix]-<fill>-[postfix]
            var prefix = SubLayout(options, _prefix, random, layout.From, layout.To);
            var postfix = SubLayout(options, _postfix, random, prefix.To(), layout.To);

            var postFixFrom = layout.To - prefix.Length();
            postfix.SetNewFrom(postFixFrom);
            var fill = SubLayout(options, _fill, random, prefix.To(), postFixFrom);

            postfix.SetNewFrom(fill.To());

            result.Tiles.AddRange(prefix.Tiles);
            result.Tiles.AddRange(fill.Tiles);
            result.Tiles.AddRange(postfix.Tiles);

            return result;
        }

        LayoutResult SubLayout(TilerOptions options, ITileLayouter layouter, System.Random r, float from, float to)
        {
            var layoutData = new LayoutData();
            layoutData.From = from;
            layoutData.To = to;
            layoutData.Seed = r.Next();

            return layouter.LayoutTiles(ref layoutData, options);
        }

        ITileLayouter _prefix;
        ITileLayouter _fill;
        ITileLayouter _postfix;
    }
}

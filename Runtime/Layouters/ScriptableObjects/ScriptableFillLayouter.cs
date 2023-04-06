using System.Collections.Generic;
using UnityEngine;

namespace Tiler
{
    [CreateAssetMenu(fileName = "FillLayout", menuName = "Tiler/Fill Layout", order = 1)]
    public class ScriptableFillLayouter : ScriptableTileLayouter
    {
        public List<BaseTileLayouter.Entry> Start;
        public List<BaseTileLayouter.Entry> Tiles;
        public List<BaseTileLayouter.Entry> End;

        public bool Stretch = false;

        override protected ITileLayouter CreateBaseLayouter()
        {
            var prefix = new RandomElementLayouter(Start);
            var fill = new RandomFillLayouter(Tiles, Stretch);
            var postfix = new RandomElementLayouter(End);

            return new CompositeFillLayouter(prefix, fill, postfix);
        }
    }
}

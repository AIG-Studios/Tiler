using System.Collections.Generic;
using UnityEngine;

namespace Tiler
{
    [CreateAssetMenu(fileName = "FillLayout", menuName = "Tiler/Fill Layout", order = 1)]
    public class ScriptableFillLayouter : ScriptableTileLayouter
    {
        public List<LayoutTile> Start;
        public List<LayoutTile> Tiles;
        public List<LayoutTile> End;

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

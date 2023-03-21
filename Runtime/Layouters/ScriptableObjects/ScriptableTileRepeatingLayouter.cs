using System.Collections.Generic;
using UnityEngine;

namespace Tiler
{
    [CreateAssetMenu(fileName = "RepeatingLayout", menuName = "Tiler/Repeating Layout", order = 1)]
    public class ScriptableTileRepeatingLayouter : ScriptableTileLayouter
    {
        public List<LayoutTile> Tiles;
        public bool Stretch = false;
        override protected ITileLayouter CreateBaseLayouter()
        {
            return new RepeatingFillLayouter(Tiles, Stretch);
        }
    }
}

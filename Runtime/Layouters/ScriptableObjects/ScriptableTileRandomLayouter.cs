using System.Collections.Generic;
using UnityEngine;

namespace Tiler
{
    [CreateAssetMenu(fileName = "RandomLayout", menuName = "Tiler/Random Layout", order = 1)]
    public class ScriptableTileRandomLayouter : ScriptableTileLayouter
    {
        public List<LayoutTile> Tiles;
        public bool Stretch = false;

        override protected ITileLayouter CreateBaseLayouter()
        {
            return new RandomFillLayouter(Tiles, Stretch);
        }
    }
}

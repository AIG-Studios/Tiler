using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tiler
{
    public abstract class BaseTileLayouter : ITileLayouter
    {
        [Serializable]
        public class Entry : IGenericWeightRandomizerList<LayoutTile>
        {
            [SerializeField] public LayoutTile Tile;
            [SerializeField] public float Weight = 1;

            public float EntryWeight => Weight;
            public LayoutTile EntryValue => Tile;
        }

        protected BaseTileLayouter()
        {

        }

        protected BaseTileLayouter(List<Entry> tiles)
        {
            SetTiles(tiles);
        }

        public abstract LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options);

        protected void SetTiles(List<Entry> tiles)
        {
            _tiles = tiles;
        }

        protected List<Entry> _tiles;
    }
}

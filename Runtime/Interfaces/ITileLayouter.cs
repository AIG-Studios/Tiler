using System.Collections.Generic;
using System.Linq;

namespace Tiler
{
    public class LayoutEntry
    {
        public enum LayoutMode
        {
            None,
            Deform,
            DeformAndStretch,
            OnlyAlign,
        }

        public LayoutTile Tile;
        public float From;
        public float To;
        public LayoutMode ProposedMode
        {
            protected get;
            set;
        }

        public LayoutMode Mode
        {
            get
            {
                if (Tile && Tile.OverrideLayoutMode != LayoutMode.None)
                    return Tile.OverrideLayoutMode;
                return ProposedMode;
            }
        }

        public float Length
        {
            get { return To - From; }
        }
    }

    public struct LayoutData
    {
        public float From;
        public float To;
        public int Seed;
    }

    public struct LayoutResult
    {
        public List<LayoutEntry> Tiles;

        public float From()
        {
            if (Tiles.Count == 0)
                return 0;
            return Tiles.First().From;
        }

        public float To()
        {
            if (Tiles.Count == 0)
                return 0;
            return Tiles.Last().To;
        }

        public float Length()
        {
            return To() - From();
        }

        public void SetNewFrom(float from)
        {
            if (Tiles.Count == 0)
                return;
            var delta = from - Tiles.First().From;
            foreach (var tile in Tiles)
            {
                tile.From += delta;
                tile.To += delta;
            }
        }

    }

    public interface ITileLayouter
    {
        public LayoutResult LayoutTiles(ref LayoutData layout, TilerOptions options);
    }


}


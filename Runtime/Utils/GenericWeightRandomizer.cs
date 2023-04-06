using System.Collections.Generic;
namespace Tiler
{
    public interface IGenericWeightRandomizerList<T>
    {
        float EntryWeight
        {
            get;
        }

        T EntryValue
        {
            get;
        }
    }

    public class GenericWeightRandomizer<T, L> where L : IGenericWeightRandomizerList<T>
    {
        public GenericWeightRandomizer(List<L> list = null)
        {
            Prepare(list);
        }

        public T Pick(List<L> list, System.Random random)
        {
            Prepare(list != null ? list : _list);

            var v = random.NextDouble() * _maxWeight;

            foreach (var entry in _cache)
            {
                if (v < entry.weight) return entry.entry;
            }

            return default(T);
        }

        public void Prepare(List<L> list)
        {
            if (list == null) return;
            if (_list == list && _cachedSize == list.Count) return;
            _list = list;
            _cachedSize = _list.Count;
            _cache = new List<CacheInfo>();

            float c = 0.0f;
            foreach (var entry in list)
            {
                c += entry.EntryWeight;
                _cache.Add(new CacheInfo(c, entry.EntryValue));
            }
            _maxWeight = c;
        }

        protected struct CacheInfo
        {
            public CacheInfo(float w, T t) { weight = w; entry = t; }
            public float weight;
            public T entry;
        }

        public List<L> GetList() { return _list; }

        List<L> _list;
        List<CacheInfo> _cache;
        int _cachedSize = -1;
        float _maxWeight;
    }
}
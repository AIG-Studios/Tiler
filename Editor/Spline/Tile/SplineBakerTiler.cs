using BezierSolution;
using UnityEngine;

namespace Tiler
{
    public class SplineBakerTiler : Singleton<SplineBakerTiler>
    {
        ITileInstantiator _instantiator;
        BezierSplineDeformer _deformer;

        public SplineBakerTiler()
        {
            _instantiator = new SplineBakerInstantiator();
            _deformer = new BezierSplineDeformer();
        }

        public void CreateTiles(BezierSpline spline, TilerOptions options)
        {
            var layout = CreateLayout(spline, options);
            _deformer.SetSpline(spline);
            _instantiator.Instantiate(_deformer, options);
        }

        public void DestroyBakedContent(GameObject owner)
        {
            _instantiator.DestroyBakedContent(owner);
        }

        LayoutResult CreateLayout(BezierSpline spline, TilerOptions options)
        {
            var layoutData = new LayoutData()
            {
                From = 0,
                To = spline.length,
                Seed = options.seed
            };

            return options.layouter.LayoutTiles(ref layoutData, options);
        }
    }
}



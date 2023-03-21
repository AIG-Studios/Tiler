using BezierSolution;
using UnityEngine;

namespace Tiler
{
    public class SplineRuntimeTiler : Singleton<SplineRuntimeTiler>
    {
        ITileInstantiator _instantiator;
        BezierSplineDeformer _Deformer;


        public SplineRuntimeTiler()
        {
            _instantiator = new SplineRuntimeInstantiator();
            _Deformer = new BezierSplineDeformer();
        }

        public void CreateTiles(BezierSpline spline, TilerOptions options)
        {
            options.layout = CreateLayout(spline.length, options);
            _Deformer.SetSpline(spline);
            _instantiator.Instantiate(_Deformer, options);
        }

        public void DestroyBakedContent(GameObject owner)
        {
            _instantiator.DestroyBakedContent(owner);
        }

        LayoutResult CreateLayout(float length, TilerOptions options)
        {
            var layoutData = new LayoutData()
            {
                From = 0,
                To = length,
                Seed = options.seed
            };

            return options.layouter.LayoutTiles(ref layoutData, options);
        }
    }
}



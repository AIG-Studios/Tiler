using BezierSolution;
using UnityEngine;
using static BezierSolution.BezierSpline;

namespace Tiler
{
    public class BezierSplineDeformer : TileDeformerBase
    {
        BezierSpline _spline;
        float _length = 0;
        PointCache _cache;

        public void SetSpline(BezierSpline spline)
        {
            _cache = spline.GeneratePointCache();
            _spline = spline;
            _length = _spline.length;
        }

        static Quaternion baseRotation = Quaternion.Euler(0, -90, 0);
        protected override void BendVertex(ref Vector3 vertex, ref Vector3 normal, float distance)
        {
            var p = (_intervalStart + distance) / _length;
            var bPoint = _cache.GetPoint(p);
            var bNormal = _cache.GetNormal(p);
            var bDirection = _cache.GetTangent(p);

            // reset X value (as vertex will be translated anyway)
            vertex.x = 0;
            vertex *= _options.globalScale;

            // application of the rotation + location
            var rotation = Quaternion.LookRotation(bDirection, bNormal);
            Quaternion q = rotation * baseRotation;
            vertex = q * vertex + bPoint;
            normal = q * normal;

            vertex = _spline.transform.InverseTransformPoint(vertex);
            normal = _spline.transform.InverseTransformDirection(normal);
        }


        protected override Vector3 GetPositionAtDistance(float distance)
        {
            var p = (_intervalStart + distance) / _length;
            return _cache.GetPoint(p);
        }

        protected override Vector3 GetNormalAtDistance(float distance)
        {
            var p = (_intervalStart + distance) / _length;
            return _cache.GetNormal(p);
        }
    }
}

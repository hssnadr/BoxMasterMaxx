using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bezier
{
    public class RandomSplineWalker : SplineWalker
    {
        public BezierSpline nextSpline;

        protected override void Update()
        {
            base.Update();
        }

        void Start()
        {
            CreateNextSpline();
        }

        private BezierSpline CreateNextSpline()
        {
            spline = new GameObject("Spline").AddComponent<BezierSpline>();

            spline.Reset();

            // We create the first curve of the spline.
            spline.AddCurve();
            SetRandomPoint();
            spline.AddCurve();
            SetRandomPoint();
            spline.AddCurve();
            SetRandomPoint();

            return nextSpline;
        }

        private void SetRandomPoint()
        {
            Vector2 randomPoint = new Vector2(Random.Range(0.0f, 200.0f), Random.Range(0.0f, 200.0f));

            spline.SetControlPoint(spline.ControlPointCount - 1, randomPoint);
            spline.SetControlPointMode(spline.ControlPointCount - 1, BezierControlPointMode.Mirrored);

            spline.SetControlPoint(spline.ControlPointCount - 2, randomPoint + Random.insideUnitCircle * 200.0f);
            spline.SetControlPointMode(spline.ControlPointCount - 2, BezierControlPointMode.Mirrored);
        }
    }
}

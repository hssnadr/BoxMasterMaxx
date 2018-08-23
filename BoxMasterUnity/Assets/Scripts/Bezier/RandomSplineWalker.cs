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
            spline = CreateNextSpline();
        }

        private BezierSpline CreateNextSpline()
        {
            nextSpline = new GameObject("Spline").AddComponent<BezierSpline>();

            nextSpline.Reset();

            // We create the first curve of the spline.
            for (int i = 0; i < 10; i++)
            {
                //SetRandomPoint(nextSpline);
                nextSpline.AddCurve();
                SetRandomPoint(nextSpline);
                nextSpline.AddCurve();
                SetRandomPoint(nextSpline);
                nextSpline.AddCurve();
                SetRandomPoint(nextSpline);
            }

            return nextSpline;
        }

        private void SetRandomPoint(BezierSpline spline)
        {
            var camera = GameManager.instance.GetCamera(0).GetComponent<Camera>();
            Vector2 randomPoint = camera.ViewportToWorldPoint(new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.2f, 0.8f)));

            spline.SetControlPoint(spline.ControlPointCount - 1, randomPoint);
            spline.SetControlPointMode(spline.ControlPointCount - 1, BezierControlPointMode.Mirrored);

            spline.SetControlPoint(spline.ControlPointCount - 2, randomPoint + Random.insideUnitCircle * 50.0f);
            spline.SetControlPointMode(spline.ControlPointCount - 2, BezierControlPointMode.Mirrored);
        }
    }
}

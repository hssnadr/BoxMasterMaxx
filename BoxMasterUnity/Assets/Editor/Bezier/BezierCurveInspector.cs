// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bezier
{
    [CustomEditor(typeof(BezierCurve))]
    public class BezierCurveInspector : Editor
    {
        private BezierCurve _curve;
        private Transform _handleTransform;
        private Quaternion _handleRotation;

        private const int lineSteps = 20;
        private const float directionScale = 0.5f;

        private void OnSceneGUI()
        {
            _curve = target as BezierCurve;
            _handleTransform = _curve.transform;
            _handleRotation = Tools.pivotRotation == PivotRotation.Local ?
                _handleTransform.rotation : Quaternion.identity;

            Vector3 p0 = ShowPoint(0);
            Vector3 p1 = ShowPoint(1);
            Vector3 p2 = ShowPoint(2);
            Vector3 p3 = ShowPoint(3);

            // The color of the line
            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            ShowDirections();
            Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2.0f);
        }

        private void ShowDirections()
        {
            Handles.color = Color.green;
            Vector3 point = _curve.GetPoint(0.0f);
            Handles.DrawLine(point, point + _curve.GetDirection(0.0f) * directionScale);
            for (int i = 0; i < lineSteps; i++)
            {
                point = _curve.GetPoint((1 + i) / (float)lineSteps);
                Handles.DrawLine(point, point + _curve.GetDirection((i + 1) / (float)lineSteps) * directionScale);
            }
        }

        private Vector3 ShowPoint(int index)
        {
            Vector3 point = _handleTransform.TransformPoint(_curve.points[index]);
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, _handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_curve, "Move Point");
                EditorUtility.SetDirty(_curve);
                _curve.points[index] = _handleTransform.InverseTransformPoint(point);
            }
            return point;
        }
    }
}
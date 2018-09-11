// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bezier
{
    [CustomEditor(typeof(Line))]
    public class LineInspector : Editor
    {
        private void OnSceneGUI()
        {
            Line line = target as Line;
            // We convert the points into world space
            Transform handleTransform = line.transform;
            Vector3 p0 = handleTransform.TransformPoint(line.p0);
            Vector3 p1 = handleTransform.TransformPoint(line.p1);

            // To show position handle for the two points we need the transform's rotation
            Quaternion handleRotation = Tools.pivotRotation == PivotRotation.Local ? handleTransform.rotation : Quaternion.identity;

            Handles.color = Color.white;
            Handles.DrawLine(p0, p1);
            Handles.DoPositionHandle(p0, handleRotation);
            Handles.DoPositionHandle(p1, handleRotation);

            // We change the position of the line if the handles are moved
            EditorGUI.BeginChangeCheck();
            p0 = Handles.DoPositionHandle(p0, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                // We record the previous state of the object to allow the user to undo the operation
                Undo.RecordObject(line, "Move Point");
                // We set the editor dirty, to warn Unity that a unsaved change was made
                EditorUtility.SetDirty(line);
                line.p0 = handleTransform.InverseTransformDirection(p0);
            }
            EditorGUI.BeginChangeCheck();
            p1 = Handles.DoPositionHandle(p1, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(line, "Move Point");
                EditorUtility.SetDirty(line);
                line.p1 = handleTransform.InverseTransformDirection(p1);
            }
        }
    }
}

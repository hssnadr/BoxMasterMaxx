// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CRI.HitBox.Arduino;

namespace CRI.HitBox.Game
{
    public class TargetController : MonoBehaviour
    {
        public delegate void TargetControllerEvent(int playerIndex);
        public static event TargetControllerEvent onSuccessfulHit;
        private int _playerIndex;

        private int _minPoints;
        private int _maxPoints;
        private float _tolerance;
        private float _targetCooldown;

        public int playerIndex
        {
            get { return _playerIndex; }
            set
            {
                foreach (var target in _targets)
                    target.playerIndex = value;
                foreach (Transform child in transform)
                    child.gameObject.layer = 8 + value;
                _playerIndex = value;
                gameObject.layer = 8 + value;
            }
        }

        public Camera playerCamera;

        [SerializeField]
        private RandomTarget[] _targets;

        private void OnEnable()
        {
            ImpactPointControl.onImpact += OnImpact;
        }

        private void OnDisable()
        {
            ImpactPointControl.onImpact -= OnImpact;
        }

        private void OnImpact(Vector2 position, int playerIndex)
        {
            if (this.playerIndex == playerIndex)
            {
                // Layer mask of the player
                int layerMask = 1 << (8 + playerIndex);
                Vector3 cameraForward = playerCamera.transform.forward;
                Debug.DrawRay(position, cameraForward * 5000.0f, Color.yellow, 10.0f);
                var hits = Physics.RaycastAll(position, cameraForward, Mathf.Infinity, layerMask);
                if (hits.Any(x => x.collider.GetComponent<RandomTarget>() != null
                && _targets.Contains(x.collider.GetComponent<RandomTarget>())))
                {
                    var hitTargets = hits
                        .Where(
                            x => x.collider.GetComponent<RandomTarget>() != null
                            && _targets.Contains(x.collider.GetComponent<RandomTarget>())
                            )
                        .OrderBy(
                            x => x.transform.position.z * cameraForward.z
                        );
                    var first = hitTargets.First();


                    foreach (var hitTarget in hitTargets)
                    {
                        if (hitTarget.collider.GetComponent<RandomTarget>().activated)
                        {
                            ScoreUp(hitTarget);
                            break;
                        }
                    }
                    bool direction = (first.collider.transform.position.z - first.collider.GetComponent<RandomTarget>().zPosition) * cameraForward.z >= 0;
                    GetComponentInParent<MovementController>().OnHit(direction ? cameraForward : -cameraForward, first);
                }
            }
        }

        private void ScoreUp(RaycastHit hit)
        {
            Vector2 center = hit.collider.bounds.center;
            Vector2 hitPoint = hit.point;
            float distance = Vector2.Distance(hitPoint, center);
            float maxDistance = hit.collider.bounds.extents.x;
            float minDistance = maxDistance * _tolerance;
            int score = (int)Mathf.Clamp(_maxPoints * (maxDistance - distance) / (maxDistance - minDistance), _minPoints, _maxPoints);
            Debug.Log(_maxPoints * (maxDistance - distance) / (maxDistance - minDistance));
            Debug.Log(score);
            GameManager.instance.ScoreUp(score);
            hit.collider.GetComponent<RandomTarget>().Hit();
            onSuccessfulHit(playerIndex);
        }

        public void Init(int playerIndex,
            Camera playerCamera,
            int minPoints,
            int maxPoints, 
            float tolerance,
            float targetCooldown,
            int activationTakeCount)
        {
            this.playerIndex = playerIndex;
            this.playerCamera = playerCamera;
            _minPoints = minPoints;
            _maxPoints = maxPoints;
            _tolerance = tolerance;
            _targetCooldown = targetCooldown;

            Activate(activationTakeCount);
        }

#if UNITY_EDITOR
        private void OnMouseDown()
        {
            if (GetComponentInParent<MovementController>().mousePlayerIndex == playerIndex)
            {
                Vector3 mousePosition = Input.mousePosition;
                if (!playerCamera.orthographic)
                    mousePosition.z = transform.position.z;
                OnImpact(playerCamera.ScreenToWorldPoint(mousePosition), playerIndex);
            }
        }
#endif

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnMouseDown();
        }
#endif

        private void Awake()
        {
            _targets = GetComponentsInChildren<RandomTarget>(true);
        }

        public void Activate(int takeCount = 1)
        {
            var random = new System.Random();
            RandomTarget[] targetsToActivate = _targets.Where(x => x.isActiveAndEnabled && !x.activated && Time.time - x.lastHit > _targetCooldown).OrderBy(i => random.Next()).Take(takeCount).ToArray();
            if (targetsToActivate.Length < takeCount)
                targetsToActivate = _targets.Where(x => x.isActiveAndEnabled && !x.activated).OrderBy(i => random.Next()).Take(takeCount).ToArray();
            foreach (var target in targetsToActivate)
                target.activated = true;
        }
    }
}

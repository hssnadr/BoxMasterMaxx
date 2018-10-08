using System;
using UnityEngine;

namespace CRI.HitBox.Game
{
    /// <summary>
    /// Manages the animator of the movement controller
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MovementControllerAnimator : MonoBehaviour
    {
        private Vector3 _leftMostPosition;

        private Vector3 _extents;

        private float _speed = 0.0f;

        private bool _move = false;

        private float _moveTime;

        private Animator _animator;

        private void Start()
        {
            var mainCamera = ApplicationManager.instance.GetCamera(0);
            _extents = mainCamera.bounds.extents;
            _speed = ApplicationManager.instance.gameSettings.targetHorizontalMovementSpeed;
            _animator = GetComponent<Animator>();
            _leftMostPosition = new Vector3(-mainCamera.bounds.extents.x + transform.lossyScale.x * 2,
                transform.position.y,
                transform.position.z);
        }

        private void Update()
        {
            int sphereNumber = SphereNumber(ApplicationManager.instance.gameManager.successfulHitCount,
                ApplicationManager.instance.gameSettings.targetCountThreshold,
                ApplicationManager.instance.gameMode);
            _animator.SetInteger("SphereCount", sphereNumber);
            _animator.SetBool("1P", ApplicationManager.instance.gameMode == GameMode.P1);
            if (sphereNumber > 4 && !_move)
            {
                _move = true;
                _moveTime = Time.time;
            }
            if (_move)
            {
                transform.position = new Vector3(
                    _leftMostPosition.x + Mathf.PingPong((Time.time - _moveTime) * _speed + _extents.x - transform.lossyScale.x * 2, _extents.x * 2 - transform.lossyScale.x * 4),
                    transform.position.y,
                    transform.position.z
                    );
            }
        }

        private int SphereNumber(int sucessfulHitCount, int[] targetCountThreshold, GameMode mode)
        {
            for (int i = targetCountThreshold.Length - 1; i >= 0; i--)
            {
                if ((mode == GameMode.P2
                    && sucessfulHitCount >= targetCountThreshold[i])
                    || (mode == GameMode.P1 && sucessfulHitCount * 2 >= targetCountThreshold[i]))
                    return i + 2;
            }
            return 0;
        }
    }
}
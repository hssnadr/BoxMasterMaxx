// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CRI.HitBox.Game
{
    public class MovementController : MonoBehaviour
    {
        /// <summary>
        /// The rotation speed of the movement controller when hit by a force.
        /// </summary>
        [Tooltip("The rotation speed of the movement controller when hit by a force.")]
        [SerializeField]
        private float _rotationSpeed = 50.0f;
        /// <summary>
        /// Z Rotation speed of the movement controller.
        /// </summary>
        [Tooltip("Z Rotation speed of the movement controller.")]
        [SerializeField]
        private float _zRotationSpeed = 20.0f;
        /// <summary>
        /// The max angular velocity of the rigidbody.
        /// </summary>
        [Tooltip("The max angular velocity of the rigidbody.")]
        [SerializeField]
        private float maxAngularVelocity = 3.0f;
#if UNITY_EDITOR
        /// <summary>
        /// (EDITOR-ONLY) The index of the player when a mouse click occurs.
        /// </summary>
        [Tooltip("(EDITOR-ONLY) The index of the player when a mouse click occurs.")]
        [SerializeField]
        private int _mousePlayerIndex = 0;

        //private Vector3 leftMostPosition;

        //private Vector3 rightMostPosition;

        public int mousePlayerIndex
        {
            get
            {
                return _mousePlayerIndex;
            }
        }
#endif

        private void Start()
        {
            _rotationSpeed = ApplicationManager.instance.gameSettings.targetRotationSpeed;
            _zRotationSpeed = ApplicationManager.instance.gameSettings.targetZRotationSpeed;
            maxAngularVelocity = ApplicationManager.instance.gameSettings.targetMaxAngularVelocity;
            /*
            var mainCamera = GameManager.instance.GetCamera(0);
            leftMostPosition = new Vector3(-mainCamera.bounds.extents.x + transform.lossyScale.x,
                transform.position.y,
                transform.position.z);
            rightMostPosition = new Vector3(mainCamera.bounds.extents.x - transform.lossyScale.x,
                transform.position.y,
                transform.position.z);
                */
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVelocity;
            transform.RotateAround(transform.position, Vector3.forward, _zRotationSpeed * Time.fixedDeltaTime);
        }
        
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.A))
                _mousePlayerIndex = 0;
            if (Input.GetKeyUp(KeyCode.Z))
                _mousePlayerIndex = 1;
#endif
        }


        public void OnHit(Vector3 cameraForward, RaycastHit hit)
        {
            GetComponent<Rigidbody>().AddForceAtPosition(cameraForward * _rotationSpeed, hit.point, ForceMode.Impulse);
        }
    }
}
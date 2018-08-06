// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ImpactPointControl : MonoBehaviour
{
    public delegate void ImpactPointControlEvent(Vector2 position, int playerIndex);
    public static event ImpactPointControlEvent onImpact;

    private Vector3 _acceleration;
    private GameObject[] _pointGrid;

    // Point of impact
    private float _xG = 0f;            // X coordinate of current impact
    private float _yG = 0f;            // X coordinate of current impact
    private float _totG = 0;           // total pressure of current impact

    private float _oldXG = -666;
    private float _oldYG = -666;

    public float threshImpact = 20;     // min value to detect impact
    public int delayOffHit = 50;      // minimum time (in ms) between 2 impacts to be validated (minimum 50ms <=> maximum 50 hits/s)
    private float _timerOffHit0 = 0;     // time of the last valid impact
    [SerializeField]
    private Vector3 _position;

    public Vector3 position { get { return _position; } }

    public int playerIndex = 0;

    private int _countHit = 0;          // number of hit

    private void Start()
    {
        _acceleration = GameManager.instance.arduinoSerials[playerIndex].GetComponent<ArduinoTouchSurface>().acceleration;
        _pointGrid = GameObject.FindGameObjectsWithTag("datapoint").Where(x => x.GetComponent<DatapointControl>().playerIndex == playerIndex).ToArray();
    }

    private void Update()
    {
        // Get instant center of pressure
        float totG_ = 0.0f;   // instant total pressure
        float xG_ = 0f;       // instant X coordinate of center of pressure
        float yG_ = 0f;       // instant Y coordinate of center of pressure;
        
        foreach (GameObject datapoint in _pointGrid)
        {
            if (datapoint.GetComponent<DatapointControl>().curDerivVal > this.threshImpact)
            {
                /////////////////////////////////////////////////////////////////////////////////////
                /// /////////////////////////////////////////////////////////////////////////////////////
                datapoint.GetComponent<DatapointControl>().threshImpact = (int)this.threshImpact;   // TO REMOVE
                                                                                                /////////////////////////////////////////////////////////////////////////////////////
                                                                                                /// /////////////////////////////////////////////////////////////////////////////////////

                totG_ += datapoint.GetComponent<DatapointControl>().curRemapVal;
                xG_ += datapoint.GetComponent<DatapointControl>().curRemapVal * datapoint.transform.position.x;
                yG_ += datapoint.GetComponent<DatapointControl>().curRemapVal * datapoint.transform.position.y;
                _timerOffHit0 = Time.time;
            }
        }

        // Get current impact
        if (1000 * (Time.time - _timerOffHit0) > this.delayOffHit && _totG != 0f)
        {
            // Get current impact positon
            _xG /= _totG;   // get X coordinate of current impact
            _yG /= _totG;   // get Y coordinate of current impact

            // Set impact point object at new position
            //this.gameObject.transform.position = new Vector3(xG, yG,0);
            //this.gameObject.transform.position += this.acceleration;  // get instant acceleration and shift pressure center

            _position = new Vector3(_xG, _yG, 0);
            onImpact(_position, playerIndex);

            //-------------------------
            //-------------------------
            // TODO TO CHANGE
            //GameObject.FindGameObjectWithTag ("target").GetComponent<TargetControl>().setForce(new Vector3(0,0,500), this.gameObject.transform.position) ;
            //-------------------------
            //-------------------------

            _oldXG = _xG;
            _oldYG = _yG;
            _xG = 0;     // reset X coordinate of current impact
            _yG = 0;     // reset Y coordinate of current impact
            _totG = 0;   // reset pressure of current impact

            _countHit++;   // increment number of hit
        }
        else
        {
            _xG += xG_;
            _yG += yG_;
            _totG += totG_;
        }
    }
}

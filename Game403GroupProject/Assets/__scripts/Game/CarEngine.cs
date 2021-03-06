﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEngine : MonoBehaviour {
    public Transform path;
    private float maxSteerAngle = 80;
    public WheelCollider wheelFL;
    public WheelCollider wheelFR;
    public WheelCollider wheelRL;
    public WheelCollider wheelRR;

    private List<Transform> points = new List<Transform>();
    private int currentPoint = 0;
	// Use this for initialization
	void Start () {
        Transform[] pointTransform = path.GetComponentsInChildren<Transform>();
        points = new List<Transform>();
        for(int i = 0;i < pointTransform.Length; i++)
        {
            if (pointTransform[i] != path.transform)
            {
                points.Add(pointTransform[i]);
            }
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        ApplySteer();
        Drive();
        CheckWayPointDistance();
	}

    private void CheckWayPointDistance()
    {
        if(Vector3.Distance(transform.position, points[currentPoint].position) < 5f)
        {
            if(currentPoint == points.Count - 1)
            {
                currentPoint = 0;
            }
            else
            {
                currentPoint++;
            }
        }
    }
    private void Drive()
    {
        wheelFL.motorTorque = 120;
        wheelFR.motorTorque = 120;
    }
    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(points[currentPoint].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        wheelFL.steerAngle = newSteer;
        wheelFR.steerAngle = newSteer;
        if(wheelFL.steerAngle != 0f || wheelFR.steerAngle != 0f)
        {
            ApplyBrakes();
        }
    }

    private void ApplyBrakes()
    {
        wheelFL.motorTorque = -120;
        wheelFR.motorTorque = -120;
    }
}

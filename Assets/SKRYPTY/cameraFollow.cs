﻿using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target;
    // public GameObject gameObjTarget;
    public float distance = 3.0f;
    public float height = 3.0f;
    public float damping = 5.0f;
    public bool smoothRotation = true;
    public bool followBehind = true;
    public float rotationDamping = 10.0f;


    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("CAMERA_FOLLOW_POINT").transform;
    }
    void Update()
    {
        Vector3 wantedPosition;
        if (followBehind)
           
            wantedPosition = target.TransformPoint(0, height, -distance);
        else
            wantedPosition = target.TransformPoint(0, height, distance);

        transform.position = Vector3.Lerp(transform.position, wantedPosition, Time.deltaTime * damping);

        if (smoothRotation)
        {
            Quaternion wantedRotation = Quaternion.LookRotation(target.position - transform.position, target.up);
            //Quaternion ownRotation = Quaternion.RotateTowards;
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
        }
        else transform.LookAt(target, target.up);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    private GameObject joystick;
    private Vector2 dir;

    UnityEngine.AI.NavMeshAgent agent;

    Animator anim;
    Vector2 smoothDeltaPosition = Vector2.zero;

    RaycastHit hitInfo = new RaycastHit();

    Vector2 velocity = Vector2.zero;
    bool shouldMove;


    void Start()
    {
        joystick = GameObject.FindGameObjectWithTag("Joystick");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.updatePosition = false;
    }

    private void Update()
    {
        if (Input.GetKey("space"))
        {
            anim.SetBool("swordSlash", true);
        }
        if (anim.GetCurrentAnimatorStateInfo(1).IsName("swordSlash"))
        {
            anim.SetBool("swordSlash", false);
        }

        dir = joystick.GetComponent<Joystick>().direction;

        agent.speed = Mathf.Clamp(dir.magnitude, 0, 30) / 8;
        agent.destination = transform.position + new Vector3(dir.x, 0, dir.y).normalized / 5;
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        if (dir == Vector2.zero)
        {
            anim.SetLayerWeight(1, 0);
            anim.SetLayerWeight(2, 1);
            shouldMove = false;
        }
        else
        {
            shouldMove = true;
            anim.SetLayerWeight(2, 0);
            anim.SetLayerWeight(1, 0.5f);
        }


        // Update animation parameters
        anim.SetBool("shouldMove", shouldMove);
        anim.SetFloat("velX", velocity.x);
        anim.SetFloat("velY", velocity.y);

        GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
    }

    void FixedUpdate()
    {


    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }

    void OnAnimatorMove()
    {
        // Update position to agent position
        transform.position = agent.nextPosition;
        anim.ApplyBuiltinRootMotion();
    }
}

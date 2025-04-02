using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy_Bat : Enemy
{
    [Header("Bat details")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private float agrroRadius = 7;
    [SerializeField] private float chaseDuration = 1;

    private float defaultSpeed;
    private float chaseTimer;

    private Vector3 originalPosition;
    private Vector3 destination;

    private bool candDetectPlayer;
    private Collider2D target;

    protected override void Awake()
    {
        base.Awake();

        defaultSpeed = moveSpeed;
        originalPosition = transform.position;
        canMove = false;
    }

    protected override void Update()
    {
        base.Update();

        chaseTimer -= Time.deltaTime;

        if (idleTimer < 0)
            candDetectPlayer = true;

        HandleMovement();
        HandlePlayerDetection();
    }

    private void HandleMovement()
    {
        if (canMove == false)
            return;

        HandleFlip(destination.x);
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (chaseTimer > 0 && target != null)
            destination = target.transform.position;
        else
            moveSpeed = attackSpeed;

        if (Vector2.Distance(transform.position, destination) < .1f)
        {
            if (destination == originalPosition)
            {
                idleTimer = idleDuration;
                candDetectPlayer = false;
                canMove = false;
                anim.SetBool("isMoving", false);
                target = null;
                moveSpeed = defaultSpeed;
            }
            else
            {
                destination = originalPosition;
            }
        }

    }

    private void HandlePlayerDetection()
    {
        if (target == null && candDetectPlayer)
        {
            target = Physics2D.OverlapCircle(transform.position, agrroRadius, whatIsPlayer);

            if (target != null)
            {
                chaseTimer = chaseDuration;
                destination = target.transform.position;
                candDetectPlayer = false;
                anim.SetBool("isMoving", true);
            }
        }
    }

    private void AllowMovement() => canMove = true;

    protected override void HandleAnimator()
    {
        
    }

    public override void Die()
    {
        base.Die();

        canMove = false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, agrroRadius);
    }


}

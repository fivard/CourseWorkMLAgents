using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float ballSpeed;
    private float randomizedSpeed = 0f;
    private float nextActionTime = -1f;
    private Vector3 targetPosition;

    private void FixedUpdate()
    {
        if (ballSpeed > 0f)
        {
            Swim();
        }
    }

    private void Swim()
    {
        if (Time.fixedTime >= nextActionTime)
        {
            randomizedSpeed = ballSpeed * UnityEngine.Random.Range(.5f, 1.5f);
            targetPosition = HumanArea.ChooseRandomPosition(transform.parent.position, 100f, 260f, 2f, 13f);
            transform.rotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
            float timeToGetThere = Vector3.Distance(transform.position, targetPosition) / randomizedSpeed;
            nextActionTime = Time.fixedTime + timeToGetThere;
        }
        else
        {
            Vector3 moveVector = randomizedSpeed * transform.forward * Time.fixedDeltaTime;
            if (moveVector.magnitude <= Vector3.Distance(transform.position, targetPosition))
            {
                transform.position += moveVector;
            }
            else
            {
                transform.position = targetPosition;
                nextActionTime = Time.fixedTime;
            }
        }
    }
}

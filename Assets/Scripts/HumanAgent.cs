using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class HumanAgent : Agent
{
    public GameObject heartPrefab;
    private HumanArea humanArea;
    private Animator animator;
    private RayPerception3D rayPerception;
    private GameObject baby;

    private bool isFull;

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float forwardOrBackward = 0f;
        float leftOrRight = 0f;

        if (vectorAction[0] == 1f)
        {
            forwardOrBackward = 1f;
        } 
        else if (vectorAction[0] == 2f)
        {
            forwardOrBackward = -1f;
        }

        if (vectorAction[1] == 1f)
        {
            leftOrRight = -1f;
        }
        else if (vectorAction[1] == 2f)
        {
            leftOrRight = 1f;
        }

        animator.SetFloat("Vertical", forwardOrBackward);
        animator.SetFloat("Horizontal", leftOrRight);

        AddReward(-1f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        isFull = false;
        humanArea.ResetArea();
    }

    public override void CollectObservations()
    {
        AddVectorObs(isFull);
        AddVectorObs(Vector3.Distance(baby.transform.position, transform.position));
        AddVectorObs((baby.transform.position - transform.position).normalized);
        AddVectorObs(transform.forward);
        float rayDistance = 20f;
        float[] rayAngles = new float[36];
        for (int i = 0; i < 36; i++)
        {
            rayAngles[i] = 10f + 10f * i;
        }
        string[] detectableObjects = { "baby", "ball", "wall" };
        AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
    }

    private void Start()
    {
        humanArea = GetComponentInParent<HumanArea>();
        baby = humanArea.humanBaby;
        animator = GetComponent<Animator>();
        rayPerception = GetComponent<RayPerception3D>();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, baby.transform.position) < humanArea.feedRadius)
        {
            RegurgitateBall();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ball"))
        {
            EatBall(collision.gameObject);
        }
        else if (collision.transform.CompareTag("baby"))
        {
            RegurgitateBall();
        }
    }

    private void EatBall(GameObject ballObject)
    {
        if (isFull) return; 
        isFull = true;

        humanArea.RemoveSpecificBall(ballObject);

        AddReward(1f);
    }

    private void RegurgitateBall()
    {
        if (!isFull) return;
        isFull = false;


        GameObject heart = Instantiate<GameObject>(heartPrefab);
        heart.transform.parent = transform.parent;
        heart.transform.position = baby.transform.position + Vector3.up*2f;
        Destroy(heart, 4f);

        AddReward(1f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;
using System;

public class HumanArea : Area
{
    public HumanAgent humanAgent;
    public GameObject humanBaby;
    public Ball ballPrefab;
    public TextMeshPro cumulativeRewardText;

    [HideInInspector]
    public float ballSpeed = 0f;
    [HideInInspector]
    public float feedRadius = 1f;

    private List<GameObject> ballList;

    public override void ResetArea()
    {
        RemoveAllBall();
        PlaceHuman();
        PlaceBaby();
        SpawnBall(4, ballSpeed);
    }

    public void RemoveSpecificBall(GameObject ballObject)
    {
        ballList.Remove(ballObject);
        Destroy(ballObject);
    }

    public static Vector3 ChooseRandomPosition(Vector3 center, float minAngle, float maxAngle, float minRadius, float maxRadius)
    {
        float radius = minRadius;

        if (maxRadius > minRadius)
        {
            radius = UnityEngine.Random.Range(minRadius, maxRadius);
        }

        return center + Quaternion.Euler(0f, UnityEngine.Random.Range(minAngle, maxAngle), 0f) * Vector3.forward * radius;
    }

    private void RemoveAllBall()
    {
        if (ballList != null)
        {
            for (int i = 0; i < ballList.Count; i++)
            {
                if (ballList[i] != null)
                {
                    Destroy(ballList[i]);
                }
            }
        }

        ballList = new List<GameObject>();
    }

    private void PlaceHuman()
    {
        humanAgent.transform.position = ChooseRandomPosition(transform.position, 0f, 360f, 0f, 9f) + Vector3.up * .5f;
    }

    private void PlaceBaby()
    {
        humanBaby.transform.position = ChooseRandomPosition(transform.position, -45f, 45f, 3f, 9f) + Vector3.up * .5f;
    }

    private void SpawnBall(int count, float ballSpeed)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject ballObject = Instantiate<GameObject>(ballPrefab.gameObject);
            ballObject.transform.position = ChooseRandomPosition(transform.position, 100f, 260f, 2f, 13f) + Vector3.up * .5f;
            ballObject.transform.rotation = Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
            ballObject.transform.parent = transform;
            ballList.Add(ballObject);
            ballObject.GetComponent<Ball>().ballSpeed = ballSpeed;
        }
    }

    private void Update()
    {
        cumulativeRewardText.text = humanAgent.GetCumulativeReward().ToString("0.00");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class HumanAcademy : Academy
{
    private HumanArea[] humanAreas;

    public override void AcademyReset()
    {
        if (humanAreas == null)
        {
            humanAreas = FindObjectsOfType<HumanArea>();
        }

        foreach (HumanArea humanArea in humanAreas)
        {
            humanArea.ballSpeed = resetParameters["ball_speed"];
            humanArea.feedRadius = resetParameters["feed_radius"];
            humanArea.ResetArea();
        }
    }
}

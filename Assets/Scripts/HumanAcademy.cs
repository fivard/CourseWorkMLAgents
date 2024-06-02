using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class HumanAcademy : Academy
{
    private HumanArea[] humanAreas;

    public override void AcademyReset()
    {
        // Get the human areas
        if (humanAreas == null)
        {
            humanAreas = FindObjectsOfType<HumanArea>();
        }

        // Set up areas
        foreach (HumanArea humanArea in humanAreas)
        {
            humanArea.fishSpeed = resetParameters["fish_speed"];
            humanArea.feedRadius = resetParameters["feed_radius"];
            humanArea.ResetArea();
        }
    }
}

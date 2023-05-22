using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWithTimer : Building
{
    private void Update()
    {
        if (!current.Placed)
        {
            TimerTooltip.HideTimer_Static();
        }
    }
    public override void Place()
    {
        base.Place();
        Timer timer = gameObject.AddComponent<Timer>();
        timer.Initialize("Building", DateTime.Now, TimeSpan.FromMinutes(3));
        timer.StartTimer();
        timer.TimerFinishedEvent.AddListener(delegate
        {
            Destroy(timer);
        });
    }
    private void OnMouseUpAsButton()
    {
        if (Placed && GridBuildingSystem.current.temp == null)
        {
            TimerTooltip.ShowTimer_Static(current.gameObject);
        }
        if (GridBuildingSystem.current.temp == null)
        {
            GridBuildingSystem.current.buildingPanel.enabled = true;
            GridBuildingSystem.current.temp = current;
        }
    }
}


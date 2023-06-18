using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Egg", menuName = "GameObjects/StorageItems/Egg")]

public class Egg : CollectibleItem
{
    public String id = GenerateUUID();
    public Dictionary<CollectibleItem, int> ItemsNeeded;
    public TimePeriod hatchTime;
    public TimeSpan productionTime;

    public int Strength = 1;
    public int Growth = 1;
    public int Gain = 1;

    public Chicken hatchedChicken;

    [System.Serializable]
    public struct TimePeriod
    {
        public int Days;
        public int Hours;
        public int Minutes;
        public int Seconds;
    }

    protected void OnValidate()
    {
        ItemsNeeded = new Dictionary<CollectibleItem, int>() { { this, 1 } };
        productionTime = new TimeSpan(hatchTime.Days, hatchTime.Hours, hatchTime.Minutes, hatchTime.Seconds);
    }
    public static string GenerateUUID()
    {
        Guid guid = Guid.NewGuid();
        string uuid = guid.ToString();
        uuid = uuid.Replace("-", "").ToLower();
        return uuid;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chicken", menuName = "GameObjects/StorageItems/Chicken")]
public class Chicken : CollectibleItem
{
    public int Strength;
    public int Growth;
    public int Gain;

    [SerializeField] public List<CollectibleItem> drop;

    public Dictionary<CollectibleItem, int> thatChicken;
    protected void OnValidate()
    {
        thatChicken = new Dictionary<CollectibleItem, int>() { { this, 1 } };
    }
}

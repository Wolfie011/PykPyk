using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenNest : Building, ISource
{
    public State currentState { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Collect()
    {
        throw new System.NotImplementedException();
    }

    public void Produce(Dictionary<CollectibleItem, int> itemsNeeded, CollectibleItem itemToProduce)
    {
        throw new System.NotImplementedException();
    }
}

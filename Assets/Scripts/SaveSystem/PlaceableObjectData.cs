using System;
using UnityEngine;

[Serializable]
public class PlaceableObjectData : Data
{
    //ShopItem name to load it from the resources
    public string assetName;
    //position of the object on the map
    public Vector3 position;
}

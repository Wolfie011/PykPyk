using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class SaveData
{
    //id count for generating a new id
    public static int IdCount;

    //all the placeable objects on the map
    public Dictionary<string, PlaceableObjectData> placeableObjectDatas = 
        new Dictionary<string, PlaceableObjectData>();
    
    public static string GenerateId()
    {
        //increase id
        IdCount++;
        //return it as string
        return IdCount.ToString();
    }

    public void AddData(Data data)
    {
        //check the type of data
        if (data is PlaceableObjectData plObjData)
        {
            //if it is already in the dictionary
            if (placeableObjectDatas.ContainsKey(plObjData.ID))
            {
                //update the information
                placeableObjectDatas[plObjData.ID] = plObjData;
            }
            else
            {
                //add a new object to save
                placeableObjectDatas.Add(plObjData.ID, plObjData);
            }
        }
    }

    public void RemoveData(Data data)
    {
        //check the type of data
        if (data is PlaceableObjectData plObjData)
        {
            //check if it is in the dictionary
            if (placeableObjectDatas.ContainsKey(plObjData.ID))
            {
                //remove it from the dictionary
                placeableObjectDatas.Remove(plObjData.ID);
            }
        }
    }

    //this method called when deserializing the object
    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        //in case the data dictionary is null, create it
        placeableObjectDatas ??= new Dictionary<string, PlaceableObjectData>();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    public static PlacableObject current;
    public bool Placed { get; private set; }
    private Vector3 origin;

    public BoundsInt area;

    //private string UUID = null;

    private void Awake()
    {
        current = this;
    }
    public bool CanBePlaced()
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        return BuildingSystem.current.CanTakeArea(areaTemp);
    }
    public virtual void Place()
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        //UUID = GenerateUUID();
        areaTemp.position = positionInt;

        Placed = true;
        origin = transform.position;

        BuildingSystem.current.TakeArea(areaTemp);

        PanZoom.current.UnFollowObject();
    }
    public void CheckPlacement()
    {
        if (!Placed)
        {
            if (CanBePlaced())
            {
                Place();
                origin = transform.position;
            }
            else
            {
                Destroy(transform.gameObject);
            }
            ShopManager.current.ShopButton_Click();
        }
        else
        {
            if (CanBePlaced())
            {
                Place();
                origin = transform.position;
            }
            else
            {
                transform.position = origin;
                Place();
            }
        }
    }
    /*public static string GenerateUUID()
    {
        // Generate a random GUID
        Guid guid = Guid.NewGuid();

        // Convert the GUID to a string representation
        string uuid = guid.ToString();

        // Remove hyphens and convert to lowercase
        uuid = uuid.Replace("-", "").ToLower();

        return uuid;
    }


    private void OnMouseDown()
    {
        //Debug.Log(gameObject.transform.GetComponent<PlacableObject>().UUID);
    }*/

    private bool touching = false;
    private float touchTime = 0f;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touching = true;
            touchTime = 0f;
        }
        else if (Input.GetMouseButton(0))
        {
            if (touching)
            {
                touchTime += Time.deltaTime;

                if (touchTime > 3.0f)
                {
                    if (current.gameObject.GetComponent<ObjectDrag>()) { return; }
                    touching = true;
                    current.gameObject.AddComponent<ObjectDrag>();
                    PanZoom.current.FollowObject(current.gameObject.transform);

                    Vector3Int positionInt = BuildingSystem.current.gridLayout.WorldToCell(current.transform.position);
                    BoundsInt areaTemp = current.area;
                    areaTemp.position = positionInt;

                    BuildingSystem.current.ClearArea(areaTemp, BuildingSystem.current.MainTilemap);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touching = false;
        }
    }
}
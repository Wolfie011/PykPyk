using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    private static PlacableObject instance;
    public bool Placed { get; private set; }
    public BoundsInt area;
    private Vector3 origin;

    private void Awake()
    {
        instance = this;
    }

    public bool CanBePlaced()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        return GridBuildingSystem.current.CanTakeArea(areaTemp);
    }
    public void Place()
    {
        Vector3Int positionInt = GridBuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        Placed = true;
        GridBuildingSystem.current.TakeArea(areaTemp);
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

    private bool touching;
    private float touchTime = 0f;
    public void Update()
    {
        if(!touching && Placed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchTime = 0f;
            }
            else if (Input.GetMouseButton(0))
            {
                touchTime += Time.deltaTime;

                if (touchTime > 1.5f)
                {
                    if (gameObject.GetComponent<ObjectDrag>()) { return; }
                    touching = true;
                    gameObject.AddComponent<ObjectDrag>();
                    PanZoom.current.FollowObject(gameObject.transform);

                    //GridBuildingSystem.current.FollowBuilding();

                    Vector3Int positionInt = GridBuildingSystem.current.gridLayout.WorldToCell(transform.position);
                    BoundsInt areaTemp = area;
                    areaTemp.position = positionInt;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                touching = false;
            }
        }
    }
}

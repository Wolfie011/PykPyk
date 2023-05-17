using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    private PlacableObject current;
    private Vector3 origin;
    public BoundsInt area;

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
    public void Place()
    {
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
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

    private void OnMouseDown()
    {
        Debug.Log(gameObject.name);
        clickedUUID = gameObject.name;

    }
    private string clickedUUID;
    private float touchTime = 0f;
    private bool touching;

    private void Update()
    {
        if (!touching && Placed && gameObject.CompareTag("Placable"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchTime = 0;
            }
            else if (Input.GetMouseButton(0))
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
        if (touching && Input.GetMouseButtonUp(0))
        {
            touching = false;
        }
    }
}
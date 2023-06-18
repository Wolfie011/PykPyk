using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    private PlaceableObject current;
    [SerializeField] private String name;
    private ShopItem item;
    //if the building is placed or not
    public bool Placed { get; private set; }
    //position on which an object was placed
    //(save it if the new position is not available)
    private Vector3 origin;
    
    //area under the house - stores position and building size
    public BoundsInt area;

    [ReadOnly] public PlaceableObjectData data = new PlaceableObjectData();
    
    private void Awake()
    {
        PanZoom.current.FollowObject(transform);
        current = this;
    }

    //Initialization method used for first placement (it wasn't saved before)
    public void Initialize(ShopItem shopItem)
    {
        item = shopItem;
        data.assetName = item.Name;
        data.ID = SaveData.GenerateId();
    }

    //Initialization method used for loading the object after it's been saved
    public void Initialize(ShopItem shopItem, PlaceableObjectData objectData)
    {
        item = shopItem;
        data = objectData;
    }

    public void Load()
    {
        PanZoom.current.UnfollowObject();
        Destroy(GetComponent<ObjectDrag>());
        Place();
    }
    
    /*
     * Check if the building can be placed at its current position
     */
    public bool CanBePlaced()
    {
        //create an area under the building
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;

        //call the GridBuildingSystem to check the area
        return BuildingSystem.current.CanTakeArea(areaTemp);
    }
    
    /*
     * Make the building placed
     */
    public virtual void Place()
    {
        //create an area under the building
        Vector3Int positionInt = BuildingSystem.current.gridLayout.LocalToCell(transform.position);
        BoundsInt areaTemp = area;
        areaTemp.position = positionInt;
        //Build event
        EventManager.Instance.QueueEvent(new BuildingGameEvent(name));

        //set the bool
        Placed = true;
        //save position
        origin = transform.position;
        
        //call the system to 
        BuildingSystem.current.TakeArea(areaTemp);
        
        //updating the position of the building for saving
        data.position = transform.position;
        //Adding the data to the main save object
        GameManager.current.saveData.AddData(data);
    }

    public void CheckPlacement()
    {
        PanZoom.current.UnfollowObject();
        
        //object is new an haven't been placed before
        if (!Placed)
        {
            //if it can be placed
            if (CanBePlaced())
            {
                Place();
            }
            else
            {
                //destroy this object (because it is new)
                Destroy(transform.gameObject);
            }
        
            //open the shop afterwards
            ShopManager.current.ShopButton_Click();
        }
        //editing the map, object has been placed before
        else
        {
            //if cannot be placed
            if (!CanBePlaced())
            {
                //reset the position to origin
                transform.position = origin;
            }
            
            Place();
        }
    }

    //time elapsed since the touch begun
    private float time = 0f;
    private bool touching;
    private bool moving;

    private void Update()
    {
        if(touching && Input.GetMouseButton(0))
        {
            //increase time elapsed
            time += Time.deltaTime;

            //time limit exceeded
            if (time > 3f)
            {
                touching = false;
                moving = true;
                //add component to drag
                gameObject.AddComponent<ObjectDrag>();

                //prepare area
                Vector3Int positionInt = BuildingSystem.current.gridLayout.WorldToCell(transform.position);
                BoundsInt areaTemp = area;
                areaTemp.position = positionInt;

                //clear area on which the object was standing on
                BuildingSystem.current.ClearArea(areaTemp, BuildingSystem.current.MainTilemap);
            }
        }
    }

    private void OnMouseDown()
    {
        time = 0;
        touching = true;
    }

    protected virtual void OnClick() { }

    private void OnMouseUpAsButton()
    {
        if (moving)
        {
            moving = false;
            return;
        }
        
        OnClick();
    }
}

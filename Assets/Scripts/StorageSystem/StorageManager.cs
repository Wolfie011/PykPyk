using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    //singletone pattern
    public static StorageManager current;

    //prefabs for storage buildings
    [SerializeField] private GameObject barnPrefab;
    [SerializeField] private GameObject magazinePrefab;
    [SerializeField] private GameObject siloPrefab;
    
    //path to load collectible items from resources
    private string itemsPath = "Storage";
    //dictionaries to store different types of items
    public Dictionary<Chicken, int> chickens;
    public Dictionary<Crop, int> crops;
    //private Dictionary<Feed, int> feeds;
    //private Dictionary<Fruit, int> fruits;
    private Dictionary<Product, int> products;
    private Dictionary<Tool, int> tools;
    public Dictionary<Egg, int> eggs;
    
    //dictionary for barn items
    private Dictionary<CollectibleItem, int> barnItems;
    //dictionary for barn items
    private Dictionary<CollectibleItem, int> siloItems;
    //dictionary for magazine items
    private Dictionary<CollectibleItem, int> magazineItems;

    //storage buildings
    private StorageBuilding Barn;
    private StorageBuilding Silo;
    private StorageBuilding Magazine;

    private void Awake()
    {
        //initialize the singletone
        current = this;
        //load all items from resources
        Dictionary<CollectibleItem, int> itemsAmounts = LoadItems();
        //sort all items into different dictionaries
        Sort(itemsAmounts);
        
        //initialize the field with all the crops
        Field.Initialize(crops);
    }

    /*
     * Load collectible items from resources
     */
    private Dictionary<CollectibleItem, int> LoadItems()
    {
        //create a dictionary for all items
        Dictionary<CollectibleItem, int> itemAmounts = new Dictionary<CollectibleItem, int>();
        //load collectible items from resources
        CollectibleItem[] allItems = Resources.LoadAll<CollectibleItem>(itemsPath);

        for (int i = 0; i < allItems.Length; i++)
        {
            //check if the level is less or equal to the current
            if (allItems[i].Level >= LevelSystem.Level)
            {
                //todo remove 2 in a real game
                itemAmounts.Add(allItems[i], 0);
            }
        }

        //return dictionary with items
        return itemAmounts;
    }

    /*
     * Sort items into different categories
     */
    private void Sort(Dictionary<CollectibleItem, int> itemsAmounts)
    {
        //initialize dictionaries
        chickens = new Dictionary<Chicken, int>();
        crops = new Dictionary<Crop, int>();
        //feeds = new Dictionary<Feed, int>();
        //fruits = new Dictionary<Fruit, int>();
        products = new Dictionary<Product, int>();
        tools = new Dictionary<Tool, int>();
        eggs = new Dictionary<Egg, int>();

        siloItems = new Dictionary<CollectibleItem, int>();
        barnItems = new Dictionary<CollectibleItem, int>();
        magazineItems = new Dictionary<CollectibleItem, int>();


        //go through each item and determine the type
        foreach (var itemPair in itemsAmounts)
        {
            if (itemPair.Key is Chicken chicken)
            {
                //add item to the appropriate dictionaries
                chickens.Add(chicken, itemPair.Value);
                barnItems.Add(chicken, itemPair.Value);
            }
            else if (itemPair.Key is Crop crop)
            {
                crops.Add(crop, itemPair.Value);
                siloItems.Add(crop, itemPair.Value);   
            }
            else if (itemPair.Key is Product product)
            {
                products.Add(product, itemPair.Value);
                magazineItems.Add(product, itemPair.Value);
            }
            else if (itemPair.Key is Tool tool)
            {
                tools.Add(tool, itemPair.Value);
                magazineItems.Add(tool, itemPair.Value);
            }
            else if(itemPair.Key is Egg egg)
            {
                eggs.Add(egg, itemPair.Value);
                barnItems.Add(egg, itemPair.Value);
            }
        }
    }

    //put Barn and Silo on the map
    private void Start()
    {
        //instantiate the silo object
        GameObject siloObject = BuildingSystem.current.InitializeWithObject(siloPrefab, new Vector3(7.25f, -0.25f));
        //get the storage building component and save it
        Silo = siloObject.GetComponent<StorageBuilding>();
        //place the building onto the map
        Silo.Load();
        //initialize with items and a name
        Silo.Initialize(siloItems,"Silo");

        //instantiate the barn object
        GameObject barnObject = BuildingSystem.current.InitializeWithObject(barnPrefab, new Vector3(6f, -0.25f));
        //get the storage building component and save it
        Barn = barnObject.GetComponent<StorageBuilding>();
        //place the building onto the map
        Barn.Load();
        //initialize with items and a name
        Barn.Initialize(barnItems,"Barn");
        
        //instantiate the barn object
        GameObject magazineObject = BuildingSystem.current.InitializeWithObject(magazinePrefab, new Vector3(4f, 1f));
        //get the storage building component and save it
        Magazine = magazineObject.GetComponent<StorageBuilding>();
        //place the building onto the map
        Magazine.Load();
        //initialize with items and a name
        Magazine.Initialize(magazineItems,"Magazine");
    }

    /*
     * Get amount of an item that the player has
     */
    public int GetAmount(CollectibleItem item)
    {
        //initialize the amount with default value
        int amount = 0;
        //determine the type of an object requested
        if (item is Chicken chicken)
        {
            //try get the amount
            chickens.TryGetValue(chicken, out amount);
        }
        else if (item is Crop crop)
        {
            crops.TryGetValue(crop, out amount);
        }
        else if (item is Product product)
        {
            products.TryGetValue(product, out amount);
        }
        else if (item is Tool tool)
        {
            tools.TryGetValue(tool, out amount);
        }else if (item is Egg egg)
        {
            eggs.TryGetValue(egg, out amount);
        }

        //return the amount
        return amount;
    }

    /*
     * Check if the player has enough of an item
     * @returns true if the amount the player has is more or equal to the amount required
     */
    public bool IsEnoughOf(CollectibleItem item, int amount)
    {
        return GetAmount(item) >= amount;
    }

    public void UpdateItems(Dictionary<CollectibleItem, int> items, bool add)
    {
        //go through each item in the dictionary
        foreach (var itemPair in items)
        {
            //get the item
            var item = itemPair.Key;
            //get its amount
            var amount = itemPair.Value;

            //if we're not adding
            if (!add)
            {
                //make the amount negative
                amount = -amount;
            }

            //determine the type of the item
            if (item is Chicken chicken)
            {
                //add thee item amount to the appropriate dictionaries
                chickens[chicken] += amount;
                barnItems[item] += amount;
            }
            else if (item is Crop crop)
            {
                crops[crop] += amount;
                siloItems[item] += amount;
            }
            else if (item is Product product)
            {
                products[product] += amount;
                magazineItems[item] += amount;
            }
            else if (item is Tool tool)
            {
                tools[tool] += amount;
                magazineItems[item] += amount;
            }
            else if (item is Egg egg)
            {
                eggs[egg] += amount;
                barnItems[item] += amount;
            }
        }
    }
}

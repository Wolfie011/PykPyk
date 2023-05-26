using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singletone pattern
    public static GameManager current;

    //the main object to be saved into a file
    public SaveData saveData;
    //path for loading scriptable objects (ShopItems)
    [SerializeField] private string shopItemsPath = "Shop";

    //save the canvas
    public GameObject canvas;

    private void Awake()
    {
        current = this;

        ShopItemDrag.canvas = canvas.GetComponent<Canvas>();
        UIDrag.canvas = canvas.GetComponent<Canvas>();
    }
    private void Start()
    {
        //load the save data
        saveData = SaveSystem.Load();
        //load the game
        LoadGame();
    }

    private void LoadGame()
    {
        //load placeable objects (the map)
        LoadPlaceableObjects();
    }

    private void LoadPlaceableObjects()
    {
        //go through each saved data
        foreach (var plObjData in saveData.placeableObjectDatas.Values)
        {
            //try-catch block in case something wasn't saved properly
            //to avoid errors
            try
            {
                //get the ShopItem from resources
                ShopItem item = Resources.Load<ShopItem>(shopItemsPath + "/" + plObjData.assetName);
                //instantiate the prefab
                GameObject obj = BuildingSystem.current.InitializeWithObject(item.Prefab, plObjData.position, true);
                //get the placeable object component
                PlaceableObject plObj = obj.GetComponent<PlaceableObject>();
                //initialize the component
                plObj.Initialize(item, plObjData);
                //load to finalize placement
                plObj.Load();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
                //throw;
            }
        }
    }

    private void OnDisable()
    {
        //save the data before closing
        SaveSystem.Save(saveData);
    }

    public void GetXP(int amount)
    {
        XPAddedGameEvent info = new XPAddedGameEvent(amount);
        EventManager.Instance.QueueEvent(info);
    }
    public void GetCoins(int amount)
    {
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(amount, CurrencyType.Coins);
        EventManager.Instance.QueueEvent(info);
    }
    public void GetCrystals(int amount)
    {
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(amount, CurrencyType.Crystals);
        EventManager.Instance.QueueEvent(info);
    }
}

﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageUI : MonoBehaviour
{
    //action that is going to be invoked when 
    private Action increaseAction;
    
    //UI fields
    [SerializeField] private TextMeshProUGUI storageTypeText;
    [SerializeField] private TextMeshProUGUI maxItemsText;
    [SerializeField] private Slider maxItemsSlider;
    
    [SerializeField] private GameObject itemsView;
    [SerializeField] private GameObject increaseView;

    [SerializeField] private Transform itemsContent;
    [SerializeField] private Transform increaseContent;

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject chickenPrefab;
    [SerializeField] private GameObject eggPrefab;

    /*
     * Display the storage building name
     */
    public void SetNameText(string name)
    {
        storageTypeText.text = name;
    }

    /*
     * Initialize the UI
     */
    public void Initialize(int currentAmount, int maxAmount, Dictionary<CollectibleItem, int> itemAmounts,
                            Dictionary<CollectibleItem, int> tools, Action onIncrease)
    {
        //set the capacity text
        maxItemsText.text = currentAmount + "/" + maxAmount;
        //set the capacity slider value
        maxItemsSlider.value = (float) currentAmount / maxAmount;
        
        //initialize the items view
        InitializeItems(itemAmounts);
        //initialize the tools view
        InitializeTools(tools);

        //set the action
        increaseAction = onIncrease;
    }
    
    /*
     * Initialize the items view
     */
    private void InitializeItems(Dictionary<CollectibleItem, int> itemAmounts)
    {
        //if the window was initialized before -> clear
        int childCount = itemsContent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(itemsContent.GetChild(i).gameObject);
        }

        if (itemAmounts == null)
        {

        }
        else
        {
            //go through each item
            foreach (var itemPair in itemAmounts)
            {
                if(itemPair.Value == 0)
                {

                }
                else
                {
                    if (itemPair.Key is Chicken chicken)
                    {
                        for (int i = 1; i <= itemPair.Value; i++)
                        {
                            GameObject chickenHolder = Instantiate(chickenPrefab, itemsContent);
                            chickenHolder.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = chicken.Name;
                            chickenHolder.transform.Find("Icon").GetComponent<Image>().sprite = chicken.Icon;
                            chickenHolder.transform.Find("Stats").GetComponent<TextMeshProUGUI>().text = $"{chicken.Strength} {chicken.Growth} {chicken.Gain}";
                        }
                    }
                    else if (itemPair.Key is Egg egg)
                    {
                        for(int i = 1; i <= itemPair.Value; i++)
                        {
                            GameObject eggHolder = Instantiate(chickenPrefab, itemsContent);
                            eggHolder.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = egg.Name;
                            eggHolder.transform.Find("Icon").GetComponent<Image>().sprite = egg.Icon;
                            eggHolder.transform.Find("Stats").GetComponent<TextMeshProUGUI>().text = $"{egg.Strength} {egg.Growth} {egg.Gain}";
                        }
                    }
                    else
                    {
                        //instantiate an item holder
                        GameObject itemHolder = Instantiate(itemPrefab, itemsContent);
                        //set the icon image
                        itemHolder.transform.Find("Icon").GetComponent<Image>().sprite = itemPair.Key.Icon;
                        //set the amount text
                        itemHolder.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text = itemPair.Value.ToString();
                    }
                }
            }
        }
    }
    
    /*
     * Initialize the tools view
     */
    private void InitializeTools(Dictionary<CollectibleItem, int> tools)
    {
        //initialize the counter
        int i = 0;
        if(tools == null)
        {

        }
        else
        {
            //go through each tool in the dictionary
            foreach (var itemPair in tools)
            {
                //get the tool holder UI
                GameObject itemHolder = increaseContent.GetChild(i).gameObject;
                //initialize icon image
                itemHolder.transform.Find("Icon").GetComponent<Image>().sprite = itemPair.Key.Icon;
                //initialize amount text in a format "have/needed" (3/4)
                itemHolder.transform.Find("Amount").GetComponent<TextMeshProUGUI>().text =
                    StorageManager.current.GetAmount(itemPair.Key) + "/" + itemPair.Value;
                //increase the counter
                i++;
            }
        }
    }

    #region Buttons

    //close the storage window
    public void CloseButton_Click()
    {
        gameObject.SetActive(false);
    }

    //open increase window
    public void IncreaseButton_Click()
    {
        increaseView.SetActive(true);
    }

    //confirm increase
    public void ConfirmButton_Click()
    {
        increaseAction.Invoke();
    }

    //return to items view from increase window
    public void BackButton_Click()
    {
        increaseView.SetActive(false);
    }

    #endregion
}
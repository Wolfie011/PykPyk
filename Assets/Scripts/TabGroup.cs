using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> objectsToSwap = new List<GameObject>();
    public Sprite tabIdle;
    public Sprite tabSelected;
    public TabButton selectedTab;

    public void Start()
    {
        OnTabSelected(tabButtons[0]);
    }
    public void Subscribe(TabButton button)
    {
        tabButtons.Add(button);
    }
    private void ResetTabs()
    {
        foreach(var button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.background.sprite = tabIdle;
        }
    }
    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabSelected;

        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }
}

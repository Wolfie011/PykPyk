using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HatcherUI : MonoBehaviour
{
    #region UI
    [SerializeField] private TextMeshProUGUI storageTypeText;
    [SerializeField] private Slider capasitySlider;
    [SerializeField] private TextMeshProUGUI maxCapasity;

    [SerializeField] private GameObject hatcherView;
    [SerializeField] private GameObject eggView;
    [SerializeField] private GameObject upgradeView;

    [SerializeField] private Transform hatchContent;
    [SerializeField] private Transform eggContent;
    [SerializeField] private Transform upgradeContent;

    [SerializeField] private GameObject hatchPrefab;
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private List<GameObject> upgradeList;
    #endregion

    public Hatcher parentHatch;

    private void Awake()
    {
        HatcherWindow_Click();
    }

    public void Initialize()
    {
        InitializeHatch();
        InitializeEgg();
    }

    private void InitializeHatch()
    {
        if (parentHatch.hatches == null)
        {

        }
        else
        {
            foreach (var hatch in parentHatch.hatches)
            {
                GameObject eggHolder = Instantiate(eggPrefab, eggContent);
                eggHolder.GetComponent<HatchItem>().hatchedEgg = hatch.Value;
            }
        }
    }

    public void InitializeEgg()
    {
        //if the window was initialized before -> clear
        int childCount = eggContent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(eggContent.GetChild(i).gameObject);
        }

        if (StorageManager.current.eggs == null)
        {

        }
        else
        {
            foreach (var egg in StorageManager.current.eggs)
            {
                if(egg.Value == 0)
                {

                }
                else
                {
                    for(int i = 1; i <= egg.Value; i++)
                    {
                        //Debug.Log(i);
                        GameObject eggHolder = Instantiate(eggPrefab, eggContent);
                        eggHolder.GetComponent<EggItem>().egg = egg.Key;
                    }
                }  
            }
        }
    }

    #region Buttons

    //close the storage window
    public void CloseButton_Click()
    {
        gameObject.SetActive(false);
    }

    //open egg window
    public void EggWindow_Click()
    {
        eggView.SetActive(true);
        upgradeView.SetActive(false);
        hatcherView.SetActive(false);
        storageTypeText.text = "Eggs";
        capasitySlider.gameObject.SetActive(false);
        maxCapasity.gameObject.SetActive(false);
    }

    //open upgrade window
    public void UpgradeWindow_Click()
    {
        upgradeView.SetActive(true);
        hatcherView.SetActive(false);
        eggView.SetActive(false);
        storageTypeText.text = "Upgrades";
        capasitySlider.gameObject.SetActive(false);
        maxCapasity.gameObject.SetActive(false);
    }

    //open hatcher window
    public void HatcherWindow_Click()
    {
        hatcherView.SetActive(true);
        eggView.SetActive(false);
        upgradeView.SetActive(false);
        storageTypeText.text = "Hatcher";
        capasitySlider.gameObject.SetActive(true);
        maxCapasity.gameObject.SetActive(true);
    }

    #endregion
}

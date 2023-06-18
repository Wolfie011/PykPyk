using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Hatcher : Building
{
    private HatcherUI hatcherUI;
    public Hatcher current;

    [SerializeField] private GameObject windowPrefab;

    public Dictionary<String, Egg> hatches = new Dictionary<String, Egg>();

    public float hatchSpeed = 1;
    public static int hatchCapasity = 1;

    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        //instantiate UI
        GameObject window = Instantiate(windowPrefab, GameManager.current.canvas.transform);

        //make it invisible
        window.SetActive(false);

        //get the hatcher ui script
        hatcherUI = window.GetComponent<HatcherUI>();

        hatcherUI.parentHatch = current;

        //initialize the UI
        hatcherUI.Initialize();
    }
    public virtual void onClick()
    {
        //initialize hatcher UI so it is updated
        hatcherUI.Initialize();

        //make the UI visible after the click on the building
        hatcherUI.gameObject.SetActive(true);
    }

    private void OnMouseUpAsButton()
    {
        onClick();
    }

    public void startHatch(Egg egg)
    {
        hatches.Add(egg.id, egg);
        //hatcherUI.Initialize();
    }
}

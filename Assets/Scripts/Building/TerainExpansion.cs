using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerainExpansion : MonoBehaviour
{
    public BoundsInt area;
    private bool toogleWindow;
    private GameObject WindowFab;
    private int lvlUnlockable;

    private void OnMouseDown()
    {
        if (toogleWindow) return;
        else
        {
            toogleWindow = true;
        }
    }
    private void Update()
    {
        if (toogleWindow)
        {
            WindowFab.SetActive(true);

        }
    }
}

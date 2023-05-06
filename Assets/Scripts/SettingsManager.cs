using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager current;
    private bool opened;
    private RectTransform rt;
    private RectTransform prt;

    private void Awake()
    {
        current = this;
        rt = GetComponent<RectTransform>();
        prt = transform.parent.GetComponent<RectTransform>();

    }
    public void SettingsButton_Click()
    {
        float time = 0.2f;
        if (!opened)
        {
            LeanTween.moveX(prt, prt.anchoredPosition.y + rt.sizeDelta.y, time);
            opened = true;
            gameObject.SetActive(true);
        }
        else
        {
            LeanTween.moveX(prt, prt.anchoredPosition.y - rt.sizeDelta.y, time)
                .setOnComplete(delegate ()
                {
                    gameObject.SetActive(false);
                });
            opened = false;
        }
    }
}

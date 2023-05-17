using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerClickHandler
{
    public TabGroup tabGroup;
    [NonSerialized] public Image background;

    private void Awake()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }
}

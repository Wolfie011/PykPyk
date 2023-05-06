using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemDrag : MonoBehaviour, IEndDragHandler, IDragHandler
{
    public static Canvas canvas;
    private RectTransform rt;
    private CanvasGroup cg;
    private Image img;
    private Vector3 orginPos;
    private bool drag;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        img = GetComponent<Image>();
        orginPos = rt.anchoredPosition;
    }
    public void OnBeingDrag(PointerEventData eventData)
    {
        drag = true;
        cg.blocksRaycasts = false;
        img.maskable = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        rt.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        drag = false;
        cg.blocksRaycasts = true;
        img.maskable = true;
        rt.anchoredPosition = orginPos;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        ShopManager.current.ShopButton_Click();
        Color c = img.color;
        c.a = 0f;
        img.color = c;
    }
    private void OnEnable()
    {
        drag = false;
        cg.blocksRaycasts = true;
        img.maskable = true;

        Color c = img.color;
        c.a = 1f;
        img.color = c;
    }
}

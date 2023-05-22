using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemDrag : MonoBehaviour, IEndDragHandler, IDragHandler
{
    private ShopItem Item;

    public static Canvas canvas;

    private RectTransform rt;
    private CanvasGroup cg;
    private Image img;

    private Vector3 orginPos;
    private bool drag;

    public void Initialize(ShopItem item)
    {
        Item = item;
    }

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
        img.maskable = false;
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


        EventManager.Instance.AddListenerOnce<EnoughCurrencyGameEvent>(OnEnoughCurrency);
        EventManager.Instance.AddListenerOnce<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);
        CurrencyChangeGameEvent info = new CurrencyChangeGameEvent(-Item.Price, Item.Currency);
        EventManager.Instance.QueueEvent(info);

    }

    private void Buy()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y);
        position = Camera.main.ScreenToWorldPoint(position);
        BuildingSystem.current.InitializeWithObject(Item.Prefab, position);
    }
    private void OnEnoughCurrency(EnoughCurrencyGameEvent info)
    {
        Buy();
        EventManager.Instance.RemoveListener<NotEnoughCurrencyGameEvent>(OnNotEnoughCurrency);
    }
    private void OnNotEnoughCurrency(NotEnoughCurrencyGameEvent info)
    {
        Debug.Log("Niestaæ mnie");
        EventManager.Instance.RemoveListener<EnoughCurrencyGameEvent>(OnEnoughCurrency);
    }

    private void OnEnable()
    {
        drag = false;
        cg.blocksRaycasts = true;
        img.maskable = true;
        rt.anchoredPosition = orginPos;

        Color c = img.color;
        c.a = 1f;
        img.color = c;
    }
}
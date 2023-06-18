using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EggItem : MonoBehaviour
{
    public Egg egg;

    private void Awake()
    {
        gameObject.transform.Find("EggName").GetComponent<TextMeshProUGUI>().text = egg.Name;
        gameObject.transform.Find("EggImage").GetComponent<Image>().sprite = egg.Icon;
        gameObject.transform.Find("EggStats").GetComponent<TextMeshProUGUI>().text = $"{egg.Strength} / {egg.Gain} / {egg.Growth}";
    }
    public void startHatch()
    {
        Dictionary<CollectibleItem, int> result = new Dictionary<CollectibleItem, int>();
        result.Add(egg, 1);
        StorageManager.current.UpdateItems(result, false);

        GetComponentInParent<HatcherUI>().parentHatch.startHatch(egg);
        Destroy(gameObject);
    }
}

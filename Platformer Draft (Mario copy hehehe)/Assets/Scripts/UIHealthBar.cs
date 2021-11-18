using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    private Transform heartContainer;
    private Transform heartTemplate;
    // Start is called before the first frame update
    void Awake()
    {
        heartContainer = transform.Find("HeartContainer");
        heartTemplate = transform.Find("HeartTemplate");
    }

    public void SetHearts(int num)
    {
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        RectTransform heartRect = (RectTransform) heartTemplate.transform;
        float heartSize = heartRect.rect.width + 5;
        for (int i = 0;i<num;i++)
        {
            RectTransform heartRectTransform = Instantiate(heartTemplate, heartContainer).GetComponent<RectTransform>();
            heartRectTransform.gameObject.SetActive(true);
            heartRectTransform.anchoredPosition = new Vector2(i * heartSize, 0);
        }
    }
}

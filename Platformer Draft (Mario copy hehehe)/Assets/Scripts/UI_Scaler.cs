using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scaler : MonoBehaviour
{
    private Vector2 resolution;
    public float initialScale = 0.1f;
    private Vector2 initialSize;
    void Awake()
    {
        RectTransform initialTransform = (RectTransform)transform;
        initialSize = new Vector2(initialTransform.rect.width, initialTransform.rect.height);
        resolution = new Vector2(Screen.width, Screen.height);
        transform.localScale = new Vector2(resolution.x * initialScale / initialSize.x, resolution.x * initialScale / initialSize.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (resolution.x != Screen.width || resolution.y != Screen.height)
        {
            // do your stuff
            resolution.x = Screen.width;
            resolution.y = Screen.height;
            transform.localScale = new Vector2(resolution.x * initialScale / initialSize.x, resolution.x * initialScale / initialSize.y);
            Debug.Log(resolution);
        }
    }
}

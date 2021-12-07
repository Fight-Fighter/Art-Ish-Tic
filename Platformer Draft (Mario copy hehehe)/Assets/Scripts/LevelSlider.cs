using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform startPos;
    public Transform endPos;
    public Transform track;
    public Slider slider;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float prop = (track.position.x - startPos.position.x) / (endPos.position.x - startPos.position.x);
        slider.value = Mathf.Clamp(prop, 0, 1);
    }
}

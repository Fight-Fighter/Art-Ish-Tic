using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject boss;
    public GameObject bossLevelSlider;
    public GameObject bossHealthBar;
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D()
    {
        bossLevelSlider.SetActive(true);
        bossHealthBar.SetActive(true);
        boss.SetActive(true);
    }
}

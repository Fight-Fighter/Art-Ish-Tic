using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public float timeLeft = 50;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.Credits);
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}

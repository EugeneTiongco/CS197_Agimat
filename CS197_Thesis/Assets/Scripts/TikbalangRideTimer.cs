using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TikbalangRideTimer : MonoBehaviour
{
    // Start is called before the first frame update
    private Image timerBar;
    private float maxTime = 15f;
    private float timeLeft;
    void Start()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}

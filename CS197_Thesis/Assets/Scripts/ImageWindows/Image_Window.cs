using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Image_Window : MonoBehaviour
{
    private static Image_Window instance;
    [SerializeField] private Image redLine;
    [SerializeField] private Image tikbalangRide;

    private float maxTime = 5f;
    private float timeLeft;
    private bool isActive;

    void Start()
    {
        timeLeft = maxTime;
    }
    private void Awake()
    {
        instance = this;
        redLine.enabled = false;
        tikbalangRide.enabled = false;
    }

    void Update()
    {
        if (isActive)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                redLine.fillAmount = timeLeft / maxTime;
            }
            else
            {
                isActive = false;
                Time.timeScale = 0;
            }
        }
        //Debug.Log(isActive);
    }

    public void vHide()
    {
        gameObject.SetActive(false);
        redLine.enabled = false;
        tikbalangRide.enabled = false;
    }

    public static void Hide()
    {
        instance.vHide();
    }

   /* private void ShowString(string text)
    {
        gameObject.SetActive(true);
        transform.Find("Text").GetComponent<Text>().text = text;
        StartCoroutine(TextGoneDelay());
    }


    public static void Show(string text)
    {
        instance.ShowString(text);
    }*/


    private void Show(string turnstring)
    {
        gameObject.SetActive(true);
        transform.Find("Text").GetComponent<Text>().text = turnstring;
        StartCoroutine(TextGoneDelay());
    }

    public static void Show_Static(string winnerstring)
    {
        instance.Show(winnerstring);
    }

    private void ShowImages()
    {
        gameObject.SetActive(true);
        redLine.enabled = true;
        //tikbalangRide.enabled = true;
        isActive = true;
    }

    public static void DisplayImages()
    {
        instance.ShowImages();
    }

    public static bool CheckIsActive()
    {
        return instance.isActive;
    }

   

    IEnumerator TextGoneDelay()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn_Window2 : MonoBehaviour
{
    private static Turn_Window2 instance;

    private void Awake()
    {
        instance = this;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void S()
    {
        gameObject.SetActive(true);
        StartCoroutine(TextGoneDelay());
    }

    public static void Pakita()
    {
        instance.S();
    }

    public static void Tago()
    {
        instance.Hide();
    }

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

    IEnumerator TextGoneDelay()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}

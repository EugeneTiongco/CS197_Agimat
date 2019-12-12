using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn_Window : MonoBehaviour
{
    private static Turn_Window instance;

    private void Awake()
    {
        instance = this;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
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

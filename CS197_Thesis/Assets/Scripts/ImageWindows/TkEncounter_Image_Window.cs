using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TkEncounter_Image_Window : MonoBehaviour
{
    private static TkEncounter_Image_Window instance;
    [SerializeField] private Image tikbalang;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
        
    }

    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void ShowImages()
    {
        gameObject.SetActive(true);
        tikbalang.enabled = true;
    }

    public static void DisplayImages()
    {
        instance.ShowImages();
    }
}

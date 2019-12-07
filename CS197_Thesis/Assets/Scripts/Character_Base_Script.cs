using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Base_Script : MonoBehaviour
{
    // Start is called before the first frame update
    private int actionPoints;
    private int position;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup (bool isPlayer)
    {
        if(isPlayer)
        {
            actionPoints = 5;
        }
        else
        {
            actionPoints = 3;
        }
    }

    public int ReturnActionPoints()
    {
        return actionPoints;
    }

    public void UpdateActionPoints(int a)
    {
        actionPoints = a;
    }

    public void SetActionPoints(bool isPlayer)
    {
        if (isPlayer)
        {
            actionPoints = 5;
        }
        else
        {
            actionPoints = 3;
        }
    }

    public int ReturnPosition()
    {
        return position;
    }

    public void UpdatePosition(int a)
    {
        position = a;
    }



}

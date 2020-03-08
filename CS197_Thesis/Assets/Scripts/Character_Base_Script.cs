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
        //MovementPhase();
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


    private void MovementPhase()
    {
        int tempPos = ReturnPosition();

        if (Input.GetKeyDown(KeyCode.A))
        {
            tempPos = tempPos - 1;


            transform.position = new Vector3(transform.position.x - 1, transform.position.y);
            UpdatePosition(tempPos);

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            tempPos = tempPos + 18;


            transform.position = new Vector3(transform.position.x, transform.position.y - 1);
            UpdatePosition(tempPos);


        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            tempPos = tempPos + 1;


            transform.position = new Vector3(transform.position.x + 1, transform.position.y);
            UpdatePosition(tempPos);


        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            tempPos = tempPos - 18;


            transform.position = new Vector3(transform.position.x, transform.position.y + 1);
            UpdatePosition(tempPos);


        }

    }


}

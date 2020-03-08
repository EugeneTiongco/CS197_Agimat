using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BerberokaMazeP1a_SH : MonoBehaviour
{

    private static BerberokaMazeP1a_SH stageHandler;

    public static BerberokaMazeP1a_SH getInstance()
    {
        return stageHandler;
    }

    [SerializeField] private Transform pf_Character;
    [SerializeField] private Transform pf_Berberoka_Base;

    private Character_Base_Script playerCharacter;
    private Character_Base_Script Berberoka;
    private State state;

    private enum State
    {
        PlayerMovement, EnvironmentMovement, LoadNextPhase, Gameover
    }

    char[] map = new char[]
    {
        'w',    'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w',    'w',

        'w',    'w', 'w', 'w', 'r', 'b', 'b', 'b', 'w', 'w', 'w', 'w', 'w', 'w', 'b', 'b', 'b',    'w',
        'w',    't', 'v', 'r', 'r', 'b', 'b', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'b', 'b', 'b',    'w',
        'w',    '0', 'r', 'r', 'r', 'b', 'b', 'w', 'w', '0', 'w', 'w', 'w', 'b', 'b', 'b', 'b',    'w',
        'w',    '0', 'r', 'r', 'b', 'b', 'b', 'l', 'l', '0', '0', 'w', 'w', 'b', 'b', 'b', 'b',    'w',
        'w',    '0', 'r', 'r', 'r', 'b', 'd', 'l', 'u', 'u', 'v', 'r', 'r', 'u', '0', 'b', 'b',    'w',
        'w',    'u', 'r', 'r', '0', 'b', 'r', 'r', '0', '0', 'r', 'u', 'd', 'l', '0', 'w', 'w',    'w',
        'w',    'u', 'l', '0', '0', 'u', 'l', 'l', 'u', 'u', 'r', 'u', 'd', 'd', 'l', 'w', 'w',    'w',
        'w',    'w', 'd', '0', 'v', '0', 'u', 'u', 'u', 'w', 'r', 'u', 'd', 'd', 'w', 'w', 'w',    'w',
        'w',    'w', 'r', 'r', '0', '0', 'r', 'u', 'u', 'w', 'r', 'u', 'd', 'd', 'w', 'w', 'w',    'w',
        'w',    'w', 'w', 'd', '0', 'r', 'u', 'w', 'w', 'w', 'w', 'u', 'l', 'd', 'w', 'w', 'w',    'w',
        'w',    'w', 'w', 'd', 'r', 'u', 'u', 'b', 'b', 'b', 'w', 'w', 'w', 'r', '0', '0', 'e',    'w',
        'w',    'w', 'w', 'r', 'r', 'r', 'b', 'b', 'b', 'b', 'b', 'w', 'w', 'w', 'w', 'w', 'w',    'w',

        'w',    'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w', 'w',    'w',

    };

    private void Awake()
    {
        stageHandler = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        //Turn_Window.Show_Static("The tikbalang made you lose your way");
        playerCharacter = SpawnCharacters(true);
        Berberoka = SpawnCharacters(false);
        playerCharacter.UpdatePosition(91);
        Debug.Log("player position:" + playerCharacter.ReturnPosition());
    }
    private Character_Base_Script SpawnCharacters(bool isPlayer)
    {
        Vector3 position;
        Character_Base_Script character;
        if (isPlayer)
        {
            position = new Vector3(-7.5f, 1.5f);
            Transform characterTransform = Instantiate(pf_Character, position, Quaternion.identity);
            character = characterTransform.GetComponent<Character_Base_Script>();
            character.Setup(isPlayer);
        }
        else
        {
            position = new Vector3(-2.8f, 3.5f);
            Transform characterTransform = Instantiate(pf_Berberoka_Base, position, Quaternion.identity);
            character = characterTransform.GetComponent<Character_Base_Script>();
            character.Setup(isPlayer);
        }

        return character;
    }
    // Update is called once per frame
    void Update()
    {
        if (state == State.PlayerMovement)
        {
            MovementPhase();
            CheckEnvironmentTrigger();
        }

        if (state == State.EnvironmentMovement)
        {
            StartCoroutine(WaterCurrentDelay());
        }

        if (state == State.Gameover)
        {
            StartCoroutine(GameOverDelay());
        }

        if (state == State.LoadNextPhase)
        {
            Debug.Log("Valve turned 2");
            SceneManager.LoadScene(5);
        }
    }

  

    private void MovementPhase()
    {
        int tempPos = playerCharacter.ReturnPosition();
        

        if (Input.GetKeyDown(KeyCode.A))
        {
            tempPos = tempPos - 1;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x - 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            tempPos = tempPos + 18;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y - 1);
                playerCharacter.UpdatePosition(tempPos);                
            }

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            tempPos = tempPos + 1;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x + 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);               
            }

        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            tempPos = tempPos - 18;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1);
                playerCharacter.UpdatePosition(tempPos);      
            }

        }

        else if (map[tempPos] == 't' && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Valve turned 1");
            state = State.LoadNextPhase;
            Debug.Log("state: " + state);
            

        }
    }

    private void EnvironmentMovementPhase()
    {
        int tempPos = playerCharacter.ReturnPosition();

        if (map[tempPos] == 'u')
        {
            tempPos = tempPos - 18;
            if (CheckCollision(tempPos))
            {                
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1);
                playerCharacter.UpdatePosition(tempPos);
                
            }
        }
        else if (map[tempPos] == 'd')
        {
            tempPos = tempPos + 18;
            if (CheckCollision(tempPos))
            {              
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y - 1);
                playerCharacter.UpdatePosition(tempPos);
                
            }

        }
        else if (map[tempPos] == 'r')
        {
            tempPos = tempPos + 1;
            if (CheckCollision(tempPos))
            {                
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x + 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
                
            }

        }
        else if (map[tempPos] == 'l')
        {
            tempPos = tempPos - 1;
            if (CheckCollision(tempPos))
            {              
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x - 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
                
            }

        }
    }

    private bool CheckCollision(int tempPos)
    {
        if (map[tempPos] == 'w' || map[tempPos] == 'v' )
        {
            return false;
        }

        else
        {
            return true;
        }
    }


    private void CheckEnvironmentTrigger()
    {
        int currentPosition = playerCharacter.ReturnPosition();
        
        //currents trigger (up, down, left, right)
        if (map[currentPosition] == 'u' || map[currentPosition] == 'd' || map[currentPosition] == 'l' || map[currentPosition] == 'r' )
        {
            state = State.EnvironmentMovement;
            Debug.Log("Here:" + state);
        }

        else if(map[currentPosition] == '0')
        {
            state = State.PlayerMovement;
        }


        //fell into deep water
        else if (map[currentPosition] == 'b')
        {
            state = State.Gameover;

        }

        else if(map[currentPosition] == 't')
        {
            Turn_Window.Show_Static("Press E to turn valve.");
        }
    }

    IEnumerator WaterCurrentDelay()
    {
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(1f);
        EnvironmentMovementPhase();
        CheckEnvironmentTrigger();
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    IEnumerator GameOverDelay()
    {
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(4);
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}

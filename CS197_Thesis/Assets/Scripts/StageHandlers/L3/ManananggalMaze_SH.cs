using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManananggalMaze_SH : MonoBehaviour
{
    int[] map = new int[]
    {
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // 1
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // 2
        1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 5, 1, // 3
        1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 5, 1, // 4
        1, 0, 1, 0, 1, 0, 1, 1, 1, 2, 1, 1, 0, 1, 0, 1, 1, 1, // 5
        1, 0, 1, 0, 1, 4, 4, 4, 1, 2, 1, 1, 0, 1, 0, 1, 1, 1, // 6 
        1, 0, 1, 0, 1, 4, 4, 4, 1, 2, 1, 1, 0, 1, 0, 1, 1, 1, // 7
        1, 0, 1, 0, 1, 0, 1, 0, 1, 2, 1, 1, 2, 1, 0, 0, 0, 1, // 8
        1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 2, 1, 1, 1, 0, 1, // 9
        1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 0, 2, 2, 2, 2, 0, 0, 1, // 10
        1, 1, 1, 0, 1, 0, 2, 2, 2, 2, 0, 1, 2, 1, 1, 1, 1, 1, // 11
        1, 0, 0, 0, 1, 0, 1, 1, 1, 1, 0, 1, 2, 1, 0, 0, 0, 1, // 12
        1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, // 13
        1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, // 14
        1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, // 15
        1, 0, 0, 0, 0, 2, 2, 2, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, // 16
        1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, // 17
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 2, 1, // 18
        1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 0, 1, 1, 1, 2, 1, // 19
        1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 2, 1, // 20
        1, 0, 1, 1, 2, 1, 2, 1, 0, 1, 1, 1, 1, 1, 6, 1, 2, 1, // 21
        1, 0, 1, 1, 2, 1, 2, 1, 0, 2, 2, 2, 0, 1, 6, 0, 0, 1, // 22
        1, 0, 7, 1, 2, 1, 2, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, // 23
        1, 1, 1, 1, 0, 0, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, // 24
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1  // 25

    };

    private static ManananggalMaze_SH stageHandler;

    public static ManananggalMaze_SH getInstance()
    {
        return stageHandler;
    }

    [SerializeField] private Transform pf_Character;
    [SerializeField] private Transform AI;

    //TODO: Generate Manananggal prefab

    private Character_Base_Script playerCharacter;
    private Character_Base_Script Manananggal;
    private State state;
    Transform characterTransform;
    Transform monsterTransform;

    private int playerBawang = 0;
    private int monsterHealth = 3;
    private bool bawang1 = true;
    private bool bawang2 = true;
    private bool bawang3 = true;

    private string lastDir = "";


    private enum State
    {
        PlayerMovement, EnvironmentMovement, LoadNextPhase, Gameover, Win
    }
    private void Awake()
    {
        stageHandler = this;

    }
    // Start is called before the first frame update

  
    void Start()
    {
        Debug.Log("Start");
        SpawnCharacters();
        stageHandler.transform.position = playerCharacter.transform.position;
        playerCharacter.UpdatePosition(200);
    }


    private void SpawnCharacters()
    {
        Vector3 position = new Vector3(-6.5f, -6.5f);
        characterTransform = Instantiate(pf_Character, position, Quaternion.identity);
        playerCharacter = characterTransform.GetComponent<Character_Base_Script>();
  

        position = new Vector3(3.5f, 3.5f);
        Pathfinding.AIDestinationSetter destinationSetter;
        monsterTransform = Instantiate(AI, position, Quaternion.Euler(-90, 0, 0));
        destinationSetter = monsterTransform.GetComponent<Pathfinding.AIDestinationSetter>();
        destinationSetter.SetTarget(characterTransform);
    }

    // Update is called once per frame
    void Update()
    {
        if(characterTransform.position == monsterTransform.position)
        {
            Debug.Log("You got caught");
        }

        if (monsterHealth == 0)
        {
            Debug.Log("You win");
        }

        if (state == State.PlayerMovement)
        {
            PlayerActionPhase();
            //camera chases player
            stageHandler.transform.position = playerCharacter.transform.position;
            CheckEnvironmentTrigger();

        }

        if(state == State.EnvironmentMovement)
        {
            StartCoroutine(OilSlideDelay());
        }

    }


    public Character_Base_Script returnPlayer()
    {
        return playerCharacter;
    }

    private void PlayerActionPhase()
    {
        int tempPos = playerCharacter.ReturnPosition();
        
        if(Input.GetKeyDown(KeyCode.A))
        {
            tempPos = tempPos - 1;

            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x - 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
                lastDir = "left";
            }  
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            tempPos = tempPos + 18;

            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y - 1);
                playerCharacter.UpdatePosition(tempPos);
                lastDir = "down";
            }
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            tempPos = tempPos + 1;

            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x + 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
                lastDir = "right";
            }           
        }

        else if (Input.GetKeyDown(KeyCode.W))
        {
            tempPos = tempPos - 18;

            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1);
                playerCharacter.UpdatePosition(tempPos);
                lastDir = "up";
            }
        }

        else if (Input.GetKeyDown(KeyCode.E))
        {
            CheckActionLocation(tempPos);
        }
    }

    private void EnvironmentMovementPhase()
    {
        int tempPos = playerCharacter.ReturnPosition();

        if (lastDir == "up")
        {
            tempPos = tempPos - 18;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1);
                playerCharacter.UpdatePosition(tempPos);
            }
        }
        else if (lastDir == "down")
        {
            tempPos = tempPos + 18;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y - 1);
                playerCharacter.UpdatePosition(tempPos);
            }

        }
        else if (lastDir == "right")
        {
            tempPos = tempPos + 1;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x + 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
            }

        }
        else if (lastDir == "left")
        {
            tempPos = tempPos - 1;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x - 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
            }

        }
    }

    private bool CheckCollision(int location)
    {
        if (map[location] == 1)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    private void CheckActionLocation(int location)
    {
        if (map[location] == 4 && bawang1 == true) // bawang 1
        {
            bawang1 = false;
            playerBawang++;
            Debug.Log("Total Bawang:" + playerBawang);
        }

        else if(map[location] == 5 && bawang2 == true) //bawang 2
        {
            bawang2 = false;
            playerBawang++;
            Debug.Log("Total Bawang:" + playerBawang);
        }

        else if (map[location] == 6 && bawang3 == true) //bawang 3
        {
            bawang3 = false;
            playerBawang++;
            Debug.Log("Total Bawang:" + playerBawang);
        }

        else if (map[location] == 7) //monster body
        {
            if(playerBawang > 0)
            {
                playerBawang--;
                monsterHealth--;
                Debug.Log("Total Bawang:" + playerBawang);
            }
        }

    }

    private void CheckEnvironmentTrigger()
    {
        int currentPosition = playerCharacter.ReturnPosition();
       
        //currents trigger (up, down, left, right)
        if (map[currentPosition] == 2)
        {
            state = State.EnvironmentMovement;
            Debug.Log("Here:" + state);
        }

        else if (map[currentPosition] == 0)
        {
            lastDir = "";
            state = State.PlayerMovement;
           
        }
    }

    IEnumerator OilSlideDelay()
    {
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(0.5f);
        EnvironmentMovementPhase();
        CheckEnvironmentTrigger();
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}

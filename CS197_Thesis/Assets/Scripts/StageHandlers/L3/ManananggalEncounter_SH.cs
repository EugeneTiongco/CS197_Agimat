using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ManananggalEncounter_SH : MonoBehaviour
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
        1, 4, 4, 4, 1, 0, 1, 1, 1, 1, 0, 1, 2, 1, 0, 0, 0, 1, // 12
        1, 4, 1, 4, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 0, 1, // 13
        1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0, 1, // 14
        1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, // 15
        1, 0, 1, 0, 0, 2, 2, 2, 0, 0, 0, 1, 0, 1, 0, 1, 1, 1, // 16
        1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 0, 1, // 17
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 2, 1, // 18
        1, 1, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2, 0, 1, 1, 1, 2, 1, // 19
        1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 2, 1, // 20
        1, 0, 1, 1, 2, 1, 2, 1, 0, 1, 1, 1, 1, 1, 6, 1, 2, 1, // 21
        1, 0, 1, 1, 2, 1, 2, 1, 0, 2, 2, 2, 0, 1, 6, 0, 0, 1, // 22
        1, 0, 7, 1, 2, 1, 2, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, // 23
        1, 1, 1, 1, 0, 0, 0, 2, 2, 2, 2, 2, 0, 1, 1, 1, 1, 1, // 24
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1  // 25

    };

    private static ManananggalEncounter_SH stageHandler;

    public static ManananggalEncounter_SH getInstance()
    {
        return stageHandler;
    }

    [SerializeField] private Transform pf_Character;
    [SerializeField] private Transform AI;

    private Character_Base_Script playerCharacter;
    private Character_Base_Script Manananggal;
    private State state;
    Transform characterTransform;
    Transform monsterTransform;

    private enum State
    {
        PlayerMovement, LoadNextPhase
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
        playerCharacter.UpdatePosition(145);
    }

    private void SpawnCharacters()
    {
        Vector3 position = new Vector3(-7.5f, -3.5f);
        characterTransform = Instantiate(pf_Character, position, Quaternion.identity);
        playerCharacter = characterTransform.GetComponent<Character_Base_Script>();


        position = new Vector3(-6.5f, 10.5f);
        // Pathfinding.AIDestinationSetter destinationSetter;
        //monsterTransform = Instantiate(AI, position, Quaternion.Euler(-90, 0, 0));
        monsterTransform = Instantiate(AI, position, Quaternion.identity);
        //destinationSetter = monsterTransform.GetComponent<Pathfinding.AIDestinationSetter>();
        //destinationSetter.SetTarget(characterTransform);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.PlayerMovement)
        {
            PlayerActionPhase();
            //camera chases player
            stageHandler.transform.position = playerCharacter.transform.position;
            

        }

        if (state == State.LoadNextPhase)
        {
            SceneManager.LoadScene(11);
        }
    }

    private void PlayerActionPhase()
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

        else if (Input.GetKeyDown(KeyCode.E))
        {
            CheckActionLocation(tempPos);
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
        if (map[location] == 4) 
        {
            state = State.LoadNextPhase;
        }
    }
}

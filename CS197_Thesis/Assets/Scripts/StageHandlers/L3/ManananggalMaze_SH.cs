using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManananggalMaze_SH : MonoBehaviour
{


    private static ManananggalMaze_SH stageHandler;

    public static ManananggalMaze_SH getInstance()
    {
        return stageHandler;
    }

    [SerializeField] private Transform pf_Character;
    [SerializeField] private Transform AI;

    //[SerializeField] private Transform pf_Berberoka_Base;
    //TODO: Generate Manananggal prefab

    private Character_Base_Script playerCharacter;
    private Character_Base_Script Manananggal;
    private State state;

    

    private enum State
    {
        PlayerMovement, EnvironmentMovement, LoadNextPhase, Gameover
    }
    private void Awake()
    {
        stageHandler = this;

    }
    // Start is called before the first frame update

    Transform characterTransform;
    Transform monsterTransform;
    void Start()
    {
        Debug.Log("Start");
        SpawnCharacters();

    }

    //private Character_Base_Script SpawnCharacters(bool isPlayer)
    //{
    //    Vector3 position;
    //    Character_Base_Script character;
    //    if (isPlayer)
    //    {
    //        position = new Vector3(-0.5f, 0.5f);
    //        Transform characterTransform = Instantiate(pf_Character, position, Quaternion.identity);
    //        character = characterTransform.GetComponent<Character_Base_Script>();
    //        character.Setup(isPlayer);
    //    }
    //    else
    //    {
    //        position = new Vector3(-2.8f, 3.5f);
    //        Transform characterTransform = Instantiate(AI, position, Quaternion.identity);
    //        character = characterTransform.GetComponent<Character_Base_Script>();
    //        character.Setup(isPlayer);
    //    }

    //    return character;
    //}

    private void SpawnCharacters()
    {
        Vector3 position = new Vector3(-0.5f, 0.5f);
        characterTransform = Instantiate(pf_Character, position, Quaternion.identity);
        playerCharacter = characterTransform.GetComponent<Character_Base_Script>();
  

        position = new Vector3(-2.0f, 0.0f);
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

        if (state == State.PlayerMovement)
        {
            MovementPhase();
           
        }
    }


    public Character_Base_Script returnPlayer()
    {
        return playerCharacter;
    }
    private void MovementPhase()
    {
        int tempPos = playerCharacter.ReturnPosition();
        
        if(Input.GetKeyDown(KeyCode.A))
        {
            tempPos = tempPos - 1;


            playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x - 1, playerCharacter.transform.position.y);
            playerCharacter.UpdatePosition(tempPos);
            
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            tempPos = tempPos + 18;
            
            
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y - 1);
                playerCharacter.UpdatePosition(tempPos);
            

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            tempPos = tempPos + 1;
          
            
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x + 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
            

        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            tempPos = tempPos - 18;
            
            
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1);
                playerCharacter.UpdatePosition(tempPos);
            

        }

    }
}

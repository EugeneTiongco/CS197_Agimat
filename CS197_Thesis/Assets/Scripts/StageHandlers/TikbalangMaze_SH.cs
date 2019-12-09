using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TikbalangMaze_SH : MonoBehaviour
{

    private static TikbalangMaze_SH stageHandler;

    public static TikbalangMaze_SH getInstance()
    {
        return stageHandler;
    }


    [SerializeField] private Transform pf_Character_Base;
    [SerializeField] private Transform pf_Enemy_Base; //TODO change this to tikbalang prefab
    public AudioSource triggerSound;
    public AudioClip clip;

    private Character_Base_Script playerCharacter;
    private Character_Base_Script tikbalang;
    private State state;

    int[] map = new int[]
    {
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1,
        1, 1, 0, 0, 0, 3, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1,
        1, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1, 1,
        1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0, 1, 1,
        1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 1,
        1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1,
        1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1,
        1, 1, 1, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 2, 0, 0, 0, 1, 1,
        1, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 1, 1, 1, 0, 1, 1,
        1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1

    };
    private enum State
    {
        MovementPhase, Cutscene, Sound, End, TikbalangRide
    }

    private void Awake()
    {
        stageHandler = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = SpawnCharacters(true);
        tikbalang = SpawnCharacters(false);
        triggerSound = GetComponent<AudioSource>();
        playerCharacter.UpdatePosition(164);
        Debug.Log("player position:" + playerCharacter.ReturnPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.MovementPhase)
        {
            MovementPhase();
            CheckSceneTrigger();
        }




        if (state == State.Cutscene)
        {

            //TODO pop up should appear here
        }

        if (state == State.End)
        {
           //TODO go back to previous scene
        }
    }

    private Character_Base_Script SpawnCharacters(bool isPlayer)
    {
        Vector3 position;
        Character_Base_Script character;
        if (isPlayer)
        {
            position = new Vector3(-6.5f, -2.5f);
            Transform characterTransform = Instantiate(pf_Character_Base, position, Quaternion.identity);
            character = characterTransform.GetComponent<Character_Base_Script>();
            character.Setup(isPlayer);
        }
        else
        {
            position = new Vector3(3.5f, -2.5f);
            Transform characterTransform = Instantiate(pf_Enemy_Base, position, Quaternion.identity);
            character = characterTransform.GetComponent<Character_Base_Script>();
            character.Setup(isPlayer);
        }

        return character;
    }

    private void CheckSceneTrigger()
    {
        if (playerCharacter.ReturnPosition() == 41) // Clue trigger
        {
            Turn_Window.Show_Static("15 times you must");
        }

        else if (playerCharacter.ReturnPosition() == 174) // tikbalang trigger
        {
            Turn_Window.Show_Static("*Tikbalang riding here*");
            state = State.TikbalangRide;
            StartCoroutine(TikbalangTimer());

        }

        else if (playerCharacter.ReturnPosition() == 15) // Escaped trigger
        {
            Turn_Window.Show_Static("You escaped the tikbalang");
            state = State.End;
        }



    }

    IEnumerator TikbalangTimer()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(15);
        state = State.MovementPhase;
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    private void MovementPhase()
    {
        int tempPos = playerCharacter.ReturnPosition();

        if (Input.GetKeyDown(KeyCode.S))
        {
            tempPos = tempPos + 18;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y - 1);
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
                Debug.Log("player position:" + playerCharacter.ReturnPosition());
            }

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            tempPos = tempPos - 1;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x - 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
                Debug.Log("player position:" + playerCharacter.ReturnPosition());
            }

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            tempPos = tempPos + 1;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x + 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
                Debug.Log("player position:" + playerCharacter.ReturnPosition());
            }

        }
    }

    private bool CheckCollision(int tempPos)
    {
        if (map[tempPos] == 1)
        {
            return false;
        }

        else if (map[tempPos] != -1)
        {
            return true;
        }

        else
        {
            return true;
        }
    }
}

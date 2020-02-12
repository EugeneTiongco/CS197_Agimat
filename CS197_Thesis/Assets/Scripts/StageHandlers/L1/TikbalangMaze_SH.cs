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


    [SerializeField] private Transform pf_Character_Fog;
    [SerializeField] private Transform pf_Enemy_Base; //TODO change this to tikbalang prefab
    public AudioSource triggerSound;
    public AudioClip clip;

    private Character_Base_Script playerCharacter;
    private Character_Base_Script tikbalang;
    private State state;
    private bool rodeTikbalang = false;
    private int spaceBarPressed = 0;
    private bool isActive;
    private bool pressedE;

    private float maxTime = 5f;
    private float timeLeft;

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
         Wait, RegularMovementPhase, IrregularMovementPhase, Start, FixedMovementPhase, Cutscene, Sound, End, TikbalangRide, GameOver
    }

    private void Awake()
    {
        stageHandler = this;

    }
    // Start is called before the first frame update
    void Start()
    {
        state = State.Wait;
        pressedE = false;
        Turn_Window.Show_Static("The tikbalang made you lose your way");
        //Turn_Window.Show_Static("Press E to fix clothes");
        StartCoroutine(DelayStartCoroutine());
       
        timeLeft = maxTime;
        playerCharacter = SpawnCharacters(true);
        tikbalang = SpawnCharacters(false);
        triggerSound = GetComponent<AudioSource>();
        playerCharacter.UpdatePosition(164);
        Debug.Log("player position:" + playerCharacter.ReturnPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Start)
        {
            Turn_Window.Show_Static("Press E to invert your clothes, you don't have much time!");
            //CheckSceneTrigger();
            StartCoroutine(WaitForECoroutine());
        }

        if (state == State.RegularMovementPhase)
        {
            RegularMovementPhase();
            CheckSceneTrigger();
        }

        if (state == State.IrregularMovementPhase)
        {
            IrregularMovementPhase();
            CheckSceneTrigger();
        }


        if (state == State.TikbalangRide)
        {
            
            Image_Window.DisplayImages();

            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }
            else
            {
                Time.timeScale = 0;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                spaceBarPressed++;
            }
           
            if(timeLeft <=0 && spaceBarPressed >= 15)
            {
                Turn_Window.Show_Static("You recieved the Tikbalang's mutya!");
                Image_Window.Hide();
                if(pressedE == true)
                {
                    state = State.RegularMovementPhase;
                }

                else if (pressedE == false)
                {
                    state = State.IrregularMovementPhase;
                }

            }

            else if(timeLeft <= 0 && spaceBarPressed < 15)
            {
                Image_Window.Hide();
                state = State.GameOver;
            }

        }


        if (state == State.Cutscene)
        {

            //TODO pop up should appear here
        }

        if (state == State.End)
        {
            
            LoadNextScene();
            //TODO go back to previous scene
        }

        if (state == State.GameOver)
        {
            StartCoroutine(GameOverTextTimer());
           

        }
    }

    private Character_Base_Script SpawnCharacters(bool isPlayer)
    {
        Vector3 position;
        Character_Base_Script character;
        if (isPlayer)
        {
            position = new Vector3(-6.5f, -2.5f);
            Transform characterTransform = Instantiate(pf_Character_Fog, position, Quaternion.identity);
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
            Turn_Window.Show_Static("Spacebar. 15 times.");
        }

        else if (playerCharacter.ReturnPosition() == 174 && rodeTikbalang == false) // tikbalang trigger
        {
            //Turn_Window.Show_Static("*Tikbalang riding here*");
            state = State.TikbalangRide;

        }

        else if (playerCharacter.ReturnPosition() == 15) // Escaped trigger
        {
            Turn_Window.Show_Static("You escaped the tikbalang");
            //StartCoroutine(NextSceneCoroutine());
            state = State.End;

        }

        else if(state == State.Start && Input.GetKeyDown(KeyCode.E))
        {
            pressedE = true;
            state = State.RegularMovementPhase;
        }
    }

    IEnumerator GameOverTextTimer()
    {
        Turn_Window.Show_Static("Game Over! Try Again");
        Debug.Log("State: " + state);
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(2);

        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }


    private void RegularMovementPhase()
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


    private void IrregularMovementPhase()
    {
        int tempPos = playerCharacter.ReturnPosition();

        if (Input.GetKeyDown(KeyCode.A))
        {
            tempPos = tempPos + 18;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y - 1);
                playerCharacter.UpdatePosition(tempPos);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            tempPos = tempPos - 18;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1);
                playerCharacter.UpdatePosition(tempPos);
                Debug.Log("player position:" + playerCharacter.ReturnPosition());
            }

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            tempPos = tempPos - 1;
            if (CheckCollision(tempPos))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x - 1, playerCharacter.transform.position.y);
                playerCharacter.UpdatePosition(tempPos);
                Debug.Log("player position:" + playerCharacter.ReturnPosition());
            }

        }
        else if (Input.GetKeyDown(KeyCode.W))
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

    private void LoadNextScene()
    {
        SceneManager.LoadScene(3);
    }

    IEnumerator DelayStartCoroutine()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(3);
        state = State.Start;
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    IEnumerator WaitForECoroutine()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        CheckSceneTrigger();
        yield return new WaitForSeconds(3);
        if(pressedE == false)
        {
            state = State.IrregularMovementPhase;
        }
        
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }


}

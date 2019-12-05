using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TikbalangEncounter_SH : MonoBehaviour
{
    private static TikbalangEncounter_SH stageHandler;

    public static TikbalangEncounter_SH getInstance()
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
    private bool soundPlayed;

    private enum State
    {
        MovementPhase, Cutscene, Sound, End
    }

    private void Awake()
    {
        stageHandler = this;

    }

    void Start()
    {
        playerCharacter = SpawnCharacters(true);
        tikbalang = SpawnCharacters(false);
        triggerSound = GetComponent<AudioSource>();
        //clip = (AudioClip)Resources.Load("Mmm", typeof(AudioClip));
    }

    // Update is called once per frame
    private void Update()
    {
       
        if (state == State.MovementPhase)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y - 1);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y + 1);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x - 1, playerCharacter.transform.position.y);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                playerCharacter.transform.position = new Vector3(playerCharacter.transform.position.x + 1, playerCharacter.transform.position.y);
            }
            CheckSceneTrigger();
        }
        


        /*if ( state == State.Sound)
        {
          
            if (!triggerSound.isPlaying)
            {
                triggerSound.Play();
                if (!triggerSound.isPlaying)
                    soundPlayed = true;
                if(soundPlayed == true)
                    state = State.Cutscene;
            }
        }*/

        if( state == State.Cutscene)
        {
           
            //TODO pop up should appear here
        }

        if (state == State.End)
        {
            LoadNextScene();
        }

        


    }


    private Character_Base_Script SpawnCharacters(bool isPlayer)
    {
        Vector3 position;
        Character_Base_Script character;
        if (isPlayer)
        {
            position = new Vector3(-6.5f, -4.5f);
            Transform characterTransform = Instantiate(pf_Character_Base, position, Quaternion.identity);
            character = characterTransform.GetComponent<Character_Base_Script>();
            character.Setup(isPlayer);
        }
        else
        {
            position = new Vector3(1.5f, 0.5f);
            Transform characterTransform = Instantiate(pf_Enemy_Base, position, Quaternion.identity);
            character = characterTransform.GetComponent<Character_Base_Script>();
            character.Setup(isPlayer);
        }

        return character;
    }

    private void CheckSceneTrigger()
    {
        if (playerCharacter.transform.position.x == 1.5 && playerCharacter.transform.position.y == -1.5)
        {
            state = State.Sound;
            if (!triggerSound.isPlaying)
            {
                triggerSound.PlayOneShot(clip);
            }
            state = State.Cutscene;
            StartCoroutine(ExampleCoroutine());
           

           
        }
       
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator ExampleCoroutine()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(7);
        state = State.End;
        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }
}

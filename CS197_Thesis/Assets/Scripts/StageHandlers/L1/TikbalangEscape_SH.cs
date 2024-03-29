﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TikbalangEscape_SH : MonoBehaviour
{
    private static TikbalangEscape_SH stageHandler;

    public static TikbalangEscape_SH getInstance()
    {
        return stageHandler;
    }

    [SerializeField] private Transform pf_Character_Base;
    //[SerializeField] private Transform pf_Enemy_Base; //TODO change this to tikbalang prefab
    //public AudioSource triggerSound;
    //public AudioClip clip;

    private Character_Base_Script playerCharacter;
    //private Character_Base_Script tikbalang;
    private State state;

    int[] map = new int[]
    {
        1,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,

        1,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,  1,
        1,  1, 0, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0,  0,
        1,  1, 0, 1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1,  1,
        1,  1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1,  1,
        1,  1, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1,  1,
        1,  1, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 1, 1, 1,  1,
        1,  1, 1, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 1, 1, 1, 1,  1,
        1,  1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,  1,
        1,  1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,  1,
        1,  1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,  1,
        1,  1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,  1,
        1,  1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,  1,

        1,  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,  1
    };

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
        //tikbalang = SpawnCharacters(false);
        //triggerSound = GetComponent<AudioSource>();
        playerCharacter.UpdatePosition(136);
        Debug.Log("player position:" + playerCharacter.ReturnPosition());
    }

    // Update is called once per frame
    private void Update()
    {

        if (state == State.MovementPhase)
        {
            MovementPhase();
            CheckEndTrigger();
        }




        if (state == State.Cutscene)
        {
            //TkEncounter_Image_Window.DisplayImages();
            //TODO pop up should appear here
        }

        if (state == State.End)
        {
            //Application.Quit();
            LoadNextScene();
        }

    }


    private Character_Base_Script SpawnCharacters(bool isPlayer)
    {
        Vector3 position;
        Character_Base_Script character;
       
            position = new Vector3(1.5f, -0.5f);
            Transform characterTransform = Instantiate(pf_Character_Base, position, Quaternion.identity);
            character = characterTransform.GetComponent<Character_Base_Script>();
            character.Setup(isPlayer);
        
        
        

        return character;
    }

    private void CheckEndTrigger()
    {
        /*if (playerCharacter.ReturnPosition() == 136 || playerCharacter.ReturnPosition() == 154)
        {
            state = State.Sound;
            if (!triggerSound.isPlaying)
            {
                triggerSound.PlayOneShot(clip);
            }
            state = State.Cutscene;
            StartCoroutine(NextSceneCoroutine());
        }*/

        if(playerCharacter.ReturnPosition() == 53)
        {
            state = State.End;
        }

    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(4);
    }

    IEnumerator NextSceneCoroutine()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(7);
        state = State.End;
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

        else
        {
            return true;
        }
    }
}

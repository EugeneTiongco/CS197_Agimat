using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Handler_Script : MonoBehaviour
{
    private static Stage_Handler_Script stageHandler;

    public static Stage_Handler_Script GetInstance()
    {
        return stageHandler;
    }

    [SerializeField] private Transform pf_Character_Base;
    [SerializeField] private Transform pf_Enemy_Base;

    private Character_Base_Script playerCharacter;
    private Character_Base_Script enemyCharacter;
    private Character_Base_Script activeCharacter;
    private State state;

    private enum State
    {
        PlayerTurn, EnemyTurn, GamePhase, GameOver,
    }

    private void Awake()
    {
        stageHandler = this;

    }
    private void Start()
    {
        playerCharacter = SpawnCharacters(true);
        enemyCharacter = SpawnCharacters(false);
        SetActiveCharacter(playerCharacter);
        Turn_Window.Show_Static("Player Turn!");
        state = State.GamePhase;

    }

    private void Update()
    {
        CheckGameOver();
        int actionPoints = activeCharacter.ReturnActionPoints();
        if (state == State.GamePhase)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (actionPoints > 0)
                {
                    activeCharacter.transform.position = new Vector3(activeCharacter.transform.position.x, activeCharacter.transform.position.y - 1);
                    actionPoints--;
                    activeCharacter.UpdateActionPoints(actionPoints);
                    Debug.Log(actionPoints);
                    if (activeCharacter.ReturnActionPoints() == 0)
                        ChooseNextActiveCharacter();
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                if (actionPoints > 0)
                {
                    activeCharacter.transform.position = new Vector3(activeCharacter.transform.position.x, activeCharacter.transform.position.y + 1);
                    actionPoints--;
                    activeCharacter.UpdateActionPoints(actionPoints);
                    Debug.Log(actionPoints);
                    if (activeCharacter.ReturnActionPoints() == 0)
                        ChooseNextActiveCharacter();
                }
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (actionPoints > 0)
                {
                    activeCharacter.transform.position = new Vector3(activeCharacter.transform.position.x - 1, activeCharacter.transform.position.y);
                    actionPoints--;
                    activeCharacter.UpdateActionPoints(actionPoints);
                    Debug.Log(actionPoints);
                    if (activeCharacter.ReturnActionPoints() == 0)
                        ChooseNextActiveCharacter();
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (actionPoints > 0)
                {
                    activeCharacter.transform.position = new Vector3(activeCharacter.transform.position.x + 1, activeCharacter.transform.position.y);
                    actionPoints--;
                    activeCharacter.UpdateActionPoints(actionPoints);
                    Debug.Log(actionPoints);
                    if (activeCharacter.ReturnActionPoints() == 0)
                        ChooseNextActiveCharacter();
                }
            }
        }
    }

    private Character_Base_Script SpawnCharacters(bool isPlayer)
    {
        Vector3 position;
        Character_Base_Script character;
        if (isPlayer)
        {
            position = new Vector3(-5, 0);
            Transform characterTransform = Instantiate(pf_Character_Base, position, Quaternion.identity);
            character = characterTransform.GetComponent <Character_Base_Script>();
            character.Setup(isPlayer);
        }
        else
        {
            position = new Vector3(+5, 0);
            Transform characterTransform = Instantiate(pf_Enemy_Base, position, Quaternion.identity);
            character = characterTransform.GetComponent<Character_Base_Script>();
            character.Setup(isPlayer);
        }

        return character;
    }

    private void SetActiveCharacter (Character_Base_Script character)
    {
        activeCharacter = character;
    }

    private void ChooseNextActiveCharacter()
    {
        if(activeCharacter == playerCharacter)
        {
            SetActiveCharacter(enemyCharacter);
            activeCharacter.SetActionPoints(false);
            Turn_Window.Show_Static("Enemy Turn!");
        }
        else
        {
            SetActiveCharacter(playerCharacter);
            activeCharacter.SetActionPoints(true);
            Turn_Window.Show_Static("Player Turn!");
        }
    }

    private void CheckGameOver()
    {

        if (enemyCharacter.transform.position == playerCharacter.transform.position)
        {
            Turn_Window.Show_Static("Game Over!");
            state = State.GameOver;
        }

    }


}

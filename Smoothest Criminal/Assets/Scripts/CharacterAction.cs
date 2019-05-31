using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class CharacterAction : MonoBehaviour
{
    public float length = 1; // in seconds
    public enum actions
    {
        Move,
        Stop,
        Jump,
        TurnAround
    }
    public actions action = actions.Stop;

    public Vector2 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2(0.5f * length, originalScale.y);

        
    }
}*/

public class CharacterAction
{

    public enum Action
    {
        Move,
        Stop,
        Jump,
        TurnAround
    }
    public Action action = Action.Stop;

    public CharacterAction(Action action)
    {
        this.action = action;
    }

    public void Start()
    {
        PlayerController player = GameObject.FindObjectOfType<PlayerController>();

        switch (action)
        {
            case Action.Move:
                player.h_input = 1;
                break;

            case Action.Jump:
                player.doJump = true;
                break;

            case Action.TurnAround:
                player.facing *= -1;
                break;

            case Action.Stop:
                player.h_input = 0;
                player.doJump = false;
                break;
        }
    }

    public void End()
    {
        PlayerController player = GameObject.FindObjectOfType<PlayerController>();

        switch (action)
        {
            case Action.Move:
                player.h_input = 0;
                break;

            case Action.Jump:
                player.doJump = false;
                break;

            case Action.Stop:
                player.h_input = 1 * player.facing;
                break;
        }
    }
}

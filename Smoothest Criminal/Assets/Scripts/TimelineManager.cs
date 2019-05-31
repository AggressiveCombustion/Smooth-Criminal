using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineManager : MonoBehaviour
{
    public int currentSelectedFrame = 1;

    public int currentAction = 0;

    public int page = 1;

    public Dictionary<int, int> frameRegistry; // <frame, action>

    public Button[] frames;

    public int pages = 3;

    // Start is called before the first frame update
    void Start()
    {
        frameRegistry = new Dictionary<int, int>();
        for (int i = 0; i < 20 * pages; i++)
        {
            frameRegistry.Add(i, -1);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetAction(int index)
    {
        currentAction = index;
        Debug.Log("CurrentAction: " + index);
    }

    // add action to whichever frame we clicked
    public void SetFrame(int frame)
    {
        int index = frame + ((page - 1) * 20);

        //currentSelectedFrame = index;
        if (frameRegistry.ContainsKey(index))
        {
            frameRegistry[index] = currentAction;
        }
        else
            frameRegistry.Add(index, currentAction);

        Debug.Log("Set " + index + " to " + currentAction);
        
        UpdateIcons();
    }

    public void NextPage()
    {
        page += 1;

        if (page > pages)
            page = pages;

        UpdateIcons();
    }

    public void PrevPage()
    {
        page -= 1;

        if (page < 1)
            page = 1;

        UpdateIcons();
    }

    public void Play()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        player.transform.position = player.startPos;
        player.h_input = 1;

        Timeline t = FindObjectOfType<Timeline>();
        t.timeAction.Clear();

        foreach (KeyValuePair<int, int> kvp in frameRegistry)
        {
            CharacterAction.Action action = CharacterAction.Action.Stop;
            switch (kvp.Value)
            {
                case -1:
                    action = CharacterAction.Action.Move;
                    break;
                case 0:
                    action = CharacterAction.Action.Stop;
                    break;
                case 1:
                    action = CharacterAction.Action.Move;
                    break;
                case 2:
                    action = CharacterAction.Action.Jump;
                    break;
            }

            //Debug.Log(kvp.Key + " = " + kvp.Value);

            CharacterAction ca = new CharacterAction(action);

            t.timeAction.Add(kvp.Key, ca);
        }

        t.isPlaying = true;
    }

    void UpdateIcons()
    {
        for(int i = 0; i < 20; i++)
        {
            int frame = i + (20 * (page-1));
            Color newColor = Color.white;

            int action = frameRegistry[frame];

            switch (action)
            {
                case 0:
                    newColor = Color.white;
                    break;
                case 1:
                    newColor = Color.green;
                    break;
                case 2:
                    newColor = Color.blue;
                    break;
            }

            frames[i].GetComponent<Image>().color = newColor;
        }
    }
}

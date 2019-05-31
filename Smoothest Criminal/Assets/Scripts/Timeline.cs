using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    /*public float elapsedTime = 0.0f;
    public Dictionary<float, CharacterAction> timeAction;

    float lowest = 999.0f;
    private bool shouldLook = true;

    bool started = false;
    float startTime;
    */

    public float elapsedTime = 0.0f;
    public Dictionary<float, CharacterAction> timeAction;

    public bool isPlaying = false;

    int startTime = 999;
    private CharacterAction.Action prevAction;


    // Start is called before the first frame update
    void Awake()
    {
        timeAction = new Dictionary<float, CharacterAction>();
    }

    // Update is called once per frame
    /*void Update()
    {
        elapsedTime += Time.deltaTime * GameManager.instance.speed;

        if (shouldLook)
        {
            foreach (KeyValuePair<float, CharacterAction> kvp in timeAction)
            {
                if (kvp.Key < lowest)
                {
                    lowest = kvp.Key;
                    shouldLook = false;
                    
                    Debug.Log("Lowest = " + kvp.Key);
                }
            }
        }

        if (!timeAction.ContainsKey(lowest))
            return;
        
        if (elapsedTime >= lowest && !started)
        {
            // do action tied to timeAction[lowest]
            started = true;
            timeAction[lowest].Start();
            Debug.Log("Do a thing");
            startTime = elapsedTime;
        }

        if(elapsedTime >= startTime + timeAction[lowest].length && started)
        {
            timeAction[lowest].End();
            timeAction.Remove(lowest);
            shouldLook = true;
            Debug.Log("Done doing a thing");
            startTime = 0;
            started = false;
            lowest = 999;
        }
       
    }*/

    void Update()
    {
        if(isPlaying)
        {
            elapsedTime += Time.deltaTime * GameManager.instance.speed;
            int frame = (int)elapsedTime;

            // we've reached the end of the frame, so go to the next one
            if (elapsedTime > startTime + 1)
            {
                // is it the same type of frame?
                CharacterAction.Action newAction = timeAction[(int)elapsedTime].action;

                // if not, stop the previous one
                if (newAction != prevAction)
                {
                    timeAction[frame].End();
                }
            }

            if (timeAction.ContainsKey(frame)) // cut off decimal
            {
                timeAction[frame].Start();
                startTime = frame;
                prevAction = timeAction[frame].action;

                // remove the frame so it doesn't play again
                timeAction.Remove(frame);
            }

            
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Conductor : MonoBehaviour
{

    //public static Conductor instance;
    public int bpm;
    float lastbeat; //this is the ‘moving reference point’
    public float crotchet;
    public double startTime;
    public float songPosition;
    public float offset;
    public float pitch = 1;

    public float seconds = 244;

    public Text timeText;

    public float elapsed = 0;

    public Transform failPanel;

    [Header("Events")]
    public OnBeatEvent onBeat;

    // Use this for initialization
    void Start()
    {
        //instance = this;
        startTime = AudioSettings.dspTime;
        
        crotchet = 60.0f / bpm;
    }

    // Update is called once per frame
    void Update()
    {
        songPosition = ((float)(AudioSettings.dspTime - startTime) * pitch) - offset;
        //timeText.text = TimeFormat();

        seconds -= Time.deltaTime;

        if (songPosition > lastbeat + crotchet)
        {
            onBeat.Invoke();
            lastbeat += crotchet;
        }
    }

    string TimeFormat()
    {
        // get total seconds
        int totalSeconds = Mathf.FloorToInt(seconds);

        // divide by 60 for minutes
        int min = totalSeconds / 60;

        // mod by 60 for remaining seconds
        int sec = totalSeconds % 60;

        return min.ToString("00") + ":" + sec.ToString("00");
    }

    public void DoFail()
    {
        failPanel.GetComponent<Animator>().SetTrigger("fail");
    }
}

[System.Serializable]
public class OnBeatEvent : UnityEvent
{

}


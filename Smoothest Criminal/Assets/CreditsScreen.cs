using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScreen : MonoBehaviour
{
    bool didConfirm = false;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<SceneTransitions>().Begin();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump") || Input.GetButton("Submit") ||
            Input.GetButton("Cancel"))
        {
            if (!didConfirm)
            {
                didConfirm = true;
                Confirm();
            }
        }
    }

    public void Confirm()
    {
        /*Timer t = new Timer(1.5f, ReturnToMenu);
        GameManager.instance.AddTimer(t, gameObject);*/
        ReturnToMenu();
        FindObjectOfType<SceneTransitions>().End();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("title");
    }
}

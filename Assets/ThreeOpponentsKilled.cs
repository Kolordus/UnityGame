using UnityEngine;
using UnityEngine.UI;

public class ThreeOpponentsKilled : MonoBehaviour
{
    public string prompt;
    public GameObject canvasPrompt;
    private Canvas canvas;
    private PlayerControllerTransform player;
    private string text;
    
    void Start()
    {
        canvasPrompt = GameObject.FindGameObjectWithTag("Prompt");
        canvas = canvasPrompt.GetComponent<Canvas>();
        canvas.enabled = false;
        
    }

    private void Update()
    {
        if (PlayerControllerTransform.killedOpponents == 3 && PlayerControllerTransform.areOppKilled)
        {
            setPrompt("You've killed all the opponents!");
            canvas.enabled = true;
            
            if (Input.GetKey(KeyCode.Return))
            {
                canvas.enabled = false;
                PlayerControllerTransform.killedOpponents = 0;
            }
        }
        else
        {
            PlayerControllerTransform.killedOpponents = 0;
        }
    }


    private void setPrompt(string text)
    {
        GameObject.FindGameObjectWithTag("enterPrompt").GetComponent<Transform>().GetComponentInChildren<Text>().text = "ENTER";

        canvasPrompt.GetComponent<Transform>().GetComponentInChildren<Text>().text = text;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour
{
    public Canvas canvas;
    private GameObject[] players;
    private string text;

    void Start()
    {
        canvas.enabled = false;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            players = GameObject.FindGameObjectsWithTag("playerMulti");

            int i = 1;
            foreach (var player in players)
            {
                var playerMulti = player.GetComponent<playerMulti>();
                
                if (playerMulti.livesLeft == 0)
                {
                    text += "Player " + i + "           defeated!";
                }

                else
                {
                    text += "Player " + i + "                                      " + playerMulti.livesLeft + "\n";
                }
                
                i++;

            }
            
            setPrompt(text);
            text = "";
            canvas.enabled = true;
        }

        else
        {
            canvas.enabled = false;
            text = "";
        }
    }
    
    
    private void setPrompt(string text)
    {
        canvas.GetComponent<Transform>().GetComponentInChildren<Text>().text = text;
    }
    
}

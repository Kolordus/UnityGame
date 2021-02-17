using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainmenuscript : MonoBehaviour

{
	public Button btnStart;
	public Button btnExit;
    private Canvas manuUI;

    private void Start()
    {
		manuUI = (Canvas)GetComponent<Canvas>();

		btnStart = btnStart.GetComponent<Button>();//Ustawienie przycisku uruchomienia gry.
		btnExit = btnExit.GetComponent<Button>();//Ustawienie przycisku wyjścia z gry.

		Cursor.visible = manuUI.enabled;
		
	}


	public void SinglePlayer()
    {
	    SceneManager.LoadScene("Cave");
	}

	public void Multiplayer()
    {
	    SceneManager.LoadScene("Multiplayer");
	}

}

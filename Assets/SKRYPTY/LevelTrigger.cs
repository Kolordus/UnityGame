using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTrigger : MonoBehaviour
{
    public string sceneName;
    public string prompt;
    public string startNr;
    private GameObject canvasPrompt;
    private Canvas canvas;

    void Start()
    {
        canvasPrompt = GameObject.FindGameObjectWithTag("Prompt");
        canvas = canvasPrompt.GetComponent<Canvas>();
        canvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            startNr = "startNr" + Random.Range(1, 4).ToString();
            canvas.enabled = true;
            setPrompt();
        }
    }
    private void OnTriggerStay(Collider other) {
        if (Input.GetKeyDown(KeyCode.Return)) { // enter
            GraczInstancja.startNr = startNr;
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            canvas.enabled = false;
            setPrompt();
        }
    }

    void setPrompt() {
        GameObject.FindGameObjectWithTag("enterPrompt").GetComponent<Transform>().GetComponentInChildren<Text>().text = "ENTER";
        canvasPrompt.GetComponent<Transform>().GetComponentInChildren<Text>().text = prompt;
    } 
}

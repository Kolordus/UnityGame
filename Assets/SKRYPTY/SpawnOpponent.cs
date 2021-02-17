using System.Collections.Generic;
using UnityEngine;

public class SpawnOpponent : MonoBehaviour
{
    private GameObject opponent;
    float timer = 0.0f;
    public int seconds = 0;
    private int spawnPoint = 1;
    public GameObject[] resourceSpawnPoints;
    bool isOpponent1 = false;
    bool isOpponent2 = false;
    bool isOpponent3 = false;
    private List<int> alreadyInstatieted = new List<int>();

    public GameObject opponent1;
    public GameObject opponent2;
    public GameObject opponent3;

    private int SECONDS_TO_FIRST_OPPONENT = 30;
    private int SECONDS_TO_SECOND_OPPONENT = 120;
    private int SECONDS_TO_THIRD_OPPONENT = 240;
    // Update is called once per frameN

    private void Start()
    {
        
        resourceSpawnPoints = GameObject.FindGameObjectsWithTag("resourceSpawnPoint");
    }
    
    void Update()
    {
       
        timer += Time.deltaTime;
        seconds = (int) timer;

        if (seconds == SECONDS_TO_FIRST_OPPONENT && isOpponent1 == false)
        {
            isOpponent1 = true;
            instatiateOpponent(opponent1);
        }

        if (seconds > SECONDS_TO_SECOND_OPPONENT && isOpponent2 == false)
        {
            isOpponent2 = true;
            instatiateOpponent(opponent2);
        }

        if (seconds > SECONDS_TO_THIRD_OPPONENT && isOpponent3 == false)
        {
            isOpponent3 = true;
            instatiateOpponent(opponent3);
        }
    }
    
    private void instatiateOpponent(GameObject opponent)
    {
        spawnPoint = Random.Range(0, resourceSpawnPoints.Length);
        opponent.transform.position = resourceSpawnPoints[spawnPoint].transform.position;
        Instantiate(opponent);
    }
}

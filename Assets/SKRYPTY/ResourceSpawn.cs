using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourceSpawn : NetworkBehaviour
{
    private int spawnPoint = 1;
    public GameObject resource;
    public GameObject[] resourceSpawnPoints;


    void Start()
    {
        resourceSpawnPoints = GameObject.FindGameObjectsWithTag("resourceSpawnPoint");
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.Equals("Multiplayer") && NetworkServer.connections.Count > 0)
            InvokeRepeating("CmdSpawnResource", 1.0f, 10.0f);
        else
            InvokeRepeating("spawnResource", 15.0f, 25.0f);
    }

    void spawnResource()
    {
        GameObject _resource = Instantiate(resource); 
        spawnPoint = Random.Range(0, resourceSpawnPoints.Length);
        _resource.transform.position = resourceSpawnPoints[spawnPoint].transform.position;
        ;
    }


    [Command]
    void CmdSpawnResource()
    {
        GameObject _resource = Instantiate(resource); 
        spawnPoint = Random.Range(0, resourceSpawnPoints.Length);
        _resource.transform.position = resourceSpawnPoints[spawnPoint].transform.position;
        NetworkServer.Spawn(_resource);
    }
}

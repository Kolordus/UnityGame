using UnityEngine;
using UnityEngine.SceneManagement;

public class GraczInstancja : MonoBehaviour
{
    public static GraczInstancja instancja;
    public static string startNr;
   
    private void Awake()
    {
        if (!instancja)
        {
            DontDestroyOnLoad(this.gameObject);
            instancja = this;
        }
        else
            Destroy(gameObject);
    }
}

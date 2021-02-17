using UnityEngine;

public class InterfejsInstancja : MonoBehaviour
{
    public static InterfejsInstancja instancja;



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

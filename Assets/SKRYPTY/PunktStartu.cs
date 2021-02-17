using UnityEngine;

public class PunktStartu : MonoBehaviour
{
    private Transform trans;
    private bool startUstawiony;


    private void Start()
    {
        trans = GetComponent<Transform>();
    }

    private void Update()
    {
        if (!startUstawiony)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                GameObject start = null;

                if (GraczInstancja.startNr != null && !GraczInstancja.startNr.Equals(""))
                {
                    start = GameObject.FindGameObjectWithTag(GraczInstancja.startNr);
                }

                Vector3 pos = trans.position;
                if (start != null)
                {
                    pos = start.GetComponent<Transform>().position;
                }

                player.GetComponent<Transform>().position = pos;

                startUstawiony = true;
            }
        }
    }
}

using UnityEngine;

public class AimCameraScript : MonoBehaviour
{

    public GameObject normalCamera;
    public GameObject aimCamera;
    
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            this.transform.position = aimCamera.transform.position;
            this.transform.rotation = aimCamera.transform.rotation;
            
        }

        else
        {
            this.transform.position = normalCamera.transform.position;
            this.transform.rotation = normalCamera.transform.rotation;
        }
    }
}

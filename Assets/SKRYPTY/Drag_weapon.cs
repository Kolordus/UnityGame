using UnityEngine;

public class Drag_weapon : MonoBehaviour
{
    public GameObject M16;
    public GameObject M1;
    public GameObject M98;
    
    public GameObject bodyType;
    public Transform parent;

    public GameObject bodyTypeBack;
    public Transform parentBack;

    public GameObject pickObj;
    public int siblingIndex;

    // set parent, calculate distance

    private void Start()
    {
        if (M16.activeSelf)
        {
            pickObj = M16;
            siblingIndex = 0;
        }
            
        else if (M1.activeSelf)
        {
            pickObj = M1;
            siblingIndex = 1;
        }
            
        else if (M98.activeSelf)
        {
            pickObj = M98;
            siblingIndex = 2;
        }
            
    }

    private void Update()
    {
        if (M16.activeSelf)
        {
            pickObj = M16;
            siblingIndex = 0;
        }
            
        else if (M1.activeSelf)
        {
            pickObj = M1;
            siblingIndex = 1;
        }
            
        else if (M98.activeSelf)
        {
            pickObj = M98;
            siblingIndex = 2;
        }
            
    }

    public void onPickUp()
    {
        // Debug.Log(pickObj);
        if(pickObj != null)
        {
            // distance from weapon from hand
            Vector3 distance = pickObj.transform.position - bodyType.transform.position;
            float magnitude = distance.magnitude;

            // Debug.Log(magnitude);
            if(magnitude <= 20f)
            {
                
                pickObj.transform.SetParent(parent);
                pickObj.transform.localPosition = Vector3.zero;
                pickObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void onDrop()
    {
        if(pickObj != null)
        {
            pickObj.transform.SetParent(parentBack);
            pickObj.transform.SetSiblingIndex(siblingIndex);
            pickObj.transform.localPosition = Vector3.zero;
            pickObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}

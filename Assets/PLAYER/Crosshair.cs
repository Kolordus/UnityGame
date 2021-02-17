using UnityEngine;

public class Crosshair : MonoBehaviour
{
   private LineRenderer lr;
   public GameObject owner;

   private void Start()
   {
      lr = GetComponent<LineRenderer>();
   }

   private void Update()
   {
      
      if (!owner) return;

      lr.enabled = false;
      if (Input.GetMouseButton(1))
      {
         lr.enabled = true;
      }
      
   }
}

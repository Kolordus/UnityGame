using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairForMultiPlayer : MonoBehaviour
{
    private LineRenderer lr;
    private RaycastHit hit;
    public GameObject owner;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
      
    }

    private void Update()
    {
        if (owner)
        {
            if (Input.GetMouseButton(1))
            {
                lr.enabled = true;
                lr.SetPosition(0, transform.position);
         
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    if (hit.collider)
                    {
                        lr.SetPosition(1, hit.point);
                    }
                }
                else
                {
                    lr.SetPosition(1, transform.forward * 5000 );
                }
            }

            else
            {
                lr.enabled = false;
            }
        }
    }
}

using UnityEngine;
using System.Collections;

public class StrzalPistolet : MonoBehaviour {

	public float zasieg = 100.0f;
	public GameObject pociskPrefab;
	public float obrazenia = 50.0f;

    void Start() {        
    }
    public void strzal () {
	    Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			RaycastHit hitInfo;

            //Sprawdzenie czy promien w cos trafil czy obiekt (hitInfo) mieszczacy sie w zakresie (range)
            // w cos trafiły.
            //if (Physics.Raycast(ray, out hitInfo, zasieg, warstwaDoIgnorowania)) {
            if (Physics.Raycast(ray, out hitInfo, zasieg)){
				Vector3 hitPoint = hitInfo.point;
				
				GameObject go = hitInfo.collider.gameObject;
				
				Debug.Log("Hit Object: " + go.name);
				//Debug.Log("Hit Point: " + hitPoint);//Wspolrzedne trafionego obiektu.
				
				hit(go);

				if(pociskPrefab != null){
					Instantiate(pociskPrefab, hitPoint, Quaternion.identity);
				}
		}
	}   
    void hit( GameObject go){
		Zdrowie zdrowie = go.GetComponent<Zdrowie>();
		if(zdrowie != null) {
			zdrowie.otrzymaneObrazenia(obrazenia);
		}
	}
}

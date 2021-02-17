using UnityEngine;

public class SamoZniszczenie : MonoBehaviour {

	public float timeToLive = 2f;

	void Update () {
		timeToLive -= Time.deltaTime;

		if(timeToLive <= 0){
			Destroy(gameObject);
		}
	}

}

using UnityEngine;

public class Zdrowie : MonoBehaviour {

	public int zdrowie = 100;

	public virtual void otrzymaneObrazenia(float obrazenia) {
		Debug.Log(obrazenia);
		if (zdrowie > 0) {
			zdrowie -= (int)obrazenia;
			Debug.Log(zdrowie);
			if(zdrowie < 0)
				zdrowie = 0;
		}

		if (zdrowie <=0)
			Die();
	}

	public void Die(){
		Destroy(gameObject);	
	}

	public bool czyMartwy(){
		if (zdrowie <= 0) {
			return true;
		}
		return false;
	}

    public virtual void setZdrowie(int zdrowie) {
        this.zdrowie = zdrowie;
    }

}

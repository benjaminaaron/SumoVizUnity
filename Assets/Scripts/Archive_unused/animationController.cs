using UnityEngine;
using System.Collections;

public class animationController : MonoBehaviour {





	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "pedestrian")
			other.gameObject.GetComponent<Pedestrian>().setAnim(true);
	}


	void OnTriggerExit(Collider other){
		//Debug.Log("klappt");
		if(other.gameObject.tag == "pedestrian")
		other.gameObject.GetComponent<Pedestrian>().setAnim(false);

	}



}

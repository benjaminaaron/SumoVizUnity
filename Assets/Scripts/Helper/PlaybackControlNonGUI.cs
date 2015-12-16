using UnityEngine;
using System.Collections;

public class PlaybackControlNonGUI : MonoBehaviour{

	public bool playing = true;
	public float current_time; 
	//public decimal current_time; 
	public decimal total_time;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () {

		
		if (playing) {
			try {
				current_time = (current_time +  Time.deltaTime);
			} catch (System.DivideByZeroException) {
				current_time = 0;
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			playing = !playing;
		}
	
	}
}

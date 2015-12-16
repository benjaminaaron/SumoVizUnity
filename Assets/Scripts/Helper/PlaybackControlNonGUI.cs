using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaybackControlNonGUI : MonoBehaviour {

	public bool playing = true;
	public decimal current_time;
	public decimal total_time = 0;

	public List<decimal> deltas = new List<decimal> ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		
		if (playing) {
			try {
				decimal delta = (decimal) Time.deltaTime;
				deltas.Add(delta);
				current_time = (current_time + delta) % total_time;

				/*Debug.Log(current_time);
				Debug.Log (total_time);
				Debug.Log (delta);
				Debug.Log ("");*/

			} catch (System.DivideByZeroException) {
				current_time = 0;
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			playing = !playing;
		}
	
	}
}

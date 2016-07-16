using UnityEngine;
using System.Collections;

public class PlaybackControlNonGUI : MonoBehaviour {

	public bool playing = true;
	public float current_time;
	public float total_time = 0;

	void Start () {
		//Time.captureFramerate = 25;
	}

	int count = 0;

	void Update () {

		//comment in or out for taking screenshots at the resolution of the current window-size
		//Application.CaptureScreenshot ("Screenshots/pic" + (count ++) + ".png", 1);
		
		if (playing) {
			try {
				current_time = (current_time + Time.deltaTime) % total_time;
				//Debug.Log(current_time);
			} catch (System.DivideByZeroException) {
				current_time = 0;
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			playing = !playing;
		}
	
	}
}

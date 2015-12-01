using UnityEngine;
using System.Collections;

public class AgentView : MonoBehaviour
{

	//pedestrian that is currently followed
	private GameObject currentPedestrian = null;


	// Use this for initialization
	void Start ()
	{
	}
	
	// LateUpdate is called after all Update functions have been called. 
	void LateUpdate ()
	{

		if (currentPedestrian != null) {

			followPedestrian (currentPedestrian);
		} else {

			Debug.Log ("No valid pedestrian found");
			findRandomPedestrian ();
		}

		if (Cardboard.SDK.Triggered) {
			findRandomPedestrian ();

		}
		
	}

	/** Move the camera relative to the position of the pedestrian that is followed.
	 */
	private void followPedestrian (GameObject pedestrian)
	{

		Vector3 pedestrianPosition = pedestrian.transform.position;
		Vector3 newPosition = new Vector3 (pedestrianPosition.x, transform.position.y, pedestrianPosition.z); // not changing Y value
		transform.position = newPosition;

	}

	/** Find a random pedestrian from a list of GameObjected tagged with "pedestrian".
	 * Set 'currentPedestrian' to that random pedestrian only if hes is still active (moving).
	 */
	private void findRandomPedestrian ()
	{

		GameObject[] pedestrians = GameObject.FindGameObjectsWithTag ("pedestrian"); 

		if (pedestrians.Length == 0) {
			Debug.LogError ("No game objects are tagged with pedestrian");
		} else {

			System.Random random = new System.Random ();
			int position = random.Next (1, pedestrians.Length);
			bool isPedActive = pedestrians [position].GetComponentInChildren<Pedestrian> ().isActive ();

			while (!isPedActive) {
				position = random.Next (1, pedestrians.Length);
				isPedActive = pedestrians [position].GetComponentInChildren<Pedestrian> ().isActive ();
				Debug.Log ("Set new pedestrian to follow : " + currentPedestrian + ". Is active : " + isPedActive);
			}

			currentPedestrian = pedestrians [position];
			Debug.Log ("Set new pedestrian to follow : " + currentPedestrian + ". Is active : " + isPedActive);

		}
					
	}

//	private void findActivePedestrian ()
//	{
//
//		GameObject[] pedestrians;
//		pedestrians = GameObject.FindGameObjectsWithTag ("pedestrian"); 
//		
//		
//		if (pedestrians.Length == 0) {
//			Debug.LogWarning ("No game objects are tagged with pedestrian");
//		} else {
//
//			foreach (GameObject pedestrian in pedestrians) {
//				if (pedestrian.activeSelf) {
//					currentPedestrian = pedestrian;
//					Debug.Log ("Set new pedestrian to follow : " + currentPedestrian
//					);
//					return;
//
//				}
//			}
//		}
//	}
}

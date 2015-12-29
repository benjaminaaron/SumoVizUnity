using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class Pedestrian : MonoBehaviour {
	
	Vector3 start;
	Vector3 target;
	float movement_time_total;
	float movement_time_elapsed;
	bool animOn = false;
	private float speed;
	//to optimize the getTrait loop
	//private int currentTrait;

	private LinkedListNode<PedestrianPosition> iterator;
	private LinkedListNode<PedestrianPosition> last;
	public const float reducedStepTime = 0.1f;
	//public float lastTime;

	private PedestrianPosition lastPos;

	int id;
	//SortedList positions = new SortedList ();
	private LinkedList<PedestrianPosition> positions = new LinkedList<PedestrianPosition>();
	Color myColor;
	//bool trajectoryVisible;
	//VectorLine trajectory;

	//private InfoText it;

	private PlaybackControlNonGUI pc;
	private Renderer r;

	private bool active =true;

	// Use this for initialization
	void Start () {

		last = positions.Last;
		iterator = positions.First;
		//gameObject.AddComponent<BoxCollider>();
		transform.Rotate (0,90,0);
		//sets the color for the pedestrians
		/*myColor = Color.red;
		GetComponentInChildren<Renderer>().materials[1].color = myColor;*/

		pc = GameObject.Find ("PlaybackControl").GetComponent<PlaybackControlNonGUI> ();
		r = GetComponentInChildren<Renderer>() as Renderer;
	
		//set Tag of the game object in order to find gameobjects with the same tag
		//gameObject.tag = "pedestrian";	
	}

	// Update is called once per frame
	void Update () {

		/*if (pc.playing) {
			GetComponent<Animation>().Play ();
		/*} else {
			GetComponent<Animation>().Stop ();
		}*/

		//int index = _getTrait(positions, pc.current_time);



			
		LinkedListNode<PedestrianPosition> cur = _getTrait2 (pc.current_time);



			//Debug.Log("x:\t" +cur.Value.getX() +"y:\t"+ cur.Value.getY() + "time:\t"+cur.Value.getTime() + "id:\t" + id);
			if (cur != null && cur != last) {

			iterator = cur;

				if(!isActive()){
					active = true;
					r.enabled = true;
				}

			//r.enabled = true;


				if(animOn){
				GetComponent<Animation>().Play();
			}else{
				GetComponent<Animation>().Stop();
			}



				if (r.isVisible) {

				//if(animOn){
				//bool necessaryToTransform = (pc.current_time - lastTime) > reducedStepTime;

							//GetComponent<Animation>().Play ();
							//lastTime = pc.current_time;
							//if (index < positions.Count - 1 && index > -1){
							
							//PedestrianPosition pos = (PedestrianPosition) positions.GetByIndex (index);
							//PedestrianPosition pos2 = (PedestrianPosition) positions.GetByIndex (index+1);
							
							PedestrianPosition pos = (PedestrianPosition)cur.Value;
							PedestrianPosition pos2 = (PedestrianPosition)cur.Next.Value;
							start = new Vector3 (pos.getX (), 0, pos.getY ());
							target = new Vector3 (pos2.getX (), 0, pos2.getY ());
							float time = pc.current_time;
							float timeStepLength = Mathf.Clamp (pos2.getTime () - pos.getTime (), 0.1f, 50f); // We don't want to divide by zero. OTOH, this results in pedestrians never standing still.
							float movement_percentage = (time - pos.getTime ()) / timeStepLength;
							Vector3 newPosition = Vector3.Lerp (start, target, movement_percentage);
							//Debug.Log("Pos:/tx:" + newPosition.x +"y:/t" +newPosition.z + "id:/t" + id);
							
							gameObject.hideFlags = HideFlags.None;
							transform.position = newPosition;
				
							


							if (pos != lastPos) {
								lastPos = pos;

								Vector3 relativePos = target - start;
					
									
					
								speed = relativePos.magnitude;
								//if (start != target)
								transform.rotation = Quaternion.LookRotation (relativePos);
								

									
								GetComponent<Animation> () ["MaleArm|Walking"].speed = getSpeed () / timeStepLength;

							}

							
					/*}else {
						if(GetComponent<Animation>().isPlaying)
						GetComponent<Animation> ().Stop ();
			
					}*/
		


				
				/*else if(necessaryToTransform){
						lastTime = pc.current_time;
						//if (index < positions.Count - 1 && index > -1){
						active = true;
						r.enabled = true;
						//PedestrianPosition pos = (PedestrianPosition) positions.GetByIndex (index);
						//PedestrianPosition pos2 = (PedestrianPosition) positions.GetByIndex (index+1);
						
						PedestrianPosition pos = (PedestrianPosition)cur.Value;
						PedestrianPosition pos2 = (PedestrianPosition)cur.Next.Value;
						start = new Vector3 (pos.getX (), 0, pos.getY ());
						target = new Vector3 (pos2.getX (), 0, pos2.getY ());
						float time = pc.current_time;
						float timeStepLength = Mathf.Clamp (pos2.getTime () - pos.getTime (), 0.1f, 50f); // We don't want to divide by zero. OTOH, this results in pedestrians never standing still.
						float movement_percentage = (time - pos.getTime ()) / timeStepLength;
						Vector3 newPosition = Vector3.Lerp (start, target, movement_percentage);
						//Debug.Log("Pos:/tx:" + newPosition.x +"y:/t" +newPosition.z + "id:/t" + id);
						
						transform.position = newPosition;
						gameObject.hideFlags = HideFlags.None;
						
				}
			*/


		


			}



			
			

			} else {
			//Debug.Log("hide");
				//currentTrait = 0;
				iterator = positions.First;
			//Debug.Log(iterator.Value.getTime());
				active = false;
				r.enabled = false;
				gameObject.hideFlags = HideFlags.HideInHierarchy;
			}

	}


	public float getSpeed() {
		return speed;
	}
/*
	private int _getTrait(SortedList thisList, decimal thisValue) {
		/*while(currentTrait < thisList.Count){
			if ((decimal)thisList.GetKey(currentTrait)>=thisValue) return (currentTrait-1);
			++currentTrait;

		}
		return -1;



		for (int i = 0; i < thisList.Count; i ++) {
			if ((decimal) thisList.GetKey(i) > thisValue) 
				return (i - 1);
		}
		return -1;
	}
	*/


	public LinkedListNode<PedestrianPosition> _getTrait2(float thisValue){
		LinkedListNode<PedestrianPosition> cur = iterator;
		while ( cur.Next != null ){
			if(cur.Value.getTime() > thisValue){ 
				//if(cur.Previous == null) Debug.Log("prev = null");
				//Debug.Log ("next:/t" + cur.Value.getTime() + "thisvalue:\t" + thisValue);
				return cur.Previous;}
			
			cur = cur.Next;


		}
		//Debug.Log ("next:/t" + cur.Value.getTime() + "thisvalue:\t" + thisValue);
		return null;


}
	
	public void setID(int id) {
		this.id = id;
		this.name = "Pedestrian " + id;
	}

	public int getID(){
		return id;
	}

	public void setPositions(SortedList p) {
		positions.Clear();
		foreach (PedestrianPosition ped in p.Values) {
			positions.AddLast(ped);
		}
		PedestrianPosition pos = (PedestrianPosition)p.GetByIndex (0);
		transform.position = new Vector3 (pos.getX(), 0, pos.getY());
	}

	public bool isActive(){
		return active;
	}

	public void setAnim(bool onoff){
		//Debug.Log (onoff);
		animOn = onoff;
	}

}

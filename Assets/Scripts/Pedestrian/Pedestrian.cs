using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Pedestrian : MonoBehaviour {
	
	Vector3 start;
	Vector3 target;
	float movement_time_total;
	float movement_time_elapsed;
	private float speed;

	//public const float trajectoryInterpolationTimeStep = 0.1f;

	private float lastStepTime = 0;


	//to optimize the getTrait loop
	private int currentTrait;

	int id;





	//public List<Vector3> positions;
	private List<Vector3> positions;
	private Vector3 lastPos;


	//Color myColor;

	//bool trajectoryVisible;
	//VectorLine trajectory;


	//private InfoText it;

	private PlaybackControlNonGUI pc;
	private Renderer r;

	private bool active = true;


	/*
	void Awake(){

		ScriptablePositions pos = AssetDatabase.LoadAssetAtPath<ScriptablePositions>(@"Assets/Resources/savePositions/" +name+".asset");

	
		positions = pos.positions;



			BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.dataPath+ "/Resources/savePositions/" +"Pedestrian_" +GetInstanceID(),FileMode.Open);
			positions = (SortedList)bf.Deserialize(file);
			file.Close();

		}
		*/


	private Camera cam;
	private Plane [] planes;
	private Collider anyCollider;

	// Use this for initialization
	void Start () {

		cam = Camera.main;
		planes = GeometryUtility.CalculateFrustumPlanes(cam);
		anyCollider = gameObject.GetComponent<Collider>();

		gameObject.AddComponent<BoxCollider>();
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





	//	nessaryToCalculate = true;
		int index = _getTrait(positions, pc.current_time);
		currentTrait = index;

		//To reduce to often calculated frames

	

		
		if (index < positions.Count - 1 && index > -1 ){
			active = true;
			r.enabled = true;
			/*
			PedestrianPosition pos = (PedestrianPosition) positions.GetByIndex (index);
			PedestrianPosition pos2 = (PedestrianPosition) positions.GetByIndex (index+1);
			*/

		


				Vector3 pos = positions[index];
				Vector3 pos2 = positions[index+1];

				start = new Vector3 (pos.x, 0, pos.y);
				target = new Vector3 (pos2.x, 0, pos2.y);


				//bool nessaryToCalculate = (pc.current_time - lastStepTime) > trajectoryInterpolationTimeStep;
				//bool nessaryToCalculate = true;

				//float time =  pc.current_time;



				float timeStepLength = Mathf.Clamp(pos2.z - pos.z, 0.1f, 50f); // We don't want to divide by zero. OTOH, this results in pedestrians never standing still.
			
				//if(nessaryToCalculate){
				//lastStepTime = pc.current_time;


				//float movement_percentage = (time - pos.z) / timeStepLength;
				//Vector3 newPosition = Vector3.Lerp(start, target, movement_percentage);
				

				Vector3 newPosition = start;
				transform.position = newPosition;
				gameObject.hideFlags = HideFlags.None;
						
				//}


				//to awoid speed calculations more than nessesary
				if(lastPos != pos){

					lastPos = pos;
					Vector3 relativePos = target - start;
					speed = relativePos.magnitude;
					if (start != target) transform.rotation = Quaternion.LookRotation(relativePos);
				}


			//r.isVisible is used for all cameras GeometryUtility.TestPlanesAABB(planes,anyCollider.bounds)
			if (r.isVisible){
				//The Pedestrains behind wall should not get Involved
				GetComponent<Animation>().Play ();
				GetComponent<Animation>()["MaleArm|Walking"].speed = getSpeed () / timeStepLength;
			} else {
				//Debug.Log("Anim : Its stop");
				GetComponent<Animation>().Stop ();
			}
				


			

	
			/*
			start = new Vector3 (pos.getX(), 0, pos.getY());
			target = new Vector3 (pos2.getX(), 0, pos2.getY());
			float time = (float) pc.current_time;
			float timeStepLength = Mathf.Clamp((float)pos2.getTime() - (float)pos.getTime(), 0.1f, 50f); // We don't want to divide by zero. OTOH, this results in pedestrians never standing still.
			float movement_percentage = ((float)time - (float)pos.getTime()) / timeStepLength;
			Vector3 newPosition = Vector3.Lerp(start, target, movement_percentage);

			Vector3 relativePos = target - start;
			speed = relativePos.magnitude;

			GetComponent<Animation>()["MaleArm|Walking"].speed = getSpeed () / timeStepLength;
			if (start != target) transform.rotation = Quaternion.LookRotation(relativePos);

			transform.position = newPosition;
			gameObject.hideFlags = HideFlags.None;
			*/

		} else {
			currentTrait = 0;
			active = false;
			r.enabled = false;
			gameObject.hideFlags = HideFlags.HideInHierarchy;
		}




	}

	public float getSpeed() {
		return speed;
	}


	private int _getTrait(List<Vector3> thisList, float thisValue) {	
	//private int _getTrait(SortedList thisList, decimal thisValue) {


		/*while(currentTrait < thisList.Count){
			if ((decimal)thisList.GetKey(currentTrait)>=thisValue) return (currentTrait-1);
			++currentTrait;

		}
		return -1;
		*/
		for (int i =currentTrait; i < thisList.Count; i ++) {
			if (thisList[i].z> thisValue) 
				return (i - 1);
		}
		return -1;


/*
		for (int i = 0; i < thisList.Count; i ++) {
			if ((decimal) thisList.GetKey(i) > thisValue) 
				return (i - 1);
		}
		return -1;
		*/
	}
	
	public void setID(int id) {
		this.id = id;
		this.name = "Pedestrian " + id;
	}

	public int getID(){
		return this.id;
	}

	public void setPositions(SortedList p) {
		if(positions == null) positions = new List<Vector3>();

		foreach (PedestrianPosition ped in p.Values) {
			Vector3 cur = new Vector3();
			cur.x = ped.getX();
			cur.y = ped.getY();
			cur.z = (float) ped.getTime();	//if solution work TODO change hole loading to Vector to avoid pedestrianposition changing to float 
			//cur.w = ped.getID();
			positions.Add(cur);
		}
		/*foreach (PedestrianPosition ped in p.Values) {
			positions.Add(ped);
		}
		*/
		//ScriptablePositions pos = GetComponent<ScriptablePositions>();
		/*
		ScriptablePositions pos = ScriptableObject.CreateInstance<ScriptablePositions>();
		pos.positions = positions;	
		EditorUtility.SetDirty(pos);
		AssetDatabase.DeleteAsset(@"Assets/Resources/savePositions/" +name+".asset");
		AssetDatabase.CreateAsset(pos,@"Assets/Resources/savePositions/" +name +".asset");
		*/


		/*
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.dataPath+ "/Resources/savePositions/" +"Pedestrian_" +GetInstanceID());	
		bf.Serialize(file,positions);
		file.Close();*/
		

		PedestrianPosition transformToPos = (PedestrianPosition)p.GetByIndex (0);
		transform.position = new Vector3 (transformToPos.getX(), 0, transformToPos.getY());
	}

	public bool isActive(){
		return active;
	}

}

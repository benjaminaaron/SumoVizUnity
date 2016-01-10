using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class PedestrianLoader : MonoBehaviour {

	private List<PedestrianPosition> positions = new List<PedestrianPosition>();
	public List<GameObject> pedestrians = new List<GameObject>();
	public int[] population;
	//private Object ped;

	private Object ped1;
	private Object ped2;
	private PlaybackControlNonGUI pc;
	GameObject Pedestrians;

	System.Random rnd = new System.Random();




	void Awake(){
		//ped = Resources.Load ("Hans");


		//diff pedestrian prefabs

		ped1 =Resources.Load("Hans");
		ped2 = Resources.Load("Grete");
		
		pc = GameObject.Find("PlaybackControl").GetComponent<PlaybackControlNonGUI>();
		Pedestrians = new GameObject("Pedestrians");
	}

	// Use this for initialization
	void Start () {
	
	}

	public void addPedestrianPosition(PedestrianPosition p) {
		positions.Add (p);

		if (p.getTime ()>pc.total_time) pc.total_time = p.getTime ();
	}

	public void createPedestrians() {

		positions = positions.OrderBy(x => x.getID()).ThenBy(y => y.getTime()).ToList<PedestrianPosition>();
		SortedList currentList = new SortedList ();
		population = new int[(int)pc.total_time+1];

		for (int i = 0; i< positions.Count;i++) {
			if (positions.Count() > (i+1) && positions[i].getX() == positions[i+1].getX() && positions[i].getY() == positions[i+1].getY()) {
				// Only take into account time steps with changed coordinates. We want smooth animation.
				continue;
			}
			currentList.Add(positions[i].getTime(), positions[i]);
			population[(int) positions[i].getTime ()]++;
			if ((i == (positions.Count-1) || positions[i].getID()!=positions[i+1].getID()) && currentList.Count>0) {

				//GameObject p = (GameObject) Instantiate(ped);


				GameObject p = null;

				//diff pedestrian prefabs
				int gender = rnd.Next(0,1);
				Debug.Log(gender);
				if(gender == 0){
					p = (GameObject) Instantiate(ped1);
				}/*else{
					p = (GameObject) Instantiate(ped2);
				}*/

				if(p == null){

					throw new UnityException("not initialized pedestrian");
				}




				p.GetComponent<Pedestrian>().setGender(gender);
				//p.transform.parent = null;
				p.GetComponent<Pedestrian>().setPositions(currentList);
				p.GetComponent<Pedestrian>().setID(positions[i].getID());
				pedestrians.Add(p);
				currentList.Clear();
				p.transform.SetParent(Pedestrians.transform);
			}
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}

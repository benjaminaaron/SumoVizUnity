using UnityEngine;
using System.Collections;

public class FileChooser : MonoBehaviour {

	//FileBrowser fb = new FileBrowser();
	//FileLoaderXML fl = new FileLoaderXML();
	FileLoaderJSON flj = new FileLoaderJSON();

	void Start () {
		//fb.searchPattern = "*.xml";


		string assetsPath = Application.dataPath;

		flj.loadScenarioFile (assetsPath + "/Data/vadere_output/New_scenario.scenario");


		//fl.loadXMLFile(assetsPath + "/Data/out_flughafen-modell-gruppen.xml");
	}

/*
	void OnGUI(){	

		if (fb.draw()) {
			if (fb.outputFile == null){
				Debug.Log("Cancel hit");
				Application.Quit();
			} else {
				Debug.Log("Ouput File = \""+fb.outputFile.ToString()+"\""); //the outputFile variable is of type FileInfo from the .NET library "http://msdn.microsoft.com/en-us/library/system.io.fileinfo.aspx"

				// Load file
				fl.loadXMLFile(fb.outputFile.FullName);

				// Enable Flycam look-around
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;

				// Start playing
				// ???

				// Remove file choser dialogue
				Destroy (this);
			}
		}
	}
*/
	
	void Update () {}
}

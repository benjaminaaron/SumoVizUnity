using UnityEngine;
using System.Collections;

public class FileChooser : MonoBehaviour {

	FileBrowser fb = new FileBrowser();
	FileLoaderXML fl = new FileLoaderXML();
	FileLoaderJSON flj = new FileLoaderJSON();

	void Start () {
		//fb.searchPattern = "*.xml"; //TODO how to have both *.xml and *.scenario? couldn't figure it out
	}


	void OnGUI(){	

		if (fb.draw()) {
			if (fb.outputFile == null){
				Debug.Log("Cancel hit");
				Application.Quit();
			} else {
				Debug.Log("Ouput File = \"" + fb.outputFile.ToString() + "\""); //the outputFile variable is of type FileInfo from the .NET library "http://msdn.microsoft.com/en-us/library/system.io.fileinfo.aspx"

				// Load file
				switch (fb.outputFile.Extension) {
					case ".xml":
						fl.loadXMLFile(fb.outputFile.FullName);
						break;
					case ".scenario":
						flj.loadScenarioFile (fb.outputFile.FullName);
						break;
					default:
						Debug.Log("can't open this file");
						break;
				}

				// Enable Flycam look-around
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;

				// Remove file choser dialogue
				Destroy (this);
			}
		}
	}

	
	void Update () {}
}


public interface FileLoader {

	void loadFileByPath (string path);
	void buildGeometry();
	void loadTrajectories (string filename);
	string getIdentifier();
	string getInputfileExtension();

}

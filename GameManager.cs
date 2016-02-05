// Tom Wexler
// Example program to help you get started with your project.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	GameObject tileFolder;	// This will be an empty game object used for organizing objects in the Hierarchy pane.
	List<Tile> tiles;			// This list will hold the gem objects that are created.
	int tiletype; 			// The next gem type to be created.
	List<Marble> marbles;
	GameObject marbleFolder;
	GameObject boardObject;

	public Board boardmanager;

	public float turnProbability = .8f;

	public bool go = false;
	
	// Start is called once when the script is created.
	void Start () {
		createTileFolder ();
		createMarbleFolder ();
		createBoard ();
	}

	void createTileFolder(){
		tileFolder = new GameObject();  
		tileFolder.name = "Tiles";		// The name of a game object is visible in the hierarchy pane.
		tiles = new List<Tile>();
		tiletype = 1;
	}

	void createMarbleFolder(){
		marbleFolder = new GameObject ();
		marbleFolder.name = "Marbles";
		marbles = new List<Marble>();
	}

	void createBoard(){
		boardObject = new GameObject ();
		boardmanager = boardObject.AddComponent<Board> () as Board;
		boardmanager.createBoard ();
	}

	// Update is called every frame.
	void Update () {
		
	}

	void makeEmptyTile(float x, float y) {
		GameObject tileObject = new GameObject();			// Create a new empty game object that will hold a gem.
		Tile tile = tileObject.AddComponent<Tile>();			// Add the Gem.cs script to the object.
															// We can now refer to the object via this script.
		tile.transform.parent = tileFolder.transform;			// Set the gem's parent object to be the gem folder.
		tile.transform.position = new Vector3(x,y,0);		// Position the gem at x,y.								
		
		tile.init(x,y,1, this);					// Initialize the gem script.
		
		tiles.Add(tile);										// Add the gem to the Gems list for future access.
		tile.name = "Tile "+tiles.Count;						// Give the gem object a name in the Hierarchy pane.							
	}

	void makeTurnTile(float x, float y) {
		GameObject tileObject = new GameObject();			// Create a new empty game object that will hold a gem.
		Tile tile = tileObject.AddComponent<Tile>();			// Add the Gem.cs script to the object.
		// We can now refer to the object via this script.
		tile.transform.parent = tileFolder.transform;			// Set the gem's parent object to be the gem folder.
		tile.transform.position = new Vector3(x,y,0);		// Position the gem at x,y.								

		tile.init(x,y,2, this);					// Initialize the gem script.

		tiles.Add(tile);										// Add the gem to the Gems list for future access.
		tile.name = "Tile "+tiles.Count;						// Give the gem object a name in the Hierarchy pane.							
	}

	void makeMarble(float x, float y){
		GameObject marbleObject = new GameObject();			// Create a new empty game object that will hold a gem.
		Marble marble = marbleObject.AddComponent<Marble>();			// Add the Gem.cs script to the object.
		// We can now refer to the object via this script.
		marble.transform.parent = marbleFolder.transform;			// Set the gem's parent object to be the gem folder.
		marble.transform.position = new Vector3(x,y,0);		// Position the gem at x,y.								

		marble.init(x,y, this);					// Initialize the gem script.

		marbles.Add(marble);										// Add the gem to the Gems list for future access.
		marble.name = "Marble "+tiles.Count;						// Give the gem object a name in the Hierarchy pane.
	}

	private void beginLevel(){
		createMarbles ();
	}

	void createMarbles(){
		foreach (Square s in boardmanager.marblestart) {
			makeMarble (s.x, s.y);
		}
	}

	// This function defines the buttons, and dictates what happens when they're pressed.
	void OnGUI () {
		if (!go && GUI.Button (new Rect (300, 165, 150, 60), "START")) {
			go = true;
			beginLevel();
		}
			// Printing goes to the Console pane.  
			// If an object doesn't extend monobehavior, calling print won't do anything.  
			// Make sure "Collapse" isn't selected in the Console pane if you want to see duplicate messages.
	}

}
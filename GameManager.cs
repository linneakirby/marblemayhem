// Linnea Kirby
// Controls the game (i.e. the marbles)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public Vector2 dirN { get { return new Vector2(0, 1); } }
	public Vector2 dirS { get { return new Vector2(0, -1); } }
	public Vector2 dirE { get { return new Vector2(1, 0); } }
	public Vector2 dirW { get { return new Vector2(-1, 0); } }

	List<Marble> marbles;
	GameObject marbleFolder;
	GameObject boardObject;

	public Board boardmanager;

	public float turnProbability = .8f;

	public bool go = false;

	public int numMarbles = 0;

	void Start () {
		createBoard ();
	}

	void createBoard(){
		boardObject = new GameObject ();
		boardObject.name = "Board";
		boardmanager = boardObject.AddComponent<Board> () as Board;
		boardmanager.initBoard (this);
	}

	// Start button that disappears once clicked (and triggers the start of the game)
	void OnGUI () {
		int xpos = ((Screen.width)-(150))/2;
		int ypos = ((Screen.height)-(60))/2;
		if (!go && GUI.Button (new Rect (xpos, ypos, 150, 60), "START")) {
			go = true;
			beginLevel();
		}
	}
		
	void Update () {
		
	}

	void makeMarble(int direction, Tile t){
		GameObject marbleObject = new GameObject();	
		marbleObject.layer = 8;
		Marble marble = marbleObject.AddComponent<Marble>();

		marble.transform.parent = marbleFolder.transform;			
		marble.transform.position = new Vector3(t.x,t.y,-1);

		marble.init(direction,t, this);					

		marbles.Add(marble);										
		marble.name = "Marble "+marbles.Count;						
	}

	private void beginLevel(){
		createMarbles ();
	}

	private void createMarbleFolder(){
		marbleFolder = new GameObject ();
		marbleFolder.name = "Marbles";
		marbles = new List<Marble>();
	}

	private void createMarbles(){
		createMarbleFolder ();
		while (numMarbles < 1) {
			int i = (int)(Random.value * 100) % 10;
			int j = (int)(Random.value * 100) % 18;

			int direction = (int)(Random.value * 100) % 4;

			Tile t = boardmanager.get (i, j);
			makeMarble (direction, t);
			numMarbles++;
		}
	}



}
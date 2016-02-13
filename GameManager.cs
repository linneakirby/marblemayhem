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

	GameObject gemFolder;
	List<Gem> gems;	
	int gemType;

	public Board boardmanager;

	public float turnProbability = .8f;
	public float gemProbability = .5f;
	public float colorProbability = .25f;

	public float timeLeft;
	private string gameOver = "Game Over";
	private string timeLeftS;
	public bool go = false;
	private bool done = false;
	public bool pause = false;

	public int numMarbles = 0;
	private string totalScore = "Score: 0";

	void Start () {
		createBoard ();
		createMarbles ();
		createGemFolder ();
	}

	void createBoard(){
		boardObject = new GameObject ();
		boardObject.name = "Board";
		boardmanager = boardObject.AddComponent<Board> () as Board;
		boardmanager.initBoard (this);
	}

	// Start button that disappears once clicked (and triggers the start of the game)
	void OnGUI () {
		GUIStyle guiStyle = new GUIStyle();
		int xpos;
		int ypos;
		if (!go || pause) {
			guiStyle.fontSize = 80;
			guiStyle.normal.textColor = Color.cyan;
			guiStyle.alignment = TextAnchor.MiddleCenter;
			guiStyle.border = GUI.skin.customStyles[0].border;
			GUI.color = Color.cyan;
			xpos = ((Screen.width)-(300))/2;
			ypos = ((Screen.height)-(50))/2-(Screen.height/3);
			GUI.Label (new Rect (xpos, ypos, 300, 50), "MARBLE MAYHEM", guiStyle);

			guiStyle.fontSize = 20;
			xpos = ((Screen.width)-(400))/2;
			ypos = ((Screen.height)-(100))/2-((Screen.height/9));
			GUI.Label (new Rect(xpos, ypos, 400, 120),"INSTRUCTIONS\n" +
				"Right- and left-click on turns to change their orientation\n" +
				"Press space to pause\n"+
				"Collect as many gems as possible before time runs out!\n",guiStyle);
		}
		xpos = ((Screen.width)-(150))/2;
		ypos = ((Screen.height)-(60))/2+(Screen.height/7);
		if (!done && pause) {
			guiStyle.fontSize = 60;
			guiStyle.normal.textColor = Color.green;
			GUI.Label (new Rect (xpos, ypos, 150, 60), "PAUSED", guiStyle);
		}

		if (!go && !done && !pause && GUI.Button (new Rect (xpos, ypos, 150, 60), "START")) {
			go = true;
			beginLevel ();
		} else if(go){
			GUI.color = Color.yellow;
			if (timeLeft < 0) {
				done = true;
				go = false;
				GUI.color = Color.red;
				GUI.Label (new Rect (xpos, ypos, 150, 30), "Final " + totalScore, "box");
				GUI.Label (new Rect (xpos, ypos - 35, 150, 30), gameOver, "box");

			} else {
				xpos = ((Screen.width) - 150);
				ypos = ((Screen.height) - 30);
				GUI.Label (new Rect (xpos, ypos, 150, 30), totalScore, "box");
				GUI.Label (new Rect (0, ypos, 150, 30), timeLeftS, "box");
			}
		}
	}

	void beginLevel(){
		timeLeft = 60f;
	}

	void checkTime(){
		if(timeLeft <= 0){
			go = false;
			done = true;
		}
	}

	private string getScore(){
		int partialscore = 0;
		foreach (Marble m in marbles) {
			partialscore += m.score;
		}
		return partialscore.ToString ();
	}
		
	void Update () {
		if (Input.GetKeyDown ("space")){
			if (!pause && go) {
				pause = true;
			} else {
				pause = false;
			}

		}
		if (!done && go && !pause) {
			timeLeft -= Time.deltaTime;
			timeLeftS = "Time Left: " +timeLeft.ToString ("N2");
			makeGem ();
			totalScore = "Score: " + getScore();
			checkTime ();
		}
	}

	void makeGem(){
		float probability = gemProbability * Time.deltaTime;
		int color = (int)(colorProbability * Random.value*100)%4;
		int index;
		if (probability > Random.value) {
			int i = (int)(Random.value * 100) % 10;
			int j = (int)(Random.value * 100) % 18;
			Tile t = boardmanager.get (i, j);

			GameObject gemObject = new GameObject();	
			gemObject.layer = 8;
			Gem gem = gemObject.AddComponent<Gem>();

			gem.transform.parent = gemFolder.transform;			
			gem.transform.position = new Vector3(t.x,t.y,-1);

			gem.init(color, this);					

			gems.Add(gem);										
			gem.name = "Gem "+gems.Count;
		}
	}

	private void createGemFolder(){
		gemFolder = new GameObject ();
		gemFolder.name = "Gems";
		gems = new List<Gem>();
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

	private void createMarbleFolder(){
		marbleFolder = new GameObject ();
		marbleFolder.name = "Marbles";
		marbles = new List<Marble>();
	}

	private void createMarbles(){
		createMarbleFolder ();
		while (numMarbles < 5) {
			int i = (int)(Random.value * 100) % 10;
			int j = (int)(Random.value * 100) % 18;

			int direction = (int)(Random.value * 100) % 4;

			Tile t = boardmanager.get (i, j);

			//make sure marbles don't start on turns
			while (t.isTurn ()) {
				i = (int)(Random.value * 100) % 10;
				j = (int)(Random.value * 100) % 18;
				t = boardmanager.get (i, j);
			}
			makeMarble (direction, t);
			numMarbles++;
		}
	}



}
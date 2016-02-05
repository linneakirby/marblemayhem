using UnityEngine;
using System.Collections;
using System.IO;

public class Square : MonoBehaviour{
	public float x;
	public float y;

	public int i;
	public int j;

	public ArrayList neighbors = new ArrayList();

	public Tile tile;

	public void init (float x, float y, int i, int j){
		this.x = x;
		this.y = y;
		this.i = i;
		this.j = j;
	}

	public ArrayList getNeighbors(){
		return neighbors;
	}

	public string toString(){
		if (tile.isTurn()) {
			return "turn";
		}
		return "empty";
	}

	public void addTile(Tile tile){
		this.tile = tile;
	}

	public Tile getTile(){
		return this.tile;
	}
}

public class Board : MonoBehaviour {
	public ArrayList validsquares = new ArrayList();
	public ArrayList turns = new ArrayList();
	public ArrayList marblestart = new ArrayList();
	public char[,] charboard;

	public Square[,] squareboard;

	private Square createSquare (float x, float y, int i, int j){
		GameObject squareObject = new GameObject ();
		Square square = squareObject.AddComponent<Square> () as Square;
		square.init (x, y, i, j);
		return square;
	}

	private void findNeighbors(){
		foreach (Square square in squareboard) {
			for (int k = square.i - 1; k <= square.i + 1; k++) {
				//for (int l = square.j - 1; l <= square.j + 1; l++) {
				if (k >= 0 && k < 10 && k != square.i) {
					square.neighbors.Add (squareboard [k, square.j]);
				}
			}
			for (int k = square.j - 1; k <= square.j + 1; k++) {
				if (k >= 0 && k < 18 && k != square.j) {
					square.neighbors.Add (squareboard [square.i, k]);
				}
			}
		}
	}

	public void createBoard(){
		charboard = new char[10,18];
		squareboard = new Square[10, 18];
		float x = -9.15f;
		float y = 5.5f;
		Square square;

		for(int i=0; i<10; i++){
			for(int j=0; j<18; j++){
				square = createSquare ((float)(x + (j * 1.08)), (float)(y - i), i, j);
				squareboard [i, j] = square;
				if(square.toString().Equals("\n")){
				}
				else{
					placeSquare (square);
				}
			}
		}
		findNeighbors ();
	}

	private void placeSquare(Square square){
		if (square.toString ().Equals ("W")) {
			addSquare ("Prefabs/Wall", false, false, square);
		} else if (square.toString ().Equals ("P")) {
			addSquare ("Prefabs/Track", true, false, square);
		} else if (square.toString ().Equals ("A")) {
			addSquare ("Prefabs/TurnSE", true, true, square);
		} else if (square.toString ().Equals ("B")) {
			addSquare ("Prefabs/TurnSW", true, true, square);
		} else if (square.toString ().Equals ("C")) {
			addSquare ("Prefabs/TurnNE", true, true, square);
		} else if (square.toString ().Equals ("D")) {
			addSquare ("Prefabs/TurnNW", true, true, square);
		} else if (square.toString ().Equals ("S")) {
			addSquare ("Prefabs/Switch", false, false, square);
		} else if (square.toString ().Equals ("T")) {
			addSquare ("Prefabs/Track", true, false, square);
			marblestart.Add (square);
		}
	}

	private void addSquare(string location, bool valid, bool isTurn, Square square){
		GameObject tile = Instantiate (Resources.Load (location), new Vector2 (square.x, square.y), Quaternion.identity) as GameObject;
		if (valid) {
			validsquares.Add (square);
		}
		if (isTurn) {
			addTurn (tile, square);
		}
		if (square.toString().Equals("S")) {
			addSwitch (tile, square);
		}
		square.addTile(tile);
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Square : MonoBehaviour{
	public float x;
	public float y;

	public int i;
	public int j;

	public ArrayList neighbors = new ArrayList();

	public Tile tile;
	public bool hasTile = false;

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
	GameObject tileFolder;
	public List<Tile> tiles;

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

	//creates internal representation of the board
	public void initBoard(List<Tile> tiles, GameObject tileFolder){
		charboard = new char[10,18];
		squareboard = new Square[10, 18];
		float x = -9.15f;
		float y = 5.5f;
		Square square;
		this.tiles = tiles;
		this.tileFolder = tileFolder;

		for(int i=0; i<10; i++){
			for(int j=0; j<18; j++){
				square = createSquare ((float)(x + (j * 1.08)), (float)(y - i), i, j);
				squareboard [i, j] = square;
			}
		}
		findNeighbors ();
		placeSquares ();
	}

	//creates external representation of the board
	protected int totalturns = 30;
	private void placeSquares(){
		placeRowTurns ();
		placeColTurns ();
		placeRemainingTurns ();
		placeEmptyTiles ();
	}

	//TODO
	//place at least one turn in every row
	private void placeRowTurns(){
		for (int i = 0; i < 10; i++) { 
			totalturns--;
		}
	}

	//TODO
	//place at least one turn in every column
	private void placeColTurns(){
		for (int j = 0; j < 18; j++) { 
			totalturns--;
		}
	}

	//TODO
	private void placeRemainingTurns(){
		while (totalturns > 0) {
			totalturns--;
		}
	}

	private void placeEmptyTiles(){
		foreach(Square s in squareboard){
			if (!(s.hasTile)) {
				makeEmptyTile (s.x, s.y);
				s.hasTile = true;
			}
		}
	}

	private void makeEmptyTile(float x, float y) {
		GameObject tileObject = new GameObject();			// Create a new empty game object that will hold a gem.
		Tile tile = tileObject.AddComponent<Tile>();			// Add the Gem.cs script to the object.
		// We can now refer to the object via this script.
		tile.transform.parent = tileFolder.transform;			// Set the gem's parent object to be the gem folder.
		tile.transform.position = new Vector3(x,y,0);		// Position the gem at x,y.								

		tile.init(x,y,1, this);					// Initialize the gem script.

		tiles.Add(tile);										// Add the gem to the Gems list for future access.
		tile.name = "Tile "+tiles.Count;						// Give the gem object a name in the Hierarchy pane.							
	}

	private void makeTurnTile(float x, float y) {
		GameObject tileObject = new GameObject();			// Create a new empty game object that will hold a gem.
		Tile tile = tileObject.AddComponent<Tile>();			// Add the Gem.cs script to the object.
		// We can now refer to the object via this script.
		tile.transform.parent = tileFolder.transform;			// Set the gem's parent object to be the gem folder.
		tile.transform.position = new Vector3(x,y,0);		// Position the gem at x,y.								

		tile.init(x,y,2, this);					// Initialize the gem script.

		tiles.Add(tile);										// Add the gem to the Gems list for future access.
		tile.name = "Tile "+tiles.Count;						// Give the gem object a name in the Hierarchy pane.							
	}
}
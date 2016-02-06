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
	int tiletype;
	GameObject squareFolder;
	public List<Square> squares;

	public GameManager gm;

	public Square[,] squareboard;



	private Square createSquare (float x, float y, int i, int j){
		GameObject squareObject = new GameObject ();
		Square square = squareObject.AddComponent<Square> () as Square;
		square.transform.parent = squareFolder.transform;

		square.init (x, y, i, j);

		square.transform.position = new Vector3(square.x,square.y,0);
		squares.Add(square);										
		square.name = "Square "+squares.Count;
		return square;
	}

	//TODO - need to make the board toroidal
	private void findNeighbors(){
		foreach (Square square in squareboard) {
			for (int k = square.i - 1; k <= square.i + 1; k++) {
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
	public void initBoard(GameManager gm){
		this.gm = gm;
		createTileFolder ();
		createSquareFolder ();
		charboard = new char[10,18];
		squareboard = new Square[10, 18];
		float x = -9.05f;
		float y = 4.5f;
		Square square;

		for(int i=0; i<10; i++){
			for(int j=0; j<18; j++){
				square = createSquare ((float)(x + (j * 1.065)), (float)(y - i), i, j);
				squareboard [i, j] = square;
			}
		}
		findNeighbors ();
		placeSquares ();
	}

	void createSquareFolder(){
		squareFolder = new GameObject ();
		squareFolder.name = "Squares";
		squares = new List<Square>();
	}

	void createTileFolder(){
		tileFolder = new GameObject();  
		tileFolder.name = "Tiles";		
		tiles = new List<Tile>();
		tiletype = 1;
	}

	//creates external representation of the board
	protected int totalturns = 30;
	private void placeSquares(){
		placeRowTurns ();
		placeColTurns ();
		placeRemainingTurns ();
		placeEmptyTiles ();
	}
		
	//place at least one turn in every row
	private void placeRowTurns(){
		for (int i = 0; i < 10; i++) { 
			int j = (int)(Random.value * 100) % 18;
			makeTurnTile (squareboard [i, j]);
			totalturns--;
		}
	}
		
	//place at least one turn in every column
	private void placeColTurns(){
		for (int j = 0; j < 18; j++) { 
			int i = (int)(Random.value * 100) % 10;
			makeTurnTile (squareboard [i, j]);
			totalturns--;
		}
	}
		
	private void placeRemainingTurns(){
		while (totalturns > 0) {
			int i = (int)(Random.value * 100) % 10;
			int j = (int)(Random.value * 100) % 18;
			makeTurnTile (squareboard [i, j]);
			totalturns--;
		}
	}

	private void placeEmptyTiles(){
		foreach(Square s in squareboard){
			if (!(s.hasTile)) {
				makeEmptyTile (s);
			}
		}
	}

	private void updateSquare(Square s, Tile t){
		s.tile = t;
		s.hasTile = true;
	}

	private Tile initTile(Square s){
		GameObject tileObject = new GameObject();			
		Tile tile = tileObject.AddComponent<Tile>();			

		tile.transform.parent = tileFolder.transform;			
		tile.transform.position = new Vector3(s.x,s.y,0);

		tiles.Add(tile);										
		tile.name = "Tile "+tiles.Count;

		return tile;
	}

	private void makeEmptyTile(Square s) {
		Tile tile = initTile (s);						
		tile.init(1, s, gm);					
		updateSquare (s, tile);													
	}

	private void makeTurnTile(Square s) {
		Tile tile = initTile (s);									
		tile.init(2, s, gm);	
		updateSquare (s, tile);													
	}

	public Square get(int i, int j){
		return squareboard [i, j];
	}
}
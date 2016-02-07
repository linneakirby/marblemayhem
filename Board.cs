using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Board : MonoBehaviour {
	public ArrayList turns = new ArrayList();
	public ArrayList marblestart = new ArrayList();
	GameObject tileFolder;
	public List<Tile> tiles;
	int tiletype;
	GameObject squareFolder;

	public GameManager gm;

	public Tile[,] board;

	private int mod(int a, int b){
		return (a % b + b) % b;
	}
		
	private void findNeighbors(){
		foreach (Tile tile in board) {
			//find N neighbor
			tile.neighbors.Add (board [mod((tile.i - 1),10), tile.j]);
			//find S neighbor
			tile.neighbors.Add (board [mod((tile.i + 1),10), tile.j]);
			//find W neighbor
			tile.neighbors.Add (board [tile.i, mod((tile.j - 1),18)]);
			//find E neighbor
			tile.neighbors.Add (board [tile.i, mod((tile.j + 1),18)]);
		}
	}

	//creates internal representation of the board
	public void initBoard(GameManager gm){
		this.gm = gm;
		createTileFolder ();
		board = new Tile[10, 18];
		float x = -9.05f;
		float y = 4.5f;
		Tile tile;

		for(int i=0; i<10; i++){
			for(int j=0; j<18; j++){
				tile = createTile ((float)(x + (j * 1.065)), (float)(y - i), i, j);
				board [i, j] = tile;
			}
		}
		findNeighbors ();
		placeTiles ();
	}

	void createTileFolder(){
		tileFolder = new GameObject();  
		tileFolder.name = "Tiles";		
		tiles = new List<Tile>();
		tiletype = 1;
	}

	private Tile createTile (float x, float y, int i, int j){
		GameObject tileObject = new GameObject ();
		Tile tile = tileObject.AddComponent<Tile> ();
		tile.transform.parent = tileFolder.transform;

		tile.init (i, j, x, y, gm);

		tile.transform.position = new Vector3(tile.x,tile.y,0);
		tiles.Add(tile);										
		tile.name = "Tile "+tile.j+","+tile.i;
		return tile;
	}

	//creates external representation of the board
	protected int totalturns = 30;
	private void placeTiles(){
		placeRowTurns ();
		placeColTurns ();
		placeRemainingTurns ();
		placeEmptyTiles ();
	}
		
	//place at least one turn in every row
	private void placeRowTurns(){
		for (int i = 0; i < 10; i++) { 
			int j = (int)(Random.value * 100) % 18;
			if (!(board [i, j].isTurn ())) {
				makeTurnTile (board [i, j]);
				totalturns--;
			}
		}
	}
		
	//place at least one turn in every column
	private void placeColTurns(){
		for (int j = 0; j < 18; j++) { 
			int i = (int)(Random.value * 100) % 10;
			if (!(board [i, j].isTurn ())) {
				makeTurnTile (board [i, j]);
				totalturns--;
			}
		}
	}
		
	private void placeRemainingTurns(){
		while (totalturns > 0) {
			int i = (int)(Random.value * 100) % 10;
			int j = (int)(Random.value * 100) % 18;
			if (!(board [i, j].isTurn ())) {
				makeTurnTile (board [i, j]);
				totalturns--;
			}
		}
	}

	private void placeEmptyTiles(){
		foreach (Tile t in board) {
			t.addEmptyTexture ();
		}
	}

	private void makeTurnTile(Tile tile) {								
		tile.addTurn();													
	}

	public Tile get(int i, int j){
		return board [i, j];
	}
}
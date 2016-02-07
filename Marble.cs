using UnityEngine;
using System.Collections;

public class Marble : MonoBehaviour {
	public Vector2 N { get { return new Vector2(0, 1); } }
	public Vector2 S { get { return new Vector2(0, -1); } }
	public Vector2 E { get { return new Vector2(1, 0); } }
	public Vector2 W { get { return new Vector2(-1, 0); } }

	private MarbleModel model;		
	public float x;
	public float y;
	public float speed = .5f;
	public Vector2 direction;
	public GameManager gm;		

	public Tile currTile;
	public Tile nextTile;

	public Vector3 currpos;
	public Vector3 destpos;
	public float destdist;

	public void init(int direction, Tile tile, GameManager gm) {
		this.x = tile.x;
		this.y = tile.y;
		this.gm = gm;

		currTile = tile;
		currTile.marbles.Add (this);
		getDirection (direction);
		getNextSquare ();

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);	
		modelObject.layer = 8;
		model = modelObject.AddComponent<MarbleModel>();	
		model.init(x, y, this);	
	}

	void Update(){
		if (gm.go) {
			updateCoordinates ();
			updateDistances ();
			move ();
		}
	}

	private void updateLocation(){
		checkTurn ();
		getNextSquare ();
	}

	private void move(){
		if (this.x > 9.55) {
			this.gameObject.transform.position = new Vector3 ((nextTile.x - .5f), nextTile.y, -1f);
			updateCurrSquare ();
			this.x = currTile.x-.5f;
			this.y = currTile.y;
			updateLocation ();
		} else if (this.x < -9.55) {
			this.gameObject.transform.position = new Vector3 ((nextTile.x + .5f), nextTile.y, -1f);
			updateCurrSquare ();
			this.x = currTile.x+.5f;
			this.y = currTile.y;
			updateLocation ();
		} else if (this.y > 5f) {
			this.gameObject.transform.position = new Vector3 (nextTile.x, (nextTile.y - .5f), -1f);
			updateCurrSquare ();
			this.x = currTile.x;
			this.y = currTile.y-.5f;
			updateLocation ();
		} else if (this.y < -5f) {
			this.gameObject.transform.position = new Vector3 (nextTile.x, (nextTile.y + .5f), -1f);
			updateCurrSquare ();
			this.x = currTile.x;
			this.y = currTile.y+.5f;
			updateLocation ();
		}
		else if ((direction * speed * Time.deltaTime).magnitude < destdist) {
			this.gameObject.transform.Translate (direction * speed * Time.deltaTime);
		}
		else {
			//print ("ELSE");
			this.gameObject.transform.position = new Vector3(nextTile.x, nextTile.y, -1f);
			updateCurrSquare ();
			this.x = currTile.x;
			this.y = currTile.y;
			updateLocation ();
		}
	}

	private void checkTurn(){
		//print ("inside check turn!");
		//print (currTile.isTurn ());
		if (currTile.isTurn ()) {
			direction = currTile.getNewDirection (direction);
		} 
	}

	private void getDirection(int dir){
		if (dir == 0) {
			direction = N;
		} else if (dir == 1) {
			direction = E;
			//model.transform.eulerAngles = new Vector3(0,0,270);
		} else if (dir == 2) {
			direction = S;
			//model.transform.eulerAngles = new Vector3(0,0,180);
		} else {
			direction = W;
			//model.transform.eulerAngles = new Vector3(0,0,90);
		}

	}

	private void updateCoordinates(){
		this.x += direction.x * speed * Time.deltaTime;
		this.y += direction.y * speed * Time.deltaTime;
	}

	private void updateDistances(){
		currpos = new Vector3 (this.x, this.y, -1);
		if (currpos.magnitude > destpos.magnitude) {
			destdist = (currpos-destpos).magnitude;
		} else {
			destdist = (destpos-currpos).magnitude;
		}
	}

	//removes itself from currSquare's list of marbles, updates currSquare, and adds itself to the new currSquare's list of marbles
	private void updateCurrSquare(){
		currTile.marbles.Remove (this);
		currTile = nextTile;
		//this.x = currTile.x;
		//this.y = currTile.y;
		currTile.marbles.Add (this);
	}

	private void getNextSquare(){
		if (direction.Equals (N)) {
			nextTile = (Tile)currTile.getNeighbors() [0];
		} else if (direction.Equals (S)) {
			nextTile = (Tile)currTile.getNeighbors() [1];
		} else if (direction.Equals (E)) {
			nextTile = (Tile)currTile.getNeighbors() [3];
		} else {
			nextTile = (Tile)currTile.getNeighbors() [2];
		}
		destpos = new Vector3 (nextTile.x, nextTile.y, -1);
	}
}

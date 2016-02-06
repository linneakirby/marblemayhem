using UnityEngine;
using System.Collections;

public class Marble : MonoBehaviour {
	public Vector2 N { get { return new Vector2(0, 1); } }
	public Vector2 S { get { return new Vector2(0, -1); } }
	public Vector2 E { get { return new Vector2(1, 0); } }
	public Vector2 W { get { return new Vector2(-1, 0); } }

	private MarbleModel model;		
	private float x;
	private float y;
	public float speed = 1.5f;
	public Vector2 direction;
	private GameManager gm;		

	public Square currSquare;
	public Square nextSquare;

	public Vector3 currpos;
	public Vector3 destpos;
	public float destdist;

	void Update(){
		updateCoordinates ();
		updateDistances ();
		move ();
	}

	public void init(int direction, Square square, GameManager gm) {
		this.x = square.x;
		this.y = square.y;
		getDirection (direction);
		this.gm = gm;

		currSquare = square;
		currSquare.tile.marbles.Add (this);
		checkTurn ();
		getNextSquare ();

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);	
		model = modelObject.AddComponent<MarbleModel>();						
		model.init(x, y, this);						
	}

	private void checkTurn(){
		if (currSquare.tile.isTurn ()) {
			direction = currSquare.tile.getNewDirection (direction);
		} 
	}

	private void getDirection(int dir){
		if (dir == 0) {
			direction = N;
		} else if (dir == 1) {
			direction = E;
			//model.transform.Rotate (0, 0, 270);
		} else if (dir == 2) {
			direction = S;
			//model.transform.Rotate (0, 0, 180);
		} else {
			direction = W;
			//model.transform.Rotate (0, 0, 90);
		}

	}

	private void updateCoordinates(){
		this.x += direction.x * speed * Time.deltaTime;
		this.y += direction.y * speed * Time.deltaTime;
	}

	private void updateDistances(){
		currpos = new Vector3 (this.x, this.y, 0);
		if (currpos.magnitude > destpos.magnitude) {
			destdist = (currpos-destpos).magnitude;
		} else {
			destdist = (destpos-currpos).magnitude;
		}
	}

	private void move(){
		if ((direction * speed * Time.deltaTime).magnitude < destdist) {
			this.gameObject.transform.Translate (direction * speed * Time.deltaTime);
		}
		else {
			this.gameObject.transform.position = new Vector3(nextSquare.x, nextSquare.y, 0);
			updateCurrSquare ();
			getNextSquare ();
		}
	}

	//removes itself from currSquare's list of marbles, updates currSquare, and adds itself to the new currSquare's list of marbles
	private void updateCurrSquare(){
		currSquare.tile.marbles.Remove (this);
		currSquare = nextSquare;
		currSquare.tile.marbles.Add (this);
	}

	private void getNextSquare(){
		if (direction.Equals (N)) {
			nextSquare = (Square)currSquare.getNeighbors() [0];
		} else if (direction.Equals (S)) {
			nextSquare = (Square)currSquare.getNeighbors() [1];
		} else if (direction.Equals (E)) {
			nextSquare = (Square)currSquare.getNeighbors() [3];
		} else {
			nextSquare = (Square)currSquare.getNeighbors() [2];
		}
		destpos = new Vector3 (nextSquare.x, nextSquare.y, 0);
	}
}

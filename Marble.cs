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
	public float speed = 1.5f;
	public float cooldown = 0f;
	public Vector2 direction;
	public Vector2 destDir;
	public GameManager gm;		

	public Tile currTile;
	public Tile nextTile;

	public bool isTurning = false;
	public float clock = 0f;
	public float turnStartTime = 0f;
	public float turnClock = 0f;
	public float p;

	public int score;
	public int health;

	public Vector3 currpos;
	public Vector3 destpos;
	public float destdist;

	public void init(int direction, Tile tile, GameManager gm) {
		this.x = tile.x;
		this.y = tile.y;
		this.gm = gm;

		health = 5;
		score = 0;

		currTile = tile;
		currTile.marbles.Add (this);

		var modelObject = GameObject.CreatePrimitive(PrimitiveType.Quad);	
		modelObject.layer = 8;
		modelObject.tag = "marble";
		MeshCollider mc = modelObject.GetComponent<MeshCollider> ();
		mc.enabled = false;

		SphereCollider sc = modelObject.AddComponent<SphereCollider> ();
		sc.radius = 0.3445716f;
		//sc.isTrigger = true;
		Rigidbody rb = modelObject.AddComponent<Rigidbody> ();
		rb.useGravity = false;
		rb.isKinematic = false;
		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

		model = modelObject.AddComponent<MarbleModel>();	
		model.init(x, y, this, modelObject);	

		getDirection (direction);
		getNextSquare ();
	}

	void Update(){
		if (gm.go && !gm.pause) {
			clock += Time.deltaTime;
			turnClock += Time.deltaTime;
			updateCoordinates ();
			updateDistances ();
			p = (turnClock*speed*2);
			if (isTurning) {
				turn ();
			} else {
				move ();
			}
		}
	}

	private void updateLocation(){
		checkTurn ();
		if (!(currTile.isTurn ())) {
			getNextSquare ();
		}
	}

	private void finishTurn(){
		if (direction.Equals (N)) {
			goN ();
		} else if (direction.Equals (S)) {
			goS ();
		} else if (direction.Equals (E)) {
			goE ();
		} else {
			goW ();
		}
	}

	private void goN(){
		this.gameObject.transform.position = new Vector3 (nextTile.x, (nextTile.y - .5f), -1f);
		updateCurrSquare ();
		this.x = currTile.x;
		this.y = currTile.y-.5f;
		updateLocation ();
	}

	private void goS(){
		this.gameObject.transform.position = new Vector3 (nextTile.x, (nextTile.y + .5f), -1f);
		updateCurrSquare ();
		this.x = currTile.x;
		this.y = currTile.y+.5f;
		updateLocation ();
	}

	private void goE(){
		this.gameObject.transform.position = new Vector3 ((nextTile.x-.5f), nextTile.y, -1f);
		updateCurrSquare ();
		this.x = currTile.x-.5f;
		this.y = currTile.y;
		updateLocation ();
	}

	private void goW(){
		this.gameObject.transform.position = new Vector3 ((nextTile.x + .5f), nextTile.y, -1f);
		updateCurrSquare ();
		this.x = currTile.x+.5f;
		this.y = currTile.y;
		updateLocation ();
	}

	private void move(){
		if (this.x > 9.55) {
			goE ();
		} else if (this.x < -9.55) {
			goW ();
		} else if (this.y > 5f) {
			goN ();
		} else if (this.y < -5f) {
			goS ();
		}
		else if ((direction * speed * Time.deltaTime).magnitude < destdist) {
			this.gameObject.transform.Translate (direction * speed * Time.deltaTime);
		}
		else {
			if (direction.Equals (N)) {
				goN ();
			} else if (direction.Equals (S)) {
				goS ();
			} else if (direction.Equals (E)) {
				goE ();
			} else {
				goW ();
			}
		}
	}

	private void checkTurn(){
		if (currTile.isTurn ()) {
			destDir = currTile.getNewDirection (direction);
			getNextTileTurning ();
			if (!(direction.Equals(destDir))) {
				isTurning = true;
				turnStartTime = clock;
				turnClock = 0f;
				//turn ();
			}
		} 
	}

	public void turnHelper(){
		if (currTile.S && currTile.E) {
			//going N to E
			if (direction.Equals (N)) {
				x = currTile.x+.5f+Mathf.Cos(Mathf.PI-(p*Mathf.PI/2))/2;
				y = currTile.y-.5f+Mathf.Sin(Mathf.PI-(p*Mathf.PI/2))/2;
				this.gameObject.transform.position = new Vector3 (x, y, 0);
			}
			//going W to S
			else if (direction.Equals (W)) {
				x = currTile.x+.5f+Mathf.Cos(Mathf.PI/2+(p*Mathf.PI/2))/2;
				y = currTile.y-.5f+Mathf.Sin(Mathf.PI/2+(p*Mathf.PI/2))/2;
				this.gameObject.transform.position = new Vector3 (x, y, 0);
			}
		} else if (currTile.N && currTile.E) {
			//going S to E
			if (direction.Equals (S)) {
				x = currTile.x+.5f+Mathf.Cos(Mathf.PI+(p*Mathf.PI/2))/2;
				y = currTile.y+.5f+Mathf.Sin(Mathf.PI+(p*Mathf.PI/2))/2;
				this.gameObject.transform.position = new Vector3 (x, y, 0);
			}
			//going W to N
			else if (direction.Equals (W)) {
				x = currTile.x+.5f+Mathf.Cos(3*Mathf.PI/2-(p*Mathf.PI/2))/2;
				y = currTile.y+.5f+Mathf.Sin(3*Mathf.PI/2-(p*Mathf.PI/2))/2;
				this.gameObject.transform.position = new Vector3 (x, y, 0);
			}
		} else if (currTile.N && currTile.W) {
			//going S to W
			if (direction.Equals (S)) {
				x = currTile.x-.5f+Mathf.Cos(0-p*Mathf.PI/2)/2;
				y = currTile.y+.5f+Mathf.Sin(0-p*Mathf.PI/2)/2;
				this.gameObject.transform.position = new Vector3 (x, y, 0);
			}
			//going E to N
			else if (direction.Equals (E)) {
				x = currTile.x-.5f+Mathf.Cos(3*Mathf.PI/2+(p*Mathf.PI/2))/2;
				y = currTile.y+.5f+Mathf.Sin(3*Mathf.PI/2+(p*Mathf.PI/2))/2;
				this.gameObject.transform.position = new Vector3 (x, y, 0);
			}
		} else if (currTile.S && currTile.W) {
			//going N to W
			if (direction.Equals (N)) {
				x = currTile.x-.5f+Mathf.Cos(p*Mathf.PI/2)/2;
				y = currTile.y-.5f+Mathf.Sin(p*Mathf.PI/2)/2;
				this.gameObject.transform.position = new Vector3 (x, y, 0);
			}
			//going E to S
			else if (direction.Equals (E)) {
				x = currTile.x-.5f+Mathf.Cos(Mathf.PI/2-(p*Mathf.PI/2))/2;
				y = currTile.y-.5f+Mathf.Sin(Mathf.PI/2-(p*Mathf.PI/2))/2;
				this.gameObject.transform.position = new Vector3 (x, y, 0);
			}
		}
		if(clock-turnStartTime > ((1/speed)/2)){
			isTurning = false;
			direction = destDir;
			finishTurn ();
		}
	}

	public void turn(){
		if(isTurning){
			turnHelper ();
		}
	}

	private void getDirection(int dir){
		if (dir == 0) {
			direction = N;
		} else if (dir == 1) {
			direction = E;
			model.transform.eulerAngles = new Vector3(0,0,270);
		} else if (dir == 2) {
			direction = S;
			model.transform.eulerAngles = new Vector3(0,0,180);
		} else {
			direction = W;
			model.transform.eulerAngles = new Vector3(0,0,90);
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

	private void getNextTileTurning (){
		if (destDir.Equals (N)) {
			nextTile = (Tile)currTile.getNeighbors() [0];
			destpos = new Vector3 (nextTile.x, nextTile.y-.5f, -1);
		} else if (destDir.Equals (S)) {
			nextTile = (Tile)currTile.getNeighbors() [1];
			destpos = new Vector3 (nextTile.x, nextTile.y+.5f, -1);
		} else if (destDir.Equals (E)) {
			nextTile = (Tile)currTile.getNeighbors() [3];
			destpos = new Vector3 (nextTile.x-.5f, nextTile.y, -1);
		} else {
			nextTile = (Tile)currTile.getNeighbors() [2];
			destpos = new Vector3 (nextTile.x+.5f, nextTile.y, -1);
		}
	}

	private void getNextSquare(){
		if (direction.Equals (N)) {
			nextTile = (Tile)currTile.getNeighbors() [0];
			destpos = new Vector3 (nextTile.x, nextTile.y-.5f, -1);
		} else if (direction.Equals (S)) {
			nextTile = (Tile)currTile.getNeighbors() [1];
			destpos = new Vector3 (nextTile.x, nextTile.y+.5f, -1);
		} else if (direction.Equals (E)) {
			nextTile = (Tile)currTile.getNeighbors() [3];
			destpos = new Vector3 (nextTile.x-.5f, nextTile.y, -1);
		} else {
			nextTile = (Tile)currTile.getNeighbors() [2];
			destpos = new Vector3 (nextTile.x+.5f, nextTile.y, -1);
		}
		//destpos = new Vector3 (nextTile.x, nextTile.y, -1);
	}
}

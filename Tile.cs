using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public Vector2 dirN { get { return new Vector2(0, 1); } }
	public Vector2 dirS { get { return new Vector2(0, -1); } }
	public Vector2 dirE { get { return new Vector2(1, 0); } }
	public Vector2 dirW { get { return new Vector2(-1, 0); } }

	public bool N = false;
	public bool S = false;
	public bool E = false;
	public bool W = false;

	private TileModel model1;	
	private TileModel model2;
	public float x;
	public float y;
	public int i;
	public int j;
	private int tiletype;
	private GameManager gm;		

	private bool pit = false;
	private bool turn = false;
	public bool hasGem = false;

	public ArrayList marbles = new ArrayList();

	public ArrayList neighbors = new ArrayList();

	void Update(){
		if (Input.GetMouseButtonUp(0) && isTurn() && !hasMarble() && gm.go && !gm.pause) {
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float mouseX = worldPos.x;
			float mouseY = worldPos.y;
			if (mouseX >= x-.5f && mouseX <= x+.5f && mouseY >= y-.5f && mouseY <= y+.5f) {
				rotateR ();
			}
		}
		if (Input.GetMouseButtonUp(1) && isTurn() && !hasMarble() && gm.go && !gm.pause) {
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float mouseX = worldPos.x;
			float mouseY = worldPos.y;
			if (mouseX >= x-.5f && mouseX <= x+.5f && mouseY >= y-.5f && mouseY <= y+.5f) {
				rotateL ();
			}
		}
	}

	public ArrayList getNeighbors(){
		return neighbors;
	}

	public void init(int i, int j, float x, float y, GameManager gm) {
		this.x = x;
		this.y = y;
		this.i = i;
		this.j = j;
		this.gm = gm;
		this.tiletype = 1;

		//addEmptyTexture ();
	}

	public void addTurn(){
		this.tiletype = 2;

		turn = true;
		S = true;
		E = true;

		var modelObject2 = GameObject.CreatePrimitive (PrimitiveType.Quad);
		model2 = modelObject2.AddComponent<TileModel> ();
		model2.init (x, y, 2, this);

	}

	public void addPit(){
		this.tiletype = 3;

		pit = true;

		var modelObject2 = GameObject.CreatePrimitive (PrimitiveType.Quad);
		modelObject2.tag = "pit";
		model2 = modelObject2.AddComponent<TileModel> ();

		MeshCollider mc = modelObject2.GetComponent<MeshCollider> ();
		mc.enabled = false;

		BoxCollider bc = modelObject2.AddComponent<BoxCollider> ();
		bc.size = new Vector3(.5f, .5f, 3);
		Rigidbody rb = modelObject2.AddComponent<Rigidbody> ();
		rb.useGravity = false;
		rb.isKinematic = false;

		model2.init (x, y, 3, this);
	}

	private bool hasMarble(){
		if (marbles.Count > 0) {
			return true;
		}
		return false;
	}

	public void addEmptyTexture(){
		var modelObject1 = GameObject.CreatePrimitive (PrimitiveType.Quad);	
		model1 = modelObject1.AddComponent<TileModel> ();						
		model1.init (x, y, 1, this);
	}
		

	public bool isTurn(){
		if (turn) {
			return true;
		}
		return false;
	}

	public bool isPit(){
		if(pit){
			return true;
		}
		return false;
	}

	private void rotateR(){
		if (S && E) {
			E = false;
			W = true;
		} else if (S && W) {
			S = false;
			N = true;
		} else if (N && W) {
			W = false;
			E = true;
		} else {
			N = false;
			S = true;
		}
		this.transform.Rotate (0, 0, -90);
	}

	private void rotateL(){
		if (S && E) {
			S = false;
			N = true;
		} else if (N && E) {
			E = false;
			W = true;
		} else if (N && W) {
			N = false;
			S = true;
		} else {
			W = false;
			E = true;
		}
		this.transform.Rotate (0, 0, 90);
	}

	public Vector2 getNewDirection (Vector2 direction){
		if (direction.Equals (dirN) && S) {
			return checkEW ();
		} else if (direction.Equals (dirS) && N) {
			return checkEW ();
		} else if (direction.Equals (dirE) && W) {
			return checkNS ();
		} else if (direction.Equals (dirW) && E) {
			return checkNS ();
		} else {
			return direction;
		}
	}

	private Vector2 checkNS(){
		if (N) {
			return dirN;
		}
		return dirS;
	}

	private Vector2 checkEW(){
		if (E) {
			return dirE;
		}
		return dirW;
	}
}


using UnityEngine;
using System.Collections;

public class Edge : MonoBehaviour {
    public LinePath line;

    public PathNode anchorA;
    public PathNode anchorB;

	// Use this for initialization
	void Start () {
	    //TODO: throw error if edge is invalid (anchorA == anchorB or a duplicate exists)
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

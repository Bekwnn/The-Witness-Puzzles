using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour {
    public float snapRadius = 0.15f;
    private bool bVisited = false;
    public NodePuzzle puzzle;
	
	public bool IsInSnapRadius(Vector3 position)
    {
        return (snapRadius >= Vector3.Distance(position, transform.position));
    }

    public void SetVisted(bool b)
    {
        bVisited = b;
        if (bVisited) GetComponent<MeshRenderer>().material = puzzle.nodeVisitedMat;
        else GetComponent<MeshRenderer>().material = puzzle.nodeUnvisitedMat;
    }

    public bool IsVisited()
    {
        return bVisited;
    }
}
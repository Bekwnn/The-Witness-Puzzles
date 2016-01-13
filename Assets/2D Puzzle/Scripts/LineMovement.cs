using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineMovement : MonoBehaviour {
    public NodePuzzle puzzle;
    public Camera cam;
    public GameObject movementPlane;
    public Transform anchorA;
    public Transform anchorB;
    public Vector3 forwardOffset;
    public LinePath nearTrail;
    public float mouseSensitivity = 1f;

    private Vector3 mousePosLast;
    private float lerpDist;
    private Stack<PathNode> travelPath;
    
	void Start () {
        mousePosLast = MouseScreenToWorldPoint();
        travelPath = new Stack<PathNode>();
	}
	
	void Update () {
        UpdateLerpDist();
        UpdateLerpPos();
        CheckToSwitchSegment();

        mousePosLast = MouseScreenToWorldPoint();
    }

    void UpdateLerpDist()
    {
        Vector3 mouseDelta = MouseScreenToWorldPoint() - mousePosLast;
        mouseDelta *= mouseSensitivity;

        float movement = Vector2.Dot(mouseDelta, (anchorA.position - anchorB.position).normalized);
        lerpDist -= ((anchorA.position - anchorB.position).normalized / Vector2.Distance(anchorA.position, anchorB.position)).magnitude * movement;
        lerpDist = Mathf.Clamp01(lerpDist);
    }

    void UpdateLerpPos()
    {
        transform.position = Vector3.Lerp(anchorA.position + forwardOffset, anchorB.position + forwardOffset, lerpDist);
    }

    void CheckToSwitchSegment()
    {
        PathNode nodeA = anchorA.gameObject.GetComponent<PathNode>();
        PathNode nodeB = anchorB.gameObject.GetComponent<PathNode>();
        if (nodeA.IsInSnapRadius(transform.position)) GetNewPath(nodeA);
        else if (nodeB.IsInSnapRadius(transform.position)) GetNewPath(nodeB);
    }

    void GetNewPath(PathNode pathNode)
    {
        //if we're trying to get a new path at a visited node that isn't the last one we visited, we're trying to crossover and should stop.
        if (pathNode.IsVisited() && travelPath.Count > 0 && travelPath.Peek() != pathNode)
        {
            Debug.Log("Flashing!");
            puzzle.lineWarning.Flash();
            return;
        }

        //pop the node we're at off the travel stack if it's on it (gets readded later so long as we're not doubling back)
        if (travelPath.Count > 0 && travelPath.Peek() == pathNode) travelPath.Pop();

        //determine best neighbor to path to next based on mouse delta
        Vector3 mouseDelta = MouseScreenToWorldPoint() - mousePosLast;
        List<PathNode> neighbors = puzzle.GetNeighborsOf(pathNode);
        PathNode bestNeighbor = neighbors[0];
        float mouseDeltaDotBest = -2f;

        foreach (PathNode neighbor in neighbors)
        {
            float curDotPath = Vector2.Dot(mouseDelta.normalized, (neighbor.transform.position - pathNode.transform.position).normalized);
            
            if (curDotPath > mouseDeltaDotBest)
            {
                bestNeighbor = neighbor;
                mouseDeltaDotBest = curDotPath;
            }
        }

        //set node's visited status
        //if best neighbor isn't part of travel path then add this node to travel path and set as traveled
        if (!(travelPath.Count > 0 && travelPath.Peek() == bestNeighbor))
        {
            pathNode.SetVisted(true);

            if (travelPath.Count > 0)
            {
                Edge e = puzzle.GetEdge(pathNode, travelPath.Peek());
                e.line.GetComponent<MeshRenderer>().material = puzzle.lineVisitedMat;
            }
            nearTrail.startAnchor = pathNode.gameObject;

            travelPath.Push(pathNode);
        }
        //if best neighbor is part of travel path then we're backtracking.
        else
        {
            pathNode.SetVisted(false);

            if (travelPath.Count > 0)
            {
                Edge e = puzzle.GetEdge(pathNode, travelPath.Peek());
                e.line.GetComponent<MeshRenderer>().material = puzzle.lineUnvisitedMat;
                nearTrail.startAnchor = travelPath.Peek().gameObject;
            }
            else nearTrail.startAnchor = gameObject;
        }
        nearTrail.Refresh();

        //set new path anchors
        anchorA = pathNode.gameObject.transform;
        anchorB = bestNeighbor.gameObject.transform;
        if (lerpDist > pathNode.snapRadius / Vector2.Distance(anchorA.position, anchorB.position)) lerpDist = 0f;
    }

    Vector3 MouseScreenToWorldPoint()
    {
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Vector3.Distance(cam.transform.position, movementPlane.transform.position)));
    }
}

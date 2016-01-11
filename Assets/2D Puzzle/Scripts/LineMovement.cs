using UnityEngine;
using System.Collections;

public class LineMovement : MonoBehaviour {
    public Camera cam;
    public GameObject movementPlane;
    public Transform anchorA;
    public Transform anchorB;
    public Vector3 forwardOffset;
    private Vector3 mousePosLast;
    private float lerpDist;
    
	void Start () {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        mousePosLast = MouseScreenToWorldPoint();
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
        Debug.Log("Getting new path");
        //TODO: determine anchorB
        Vector3 mouseDelta = MouseScreenToWorldPoint() - mousePosLast;
        PathNode bestNeighbor = pathNode.neighbors[0];
        float mouseDeltaDotBest = -2f;
        foreach (PathNode neighbor in pathNode.neighbors)
        {
            float curDotPath = Vector2.Dot(mouseDelta.normalized, (neighbor.transform.position - pathNode.transform.position).normalized);
            Debug.Log(curDotPath);
            if (curDotPath > mouseDeltaDotBest)
            {
                bestNeighbor = neighbor;
                mouseDeltaDotBest = curDotPath;
            }
        }

        anchorA = pathNode.gameObject.transform;
        anchorB = bestNeighbor.gameObject.transform;
        if (lerpDist > pathNode.snapRadius / Vector2.Distance(anchorA.position, anchorB.position) ) lerpDist = 0f;
    }

    Vector3 MouseScreenToWorldPoint()
    {
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Vector3.Distance(cam.transform.position, movementPlane.transform.position)));
    }
}

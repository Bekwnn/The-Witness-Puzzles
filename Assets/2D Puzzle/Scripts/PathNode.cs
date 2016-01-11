using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour {
    public PathNode[] neighbors;
    public float snapRadius = 0.15f;
    public bool bLinesGenerated = false;
    private bool bVisited = false;
    public Material lineVisitedMat;
    public Material lineUnvisitedMat;
    public Material nodeVisitedMat;
    public Material nodeUnvisitedMat;

    // Use this for initialization
    void Start () {
        foreach (PathNode neighbor in neighbors)
        {
            if (!neighbor.bLinesGenerated)
            {
                GameObject line = new GameObject("Line");
                Instantiate<GameObject>(line);
                line.AddComponent<MeshFilter>();

                MeshRenderer renderer = line.AddComponent<MeshRenderer>();
                renderer.material = lineUnvisitedMat;

                LinePath linePath = line.AddComponent<LinePath>();
                linePath.lineWidth = 1f;
                linePath.startAnchor = gameObject;
                linePath.endAnchor = neighbor.gameObject;
            }
        }
        bLinesGenerated = true;
	}
	
	public bool IsInSnapRadius(Vector3 position)
    {
        return (snapRadius >= Vector3.Distance(position, transform.position));
    }

    public void SetVisted(bool b)
    {
        bVisited = b;
        if (bVisited) GetComponent<MeshRenderer>().material = nodeVisitedMat;
        else GetComponent<MeshRenderer>().material = nodeUnvisitedMat;
    }

    public bool IsVisited()
    {
        return bVisited;
    }
}
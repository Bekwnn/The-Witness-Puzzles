using UnityEngine;
using System.Collections.Generic;

public class NodePuzzle : MonoBehaviour {

    public Material lineVisitedMat;
    public Material lineUnvisitedMat;
    public Material nodeVisitedMat;
    public Material nodeUnvisitedMat;

    public List<PathNode> vertices;
    [HideInInspector] public List<Edge> edges;


    // Use this for initialization
    void Start() {
        edges = new List<Edge>();
        GetComponents(edges);

        foreach (Edge e in edges)
        {
            GameObject newLine = new GameObject("Line");
            newLine.AddComponent<MeshFilter>();
            newLine.AddComponent<MeshRenderer>().material = lineUnvisitedMat;

            e.line = newLine.AddComponent<LinePath>();
            e.line.lineWidth = 1f;
            e.line.startAnchor = e.anchorA.gameObject;
            e.line.endAnchor = e.anchorB.gameObject;
        }
    }


    public List<PathNode> GetNeighborsOf(PathNode main)
    {
        List<PathNode> retList = new List<PathNode>();
        foreach (Edge e in edges)
        {
            if (e.anchorA == main)
            {
                retList.Add(e.anchorB);
            }
            else if (e.anchorB == main)
            {
                retList.Add(e.anchorA);
            }
        }
        return retList;
    }

    public Edge GetEdge(PathNode a, PathNode b)
    {
        foreach (Edge e in edges)
        {
            if ((e.anchorA == a && e.anchorB == b) || (e.anchorA == b && e.anchorB == a))
                return e;
        }
        return null;
    }
}

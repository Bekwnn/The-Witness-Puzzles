using UnityEngine;
using System.Collections;

public class LinePath : MonoBehaviour {
    public GameObject startAnchor;
    public GameObject endAnchor;
    public float lineWidth = 1f;

    [HideInInspector] public MeshFilter meshComponent;

    void Start()
    {
        if (meshComponent == null) meshComponent = GetComponent<MeshFilter>();
        
        meshComponent.mesh = new Mesh();
    }

    void Update()
    {
        transform.position = (startAnchor.transform.position + endAnchor.transform.position) / 2f;
        //TODO: perform a check to see if positions have changed before doing all this crap
        Vector3 orthUnitVec = Vector3.Cross(
            startAnchor.transform.position - endAnchor.transform.position,
            Vector3.forward
        ).normalized;

        //create quad vertices
        Vector3 p0 = startAnchor.transform.position + orthUnitVec * lineWidth / 2f - transform.position;
        Vector3 p1 = startAnchor.transform.position - orthUnitVec * lineWidth / 2f - transform.position;
        Vector3 p2 = endAnchor.transform.position   + orthUnitVec * lineWidth / 2f - transform.position;
        Vector3 p3 = endAnchor.transform.position   - orthUnitVec * lineWidth / 2f - transform.position;

        meshComponent.mesh.Clear();
        meshComponent.mesh.vertices = new Vector3[] { p0, p1, p2, p3 };
        meshComponent.mesh.triangles = new int[]{
            0,1,2,
            0,2,3,
            2,1,3,
            0,3,1
        };

        meshComponent.mesh.RecalculateNormals();
        meshComponent.mesh.RecalculateBounds();
        meshComponent.mesh.Optimize();
    }
}

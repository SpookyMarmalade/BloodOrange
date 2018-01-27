using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour {
    public float viewRadius;
    [Range(0,361)]
    public float viewAngle;
    public float meshResolution;
    public float obstaclePenetration;
    public LayerMask obstacleMask;

    public MeshFilter viewMeshFilter;

    Mesh viewMesh;
    
    void Start() {
        viewMesh = new Mesh();
        viewMesh.name = "Vision Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    void LateUpdate() {
        DrawVision();
    }

    public void DrawVision() {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i< stepCount; i++) {
            float angle = transform.eulerAngles.z - viewAngle / 2 + stepAngleSize* i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point + newViewCast.dir*obstaclePenetration);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            vertices[i + 1].z = 0;
            if (i < vertexCount - 2)
            {
                triangles[i * 3 + 0] = 0;
                triangles[i * 3 + 1] = i + 2;
                triangles[i * 3 + 2] = i + 1;
            }
        }
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    public struct ViewCastInfo {
        public bool hit;
        public Vector2 point;
        public Vector2 dir;
        public float distance;
        public float angle;

        public ViewCastInfo(bool hit, Vector2 point, Vector2 dir, float distance, float angle) {
            this.hit = hit;
            this.point = point;
            this.dir = dir;
            this.distance = distance;
            this.angle = angle;
        }
    }
    ViewCastInfo ViewCast(float globalAngle) {
        Vector2 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);
        if (hit.collider != null) {
            return new ViewCastInfo(true, hit.point, dir, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo(false, transform.position + (Vector3)(dir * viewRadius), dir, viewRadius, globalAngle);
        }
    }

    Vector2 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), -Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

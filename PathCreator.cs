using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public GameObject player;
    public Vector3 startPosition;
    public LineRenderer lineRenderer;
    public List<Vector3> linePath = new List<Vector3>();
    public Vector3[] linePositions;

    private void Start()
    {
        startPosition = transform.position;
    }
    void Update()
    {

        if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            //Ray ray2 = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            Physics.Raycast(ray, out RaycastHit hit);
            Vector3 direction = new Vector3(hit.point.x, 0.1f, hit.point.z);
            transform.position = direction;
        }

        if (Vector3.Distance(startPosition, transform.position) > 0.01f)
        {
            player.GetComponent<Player>().path.Add(transform.position);
            startPosition = transform.position;
            linePath.Add(transform.position);
            linePositions = new Vector3[linePath.Count];
            for (int i = 0; i < linePath.Count; i++)
            {
                linePositions[i] = new Vector3(linePath[i].x, 0.1f, linePath[i].z);
            }

            lineRenderer.positionCount = linePath.Count;
            lineRenderer.SetPositions(linePositions);
        }


    }
}

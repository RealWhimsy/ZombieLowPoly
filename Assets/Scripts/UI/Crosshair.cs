using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.025f;
        lineRenderer.useWorldSpace = false;
		lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
		var pos = new Vector3(0, transform.position.y, -20);
		var endPos = new Vector3(pos.x, pos.y, pos.z + 400);
        lineRenderer.SetPosition(0, pos);
		lineRenderer.SetPosition(1, endPos);
    }
}

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
		EventManager.StartListening(Const.Events.PlayerDead, DisableCrosshair);
		EventManager.StartListening(Const.Events.PlayerRespawned, EnableCrosshair);
		EventManager.StartListening(Const.Events.MeleeEquipped, DisableCrosshair);
		EventManager.StartListening(Const.Events.GunEquipped, EnableCrosshair);
    }

    // Update is called once per frame
    void Update()
    {
		var pos = new Vector3(0, transform.position.y, -20);
		var endPos = new Vector3(pos.x, pos.y, pos.z + 400);
        lineRenderer.SetPosition(0, pos);
		lineRenderer.SetPosition(1, endPos);
    }

	private void DisableCrosshair() {
		if(lineRenderer.enabled)
			lineRenderer.enabled = false;
	}

	private void EnableCrosshair() {
		if(!lineRenderer.enabled)
			lineRenderer.enabled = true;
	}
}

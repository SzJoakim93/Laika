using UnityEngine;
using System.Collections;

public class cameraFollowing : MonoBehaviour {

    public Transform player;
    public Vector3 distance;
    private float zoom = 10f;
    public float pitch = 2f;
	
	void LateUpdate () {
        //transform.position = player.position - distance * zoom;
        transform.LookAt(player.position + Vector3.up * pitch);
	}
}

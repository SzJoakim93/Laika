using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State {
    idle,
    opening,
    closing
}

public class Door : MonoBehaviour
{
    public Transform Player;
    bool isOpen = true;
    State state;
    float openState;
    float closeState;
    float openTime = -10.0f;

    // Start is called before the first frame update
    void Start()
    {
        closeState = transform.position.y;
        openState = transform.position.y + 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.opening) {
            transform.Translate(0.0f, 10.0f * Time.deltaTime, 0.0f);
            if (transform.position.y > openState) {
                state = State.idle;
            }
        }

        if (state == State.closing) {
            transform.Translate(0.0f, -10.0f * Time.deltaTime, 0.0f);
            if (transform.position.y < closeState) {
                transform.position = new Vector3(transform.position.x, closeState, transform.position.z);
                state = State.idle;
            }
        }

        if (Time.timeSinceLevelLoad - openTime > 5.0f)
            state = State.closing;

        if (Vector3.Distance(Player.position, transform.position) < 3.0f) {
            state = State.opening;
            openTime = Time.timeSinceLevelLoad;
        }
    }

    void OnTriggerEnter(Collider coll) {
        if (isOpen && coll.CompareTag("Player"))
            state = State.opening;
            openTime = Time.timeSinceLevelLoad;
    }
}

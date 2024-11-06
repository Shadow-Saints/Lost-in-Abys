using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform headPos;

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, headPos.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(headPos.position, headPos.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Raycast hit: " + hit.transform.name);

            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (distance <= 3f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Mouse button clicked. Object hit: " + hit.transform.name);

                    if (hit.transform.GetComponent<KeypadKey>() != null)
                    {
                        Debug.Log("KeypadKey detected. Sending key: " + hit.transform.GetComponent<KeypadKey>().key);
                        hit.transform.GetComponent<KeypadKey>().SendKey();
                    }
                    else if (hit.transform.name == "DoorMesh")
                    {
                        Debug.Log("Door detected. Attempting to open.");
                        hit.transform.GetComponent<Door>().OpenDoor();
                    }
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] float speed;
    [SerializeField] float mouseSensibility;

    [SerializeField] GameObject[] cameras;
    int currentCameraIndex = 0;
    // Update is called once per frame
    void Update ()
    {
        Move();
        AjustCamera();

        InputSwitchCamera();
    }

    private void InputSwitchCamera()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            foreach(GameObject camera in cameras)
            {
                camera.SetActive(false);
            }
            cameras[currentCameraIndex].SetActive(true);
        }
    }

    private void AjustCamera()
    {
        if (mouseSensibility == 0)
            return;

        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        Vector3 rotation = new Vector3(-y, x, 0);

        transform.eulerAngles += rotation * mouseSensibility * Time.deltaTime;
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float z = 0;
        if (Input.GetKey(KeyCode.Q))
            z = 1;
        else if (Input.GetKey(KeyCode.E))
            z = -1;

        Vector3 input = new Vector3(x, z, y);

        if (input.magnitude > 1)
            input.Normalize();

        Vector3 direction = transform.forward * y + transform.up * z + transform.right * x;
        //transform.position += transform.TransformDirection(input) * speed * Time.deltaTime;
        transform.position += direction * speed * Time.deltaTime;

    }
}

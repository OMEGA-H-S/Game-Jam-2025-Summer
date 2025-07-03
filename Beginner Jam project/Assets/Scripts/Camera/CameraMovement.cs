using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    private Transform playerPos;
    [SerializeField] private float aheadDistanceX = 1f;
    [SerializeField] private float aheadDistanceY = 3f;
    [SerializeField] private float cameraSpeed = 0.8f;
    Camera camera;
    [SerializeField] private float orthSize = 10f;
    private Vector2 xBounds; //Temporary, will adjust values based on map bounds
    private Vector2 yBounds;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        camera = GetComponent<Camera>();
        camera.orthographicSize = orthSize; //Adjusts the zoom of the camera
        //xBounds will as if right now place virtually no limit on camera x bounds
        xBounds = new Vector2(playerPos.transform.position.x - 1500, playerPos.transform.position.x + 2000);
        //yBounds prevents camera from falling too low (Adjust +4, higher makes camera rise and lower makes camera fall)
        yBounds = new Vector2(playerPos.transform.position.y -500, playerPos.transform.position.y + 500);
    }

    // Update is called once per frame
    void Update()
    {
        //Logic to calculate the target camera location, then a small step in that direction
        Vector3 targetPos = new Vector3(playerPos.localScale.x * aheadDistanceX + playerPos.position.x, playerPos.position.y + aheadDistanceY, -10);
        Vector3 camLoc = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * cameraSpeed);
        //Preventing camera from going too far in some direction
        camLoc = new Vector3(Mathf.Clamp(camLoc.x, xBounds.x, xBounds.y), Mathf.Clamp(camLoc.y, yBounds.x, yBounds.y), -10);
        transform.position = camLoc;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Home");
        }
        
    }
}

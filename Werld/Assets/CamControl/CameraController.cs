using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int floor = 1;

    public float moveSpeed = 17.0f;
    public float lookSpeed = 1.0f;

    private Vector3 lastMousePosition;
    private Vector3 mouseDrag;

    public Vector3 positionBounds = new Vector3(50, 0, 50);

    public static CameraController Instance = null;

    void Awake()
    {
        if (Instance == null)
        {

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Move cam
        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float startingY = transform.position.y;
        transform.Translate(x, 0, z, Space.Self);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -positionBounds.x, positionBounds.x),
            startingY,
            Mathf.Clamp(transform.position.z, -positionBounds.z, positionBounds.z)
        );

        float yTarget = 20 + (Ship.floorHeight * (floor-1));
        transform.Translate(0, (yTarget - transform.position.y) * 0.07f, 0, Space.World);

        // Rotate cam
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            mouseDrag = Input.mousePosition - lastMousePosition;

            //Debug.Log(mouseDrag);
            //transform.Rotate(Vector3.right, mouseDrag.y * lookSpeed);
            transform.Rotate(0, mouseDrag.x * lookSpeed, 0, Space.World);

            lastMousePosition = Input.mousePosition;
        }


        for (int i = 0; i < 8; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1+i))
            {
                SetCameraFloor(i);
            }
        }
    }

    public void SetCameraFloor(int floor)
    {
        this.floor = floor+1;
    }
}

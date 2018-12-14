using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraOrbit : MonoBehaviour
{

    protected Transform _XForm_Parent;

    public Vector2 _LocalRotation = new Vector2(0,40);
    public float minYRotation = 35f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraDisabled = false;

    public float _fieldOfView = 10f;
    public float zoomOutMin = 1.5f;
    public float zoomInitial = 71f;
    public float zoomOutMax = 20f;
    public float zoomSpeed = .1f;

    public bool shouldOrbit = true;

    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    // Use this for initialization
    void Start()
    {
        this._XForm_Parent = this.transform.parent;
        _fieldOfView = zoomInitial;
    }


    void LateUpdate()
    {
        if (shouldOrbit){
            OrbitCamera();    
        }
        else {
            if (Input.touchCount == 3)
            {
                HandleTouchRotation();
            }
            if (Input.touchCount == 2)
            {
                HandleTouchZoom();
            }
            else if (Input.GetMouseButton(1))
            {
                HandleMouseRotation();
                HandleMouseZoom();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                zoom(-.5f);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                zoom(.5f);
            }

        }
        ClampRotation();
        MoveCameraRig();
    }

    private void MoveCameraRig()
    {
        // Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        if (camera.fieldOfView != this._fieldOfView )
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, this._fieldOfView , Time.deltaTime * ScrollDampening);
        }
    }

    private void ClampRotation()
    {
        // Clamp the y Rotation to horizon and not flipping over at the top
        if (_LocalRotation.y < minYRotation)
            _LocalRotation.y = minYRotation;
        else if (_LocalRotation.y > 90f)
            _LocalRotation.y = 90f;
    }

    private void HandleTouchRotation()
    {

    }

    private void HandleTouchZoom()
    {
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

        float difference = currentMagnitude - prevMagnitude;

        zoom(difference);
    }

    private void HandleMouseRotation()
    {
        //Rotation of the Camera based on Mouse Coordinates
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
            _LocalRotation.y += Input.GetAxis("Mouse Y") * MouseSensitivity;
        }
    }

    private void HandleMouseZoom()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;
            zoom(ScrollAmount);
        }
    }

    private void zoom(float increment){
        increment *= (this._fieldOfView * zoomSpeed);

        this._fieldOfView += increment * -1f;

        this._fieldOfView = Mathf.Clamp(this._fieldOfView, zoomOutMin, zoomOutMax);
    }

    public void Reset()
    {
        _LocalRotation.y = 90f;
        _LocalRotation.x = 0f;

        _fieldOfView = zoomInitial;
        shouldOrbit = false;
    }
    public void startOrbit(){
        shouldOrbit = true;
    }

    private void OrbitCamera()
    {
        _LocalRotation.y = 40f;
        _LocalRotation.x += Time.deltaTime * 3;

    }
}
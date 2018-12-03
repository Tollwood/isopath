using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraOrbit : MonoBehaviour
{

    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    public Vector2 _LocalRotation = new Vector2(0,90);
    public float _CameraDistance = 10f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraDisabled = false;

    public float zoomOutMin = 1.5f;
    public float zoomOutMax = 20f;

    public float minYRotation = 35f;
    private new Camera camera;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    // Use this for initialization
    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
    }


    void LateUpdate()
    {
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

        ClampRotation();

        MoveCameraRig();
    }

    private void MoveCameraRig()
    {
        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    }

    private void ClampRotation()
    {
        //Clamp the y Rotation to horizon and not flipping over at the top
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
        increment *= (this._CameraDistance * 0.3f);

        this._CameraDistance += increment * -1f;

        this._CameraDistance = Mathf.Clamp(this._CameraDistance, zoomOutMin, zoomOutMax);
    }
}
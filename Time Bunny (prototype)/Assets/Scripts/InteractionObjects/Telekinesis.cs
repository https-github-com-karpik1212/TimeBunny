using UnityEngine;
public class Telekinesis : MonoBehaviour
{
    [SerializeField] private float grabDistance, grabSpeed, grabDelta;
    private float currectGrabDistance;
    public bool isGrabbing = false;
    private Rigidbody grabbedObject;
    [SerializeField] public bool TelekinesGunIsTaken;
    private float throwForce;
    public float minThrowForce;
    public float maxThrowForce;
    public Camera mainCamera;

    [Header("Audio")]
    [SerializeField] private AudioSource Telekenesis;
    [SerializeField] private AudioClip TelekinesisGrab;

    void Update()
    {
        if (Input.GetMouseButtonUp(1) && isGrabbing)
        {
            ShootObject();
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, grabDistance))
            {
                if (hit.collider.TryGetComponent<Rigidbody>(out grabbedObject))
                {
                    GrabObject();
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && grabbedObject)
        {
            LeaveObject();
        }
    }

    void FixedUpdate()
    {
        if (isGrabbing)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                currectGrabDistance += 2f;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                currectGrabDistance -= 2f;
            }

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = currectGrabDistance;
            Vector3 targetPosition = transform.position + (Camera.main.transform.forward * currectGrabDistance);
            //Camera.main.ScreenToWorldPoint(new Vector2(Screen.width/2, Screen.height/2));
            Vector3 direction = targetPosition - grabbedObject.transform.position;
            if (Vector3.Distance(targetPosition, grabbedObject.transform.position) > grabDelta)
                grabbedObject.velocity = direction.normalized * grabSpeed;
        }
    }

    private void ShootObject()
    {
        throwForce = Mathf.Clamp(throwForce, minThrowForce, maxThrowForce);
        grabbedObject.AddForce(mainCamera.transform.forward * throwForce, ForceMode.Impulse);
        throwForce = minThrowForce;
        LeaveObject();
    }

    private void GrabObject()
    {
        currectGrabDistance = grabDistance;
        grabbedObject.useGravity = false;
        grabbedObject.drag = 10f;
        isGrabbing = true;

        //Telekenesis.clip = TelekinesisGrab;
        //Telekenesis.Play();
    }

    private void LeaveObject()
    {
        grabbedObject.useGravity = true;
        grabbedObject.drag = 1f;
        isGrabbing = false;
    }
}

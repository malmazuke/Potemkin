using UnityEngine;
using System.Collections;

public class GrabTool : MonoBehaviour {

    #region Public Properties
    
    public bool isActive = false;
    public Rigidbody2D grabRB;
    public float minimumThrowSpeed = 10.0f;
    public float throwForce = 10.0f;
    
    #endregion
    
    #region Private Properties
    
    private Camera mainCamera;
    private bool isCurrentlyGrabbingObject = false;
    private GameObject currentGrabbedObject;
    private Vector2 mouseMoveDelta = Vector2.zero;
    private Vector2 mouseLastPosition = Vector2.zero;
    
    private float mouseSpeed {
        get {
            return mouseMoveDelta.magnitude;
        }
    }
    
    #endregion
	
	#region Unity Methods
    
    void Start () {
        mainCamera = Camera.main;
    }
    
	void Update () {
	    if (grabRB == null) {
            return;
        }
        
        if (!isActive) {
            return;
        }
        
        UpdateGrabberPosition ();
        
        if (Input.GetMouseButtonDown (0) && !isCurrentlyGrabbingObject) {
            SetUpMousePosition ();
            AttemptToGrabObject ();
        } else if (Input.GetMouseButton (0)) {
            UpdateMouseSpeed ();
        } else if (Input.GetMouseButtonUp (0) && isCurrentlyGrabbingObject) {
            ReleaseObject ();
        }
	}
    
    #endregion
    
    #region Private Methods
    
    private void SetUpMousePosition () {
        mouseLastPosition = Input.mousePosition;
    }
    
    private void UpdateMouseSpeed () {
        mouseMoveDelta = (Vector2)Input.mousePosition - mouseLastPosition;
        mouseLastPosition = Input.mousePosition;
    }
    
    private void UpdateGrabberPosition () {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 position = mainCamera.ScreenToWorldPoint (mousePosition);
        grabRB.transform.position = position;
    }
    
    private void AttemptToGrabObject () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
         
        if (hit.collider != null) {
            ObjectController oc = hit.transform.gameObject.GetComponent<ObjectController> ();
            if (oc != null && oc.isGrabbable) {
                PickUpObject (hit.transform.gameObject);
            }
        }
    }
    
    private void PickUpObject (GameObject grabbedObject) {
        currentGrabbedObject = grabbedObject;
        isCurrentlyGrabbingObject = true;
    
        grabbedObject.AddComponent<HingeJoint2D> ();
        
        HingeJoint2D hj = grabbedObject.GetComponent<HingeJoint2D> ();
        hj.autoConfigureConnectedAnchor = false;
        hj.anchor = Vector2.zero;
        hj.connectedAnchor = Vector2.zero;
        hj.connectedBody = grabRB;
        
        Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D> ();
        rb.gravityScale = 1.0f;
        
        grabbedObject.layer = 9;
        Physics2D.IgnoreLayerCollision (9, 10, true);
    }
    
    private void ReleaseObject () {
        Destroy (currentGrabbedObject.GetComponent<HingeJoint2D>());
        Rigidbody2D rb = currentGrabbedObject.GetComponent<Rigidbody2D> ();
        rb.gravityScale = 0.0f;
        if (mouseSpeed < minimumThrowSpeed) {
            currentGrabbedObject.layer = 0;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
            currentGrabbedObject.transform.rotation = Quaternion.identity;
        } else {
            rb.AddForce (mouseMoveDelta * throwForce);
            rb.AddTorque (throwForce);
        }
        
        isCurrentlyGrabbingObject = false;
    }
    
    #endregion
}

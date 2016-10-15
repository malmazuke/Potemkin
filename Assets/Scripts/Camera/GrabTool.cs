using UnityEngine;
using System.Collections;

public class GrabTool : MonoBehaviour {

    #region Public Properties
    
    public bool isActive = false;
    public float grabberHeight = 2.0f;
    public Rigidbody2D grabRB;
    public Transform hingePosition;
    
    #endregion
    
    #region Private Properties
    
    private Camera mainCamera;
    private bool isCurrentlyGrabbingObject = false;
    private GameObject currentGrabbedObject;
    
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
            AttemptToGrabObject ();
        } else if (Input.GetMouseButtonUp (0) && isCurrentlyGrabbingObject) {
            ReleaseObject ();
        }
	}
    
    #endregion
    
    #region Private Methods
    
    private void UpdateGrabberPosition () {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 position = mainCamera.ScreenToWorldPoint (mousePosition);
        grabRB.transform.position = position;
    }
    
    private void AttemptToGrabObject () {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, Mathf.Infinity);
         
         if (hit.collider != null && hit.collider.tag == "GrabbableObject") {
            PickUpObject (hit.transform.gameObject);
         }
    }
    
    private void PickUpObject (GameObject grabbedObject) {
        currentGrabbedObject = grabbedObject;
        isCurrentlyGrabbingObject = true;
    
        grabbedObject.AddComponent<HingeJoint2D> ();
        grabbedObject.transform.position = hingePosition.position;
        
        HingeJoint2D hj = grabbedObject.GetComponent<HingeJoint2D> ();
        hj.autoConfigureConnectedAnchor = false;
        hj.connectedBody = grabRB;
        
        Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D> ();
        rb.gravityScale = 1.0f;        
    }
    
    private void ReleaseObject () {
        Destroy (currentGrabbedObject.GetComponent<HingeJoint2D>());
        Rigidbody2D rb = currentGrabbedObject.GetComponent<Rigidbody2D> ();
        rb.gravityScale = 0.0f;
        isCurrentlyGrabbingObject = false;
    }
    
    #endregion
}

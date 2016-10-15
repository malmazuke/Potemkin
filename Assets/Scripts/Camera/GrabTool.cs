using UnityEngine;
using System.Collections;

public class GrabTool : MonoBehaviour {

    #region Public Properties
    
    public bool isActive = false;
    public float grabberHeight = 2.0f;
    public Rigidbody grabRB;
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
        Vector3 mousePosition = Input.mousePosition;
        Vector3 position = mainCamera.ScreenToWorldPoint (mousePosition);
        position.z += position.y - grabberHeight;
        position.y = grabberHeight;
        grabRB.transform.position = position;
    }
    
    private void AttemptToGrabObject () {
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         RaycastHit hit;
         
         if(Physics.Raycast(ray, out hit))
         {
             if (hit.collider.tag == "GrabbableObject") {
                PickUpObject (hit.transform.gameObject);
             }
         }
    }
    
    private void PickUpObject (GameObject grabbedObject) {
        currentGrabbedObject = grabbedObject;
        isCurrentlyGrabbingObject = true;
    
        grabbedObject.AddComponent<HingeJoint> ();
        grabbedObject.transform.position = hingePosition.position;
        
        HingeJoint hj = grabbedObject.GetComponent<HingeJoint> ();
        hj.axis = new Vector3 (0.0f, 1.0f, -1.0f);
        hj.connectedBody = grabRB;
    }
    
    private void ReleaseObject () {
        Destroy (currentGrabbedObject.GetComponent<HingeJoint>());
    
        isCurrentlyGrabbingObject = false;
    }
    
    #endregion
}

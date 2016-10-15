using UnityEngine;
using System.Collections;

public class GrabTool : MonoBehaviour {

    #region Public Properties
    
    public bool isActive = false;
    public Rigidbody grabRB;
    public Transform hingePosition;
    
    #endregion
    
    #region Private Properties
    
    private Camera mainCamera;
    private Vector3 oldClickPosition;
    private bool isCurrentlyGrabbingObject = false;
    private GameObject currentGrabbedObject;
    
    #endregion
	
	#region Unity Methods
    
    void Start () {
        oldClickPosition = Input.mousePosition;
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
            oldClickPosition = Input.mousePosition;
            AttemptToGrabObject ();
        } else if (Input.GetMouseButtonUp (0) && isCurrentlyGrabbingObject) {
            ReleaseObject ();
        }
	}
    
    #endregion
    
    #region Private Methods
    
    private void UpdateGrabberPosition () {
        Vector3 newClickPosition = Input.mousePosition;
        Vector3 newPosition = mainCamera.transform.TransformDirection ((Vector3)((newClickPosition - oldClickPosition) * mainCamera.orthographicSize / mainCamera.pixelHeight * 2f));
        newPosition.z += newPosition.y;
        newPosition.y = 0.0f;
        grabRB.transform.position += newPosition;
        oldClickPosition = newClickPosition;
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
        grabbedObject.AddComponent<HingeJoint> ();
        grabbedObject.transform.position = hingePosition.position;
        
        HingeJoint hj = grabbedObject.GetComponent<HingeJoint> ();
        hj.axis = new Vector3 (1.0f, 0.0f, 0.0f);
        hj.connectedBody = grabRB;
        
        isCurrentlyGrabbingObject = true;
    }
    
    private void ReleaseObject () {
        isCurrentlyGrabbingObject = false;
    }
    
    #endregion
}

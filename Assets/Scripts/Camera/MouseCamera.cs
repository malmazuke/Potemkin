using UnityEngine;

public class MouseCamera : MonoBehaviour {
    
    #region Public Properties
    
    public Camera mainCamera;
    
    #endregion

    #region Private Properties
    
    Vector2 oldClickPosition;
    
    #endregion

    #region Unity Methods

    void Update () {
        if (Input.GetMouseButtonDown (1)) {
            oldClickPosition = Input.mousePosition;
        } else if (Input.GetMouseButton (1)) {
            Vector2 newClickPosition = Input.mousePosition;
            mainCamera.transform.position += mainCamera.transform.TransformDirection ((Vector3)((oldClickPosition - newClickPosition) * mainCamera.orthographicSize / mainCamera.pixelHeight * 2f));
            oldClickPosition = newClickPosition;
        }
    }
    
    #endregion
}

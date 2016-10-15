using UnityEngine;

public class TouchCamera : MonoBehaviour {

    #region Public Properties
    
    public Camera mainCamera;
    public bool allowsZoomAndRotation = false;
    
    #endregion
    
    #region Private Properties
    
	Vector2?[] oldTouchPositions = {
		null,
		null
	};
	Vector2 oldTouchVector;
	float oldTouchDistance;
    
    #endregion

    #region Unity Methods

	void Update() {
		if (Input.touchCount == 0) {
			oldTouchPositions[0] = null;
			oldTouchPositions[1] = null;
		}
		else if (Input.touchCount == 1) {
			if (oldTouchPositions[0] == null || oldTouchPositions[1] != null) {
				oldTouchPositions[0] = Input.GetTouch(0).position;
				oldTouchPositions[1] = null;
			}
			else {
				Vector2 newTouchPosition = Input.GetTouch(0).position;
				Vector3 newPos = mainCamera.transform.TransformDirection((Vector3)((oldTouchPositions[0] - newTouchPosition) * mainCamera.orthographicSize / mainCamera.pixelHeight * 2f));
                newPos.z += newPos.y;
                newPos.y = 0;
				mainCamera.transform.position += newPos;
				
				oldTouchPositions[0] = newTouchPosition;
			}
		}
		else if (allowsZoomAndRotation) {
			if (oldTouchPositions[1] == null) {
				oldTouchPositions[0] = Input.GetTouch(0).position;
				oldTouchPositions[1] = Input.GetTouch(1).position;
				oldTouchVector = (Vector2)(oldTouchPositions[0] - oldTouchPositions[1]);
				oldTouchDistance = oldTouchVector.magnitude;
			}
			else {
				Vector2 screen = new Vector2(mainCamera.pixelWidth, mainCamera.pixelHeight);
				
				Vector2[] newTouchPositions = {
					Input.GetTouch(0).position,
					Input.GetTouch(1).position
				};
				Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
				float newTouchDistance = newTouchVector.magnitude;

				mainCamera.transform.position += mainCamera.transform.TransformDirection((Vector3)((oldTouchPositions[0] + oldTouchPositions[1] - screen) * mainCamera.orthographicSize / screen.y));
				mainCamera.transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(Mathf.Clamp((oldTouchVector.y * newTouchVector.x - oldTouchVector.x * newTouchVector.y) / oldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f));
				mainCamera.orthographicSize *= oldTouchDistance / newTouchDistance;
				mainCamera.transform.position -= mainCamera.transform.TransformDirection((newTouchPositions[0] + newTouchPositions[1] - screen) * mainCamera.orthographicSize / screen.y);

				oldTouchPositions[0] = newTouchPositions[0];
				oldTouchPositions[1] = newTouchPositions[1];
				oldTouchVector = newTouchVector;
				oldTouchDistance = newTouchDistance;
			}
		}
	}
    
    #endregion
}

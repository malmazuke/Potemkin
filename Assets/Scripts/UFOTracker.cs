using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UFOTracker : MonoBehaviour {
    
    #region Public Properties
    
    public UFOController ufo;
    public Image cursorImage;
    
    #endregion
    
    #region Private Properties
    
    Camera mainCamera;
    Vector2 UFOScreenPosition;
    
    bool isUFOOnScreen {
        get {
            UFOScreenPosition = mainCamera.WorldToViewportPoint(ufo.transform.position);
            return (UFOScreenPosition.x >= 0 && UFOScreenPosition.x <= 1 && UFOScreenPosition.y >= 0 && UFOScreenPosition.y <= 1);
        }
    }
    
    #endregion
    
	#region Unity Methods
    
    void Start () {
        mainCamera = Camera.main;
    }
    
	void Update () {
	    if (isUFOOnScreen) {
            cursorImage.enabled = false;
        } else {
            UpdateCursorPosition ();
        }
	}
    
    #endregion
    
    #region Private Methods
    
    void UpdateCursorPosition () {
        if (!cursorImage.isActiveAndEnabled) {
            cursorImage.enabled = true;
        }
        
        // http://answers.unity3d.com/questions/1037969/arrows-pointing-to-offscreen-enemy.html
        Vector2 onScreenPos = new Vector2 (UFOScreenPosition.x - 0.5f, UFOScreenPosition.y - 0.5f) * 2.0f;
        float max = Mathf.Max (Mathf.Abs (onScreenPos.x), Mathf.Abs (onScreenPos.y));
        onScreenPos = (onScreenPos / (max*2)) + new Vector2 (0.5f, 0.5f);
        
        onScreenPos.x = 0.9f * onScreenPos.x + 0.05f;
        onScreenPos.y = 0.9f * onScreenPos.y + 0.05f;
        
        Rect canvasRect = cursorImage.canvas.GetComponent<RectTransform> ().rect;
        cursorImage.rectTransform.position = new Vector2 (canvasRect.width * onScreenPos.x, canvasRect.height * onScreenPos.y);
    }
    
    #endregion
}

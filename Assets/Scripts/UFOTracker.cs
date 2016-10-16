using UnityEngine;
using System.Collections;

public class UFOTracker : MonoBehaviour {
    
    #region Public Properties
    
    public UFOController ufo;
    public GameObject cursorPrefab;
    
    #endregion
    
    #region Private Properties
    
    GameObject cursor;
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
    
    void Awake () {
        CreateCursor ();
    }
    
    void Start () {
        mainCamera = Camera.main;
    }
    
	void Update () {
	    if (isUFOOnScreen) {
            cursor.SetActive (false);
        } else {
            UpdateCursorPosition ();
        }
	}
    
    #endregion
    
    #region Private Methods
    
    void CreateCursor () {
        cursor = Instantiate (cursorPrefab, Vector2.zero, Quaternion.identity) as GameObject;
        cursor.SetActive (false);
    }
    
    void UpdateCursorPosition () {
        if (!cursor.activeSelf) {
            cursor.SetActive (true);
        }
        
        // http://answers.unity3d.com/questions/1037969/arrows-pointing-to-offscreen-enemy.html
        Vector2 onScreenPos = new Vector2 (UFOScreenPosition.x - 0.5f, UFOScreenPosition.y - 0.5f) * 2.0f;
        float max = Mathf.Max (Mathf.Abs (onScreenPos.x), Mathf.Abs (onScreenPos.y));
        onScreenPos = (onScreenPos / (max*2)) + new Vector2 (0.5f, 0.5f);
        
        onScreenPos.x += (onScreenPos.x > 0.5f) ? -0.1f : 0.1f;
        onScreenPos.y += (onScreenPos.y > 0.5f) ? -0.1f : 0.1f;
        
        Vector2 screenPos = mainCamera.ViewportToWorldPoint (onScreenPos);
        cursor.transform.position = screenPos;
    }
    
    #endregion
}

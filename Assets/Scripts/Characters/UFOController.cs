using UnityEngine;
using System.Collections;

public enum UFOPhase {
    Stopped,
    Moving,
    Observing
}

public class UFOController : MonoBehaviour {

    #region Public Properties
    
    public UFOPhase currentPhase;
    public float verticalFloatingMagnitide = 0.03f;
    public float verticalFloatingSpeed = 5.0f;
    public float movementSpeed = 1.0f;
    public float phaseLength = 1.0f;
    
    #endregion

	#region Unity Methods
    
	void Awake () {
	    currentPhase = UFOPhase.Stopped;
	}
	
	void Update () {
        Oscillate ();
        
	    if (currentPhase == UFOPhase.Stopped) {
            StartCoroutine (Move ((int)phaseLength));
        }
	}
    
    #endregion
    
    #region Private Methods
    
    void Oscillate () {
        transform.localPosition = new Vector2 (transform.localPosition.x, Mathf.Sin (Time.time * verticalFloatingSpeed) * verticalFloatingMagnitide);
    }
    
    IEnumerator Move (int lengthInSeconds) {
        currentPhase = UFOPhase.Moving;
        float phaseRemaining = 0.0f;
        float totalLength = (float)lengthInSeconds;
        
        while (phaseRemaining < totalLength) {
            transform.position = Vector2.MoveTowards (transform.position, (Vector2)transform.position + new Vector2 (1.0f, 0.0f), movementSpeed * Time.deltaTime);
            
            phaseRemaining += Time.deltaTime;
            yield return null;
        }
        
        currentPhase = UFOPhase.Stopped;
    }
    
    #endregion
}

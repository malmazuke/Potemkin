using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageDialog : MonoBehaviour {

	public int textSlowness = 2;
	public int textDisplayTime = 180;

	private int _frameCounter = 0;
	private string _message;
	private int _messageDisplayIndex = -1;
	private int _messageDisplayTime = -1;

	// Use this for initialization
	void Awake () {
		this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		_frameCounter++;

		// If we have a message to show - show it
		// We use textSlowness to ensure this logic only runs
		// once every several frames
		if (this._message != null && _frameCounter % textSlowness == 0) {
			// If the message is still rolling out, then show the next character
			if (this._messageDisplayIndex < this._message.Length) {
				this._messageDisplayIndex++;

				var txt = this.GetComponentInChildren<Text> ();
				txt.text = this._message.Substring (0, this._messageDisplayIndex);
			} else {
				if (++this._messageDisplayTime > textDisplayTime) {
					Hide ();
				}
			}
		}
	}

	// TODO specify the character that is talking
	public void Show(string message) {
		if (IsShown ()) {
			Hide ();
		}
		this._message = message;
		this._messageDisplayIndex = 0;
		this._messageDisplayTime = 0;
		this.gameObject.SetActive(true);
	}

	// Hides the current message
	public void Hide() {
		this._message = null;
		this._messageDisplayIndex = -1;
		this._messageDisplayTime = -1;
		this.gameObject.SetActive(false);
	}

	public bool IsShown() {
		return this._message != null;
	}
}

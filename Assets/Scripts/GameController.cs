using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public InputField To;

	public InputField Message;

	public Button Send;

	public Text Result;

	private WhatsAppMessaging Sender;

	// Use this for initialization
	void Start () {
		InputField to = this.To.GetComponent<InputField> ();
		to.onEndEdit.AddListener (this.ToEndEdit);
		to.text = Singleton<WhatsAppModel>.Instance.To;

		InputField message = this.Message.GetComponent<InputField> ();
		message.onEndEdit.AddListener (this.MessageEndEdit);
		message.text = Singleton<WhatsAppModel>.Instance.Message;

		Button button = this.Send.GetComponent<Button> ();
		button.onClick.AddListener (this.SendClick);

		Sender = this.gameObject.AddComponent<WhatsAppMessaging> ();
	}

	public void ToEndEdit (string text) {
		Singleton<WhatsAppModel>.Instance.To = text;
	}

	public void MessageEndEdit (string text) {
		Singleton<WhatsAppModel>.Instance.Message = text;
	}

	public void SendClick () {
		string to = Singleton<WhatsAppModel>.Instance.To;
		string body = Singleton<WhatsAppModel>.Instance.Message;
		try 
		{
			Sender.Send (to, body, OnMessageSent);
		}
		catch (NumberException numExc) 
		{
			Text text = this.Result.GetComponent<Text> ();
			text.text = numExc.Message;
		}
	}

	private void OnMessageSent (bool sent, string result) {
		Text text = this.Result.GetComponent<Text> ();
		if (sent) {
			text.text = "Enviado!";
		} else {
			text.text = result;
		}
	}
}

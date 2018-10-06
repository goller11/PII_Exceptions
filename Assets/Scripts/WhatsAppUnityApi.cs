using System;
using System.Collections;
using UnityEngine;

public class WhatsAppMessaging : MonoBehaviour {
	private string Url = "api.twilio.com/2010-04-01/Accounts/";
	private string Service = "/Messages.json";
	private const string AccountSid = "ACfbd57f50e199a28eac49de4cc4acfb8a";
	private const string AuthToken = "81795970808380267013bf04070a5936";
	private const string From = "whatsapp:+14155238886";
	public bool Sent;
	public string Returned;
	private Action<bool, string> OnMessageSent;

	public void Send (string to, string body, Action<bool, string> onMessageSent) {
		if (string.IsNullOrEmpty (to)) 
		{
			throw new NumberException ("Falta el número al que mandás el mensaje.");
		}

		if (!to.StartsWith ("+598")) 
		{
			throw new NumberException ("El número al que mandás el mensaje de comienzar con +598.");
		}

		if (to.Length != 12) 
		{
			throw new NumberException ("Le faltan o sobran dígitos al número al que querés mandar el mensaje.");
		}
		long number;

		if (!Int64.TryParse (to, out number)) 
		{			
			throw new NumberException ("l número al que mandás el mensaje tiene que tener sólo números.");
		}

		if (string.IsNullOrEmpty (body)) 
		{			
			throw new NumberException ("Falta el mensaje a enviar.");
		}

		if (onMessageSent == null) 
		{			
			throw new NumberException ("El parámetro callback no puede ser null.");
		}

		this.OnMessageSent = onMessageSent;

		WWWForm form = new WWWForm ();
		form.AddField ("To", "whatsapp:" + to);
		form.AddField ("From", From);
		form.AddField ("Body", body);

		string completeUrl = "https://" + AccountSid + ":" + AuthToken + "@" + Url + AccountSid + Service;
		Debug.Log (completeUrl);
		WWW www = new WWW (completeUrl, form);
		this.StartCoroutine (WaitForRequest (www));

		//return null;
	}

	private IEnumerator WaitForRequest (WWW www) {
		yield return www;

		bool sent;
		string result;

		sent = www.error == null;
		result = sent ? www.text : www.error;
		Debug.Log (result);

		this.OnMessageSent (sent, result);
	}
}
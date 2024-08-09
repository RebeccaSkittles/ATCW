using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ConnectivityController : MonoBehaviour
{
	public delegate void EnableInputForTutorialCallback(GameObject obj);

	public static string CLIENT_VERSION = "1.0";

	public int UpdateIntervalInSeconds = 60;

	public static int ConnectionLossThreshold = 5;

	private int connectionLossCounter;

	public static EnableInputForTutorialCallback enableInputForTutorialCallback;

	private static ConnectivityController mController;

	private bool isValid = true;

	private bool showingPopup;

	private string popupText = string.Empty;

	private void Awake()
	{
		mController = this;
	}

	public static ConnectivityController GetInstance()
	{
		return mController;
	}

	public void Update()
	{
	}

    public bool CheckInternet()
    {
        StartCoroutine(CheckInternetCoroutine());
        return isValid;
    }

	    private IEnumerator CheckInternetCoroutine()
    {
        UnityWebRequest request = new UnityWebRequest("https://www.google.com");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            isValid = true;
        }
        else
        {
            isValid = false;
        }

        if (!isValid)
        {
            // Handle no internet connection
            Debug.Log("No internet connection");
        }
    }

	public void CheckServer()
	{
		SessionManager instance = SessionManager.GetInstance();
		instance.TestConnectivity();
	}

	public void ServerCallback(TFWebFileResponse response, bool isUserInfo)
	{
    	string data = response.Data;
    	Dictionary<string, object> data2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

    	if (!isValid)
    	{
        return;
    	}

    	if (response.NetworkDown)
    	{
        	connectionLossCounter++;
        	if (connectionLossCounter >= ConnectionLossThreshold)
        	{
            	isValid = false;
            	popupText = KFFLocalization.Get("!!ERROR_REQUIRESINTERNETCONNECTION");
        	}
    	}
    	else if (response.StatusCode == HttpStatusCode.OK)
    	{
        	isValid = true;
        	connectionLossCounter = 0;
        	if (isUserInfo)
        	{
            	string text = TFUtils.LoadString(data2, "version", "unknown");
            if (text != "unknown" && text != CLIENT_VERSION)
        {
                isValid = false;
                popupText = KFFLocalization.Get("!!ERROR_APPLICATIONOUTOFDATE") + "\n\n";
                popupText = popupText + string.Format(KFFLocalization.Get("!!FORMAT_CLIENT_VERSION"), CLIENT_VERSION) + "\n";
                popupText = popupText + string.Format(KFFLocalization.Get("!!FORMAT_SERVER_VERSION"), text) + "\n";
                popupText += string.Format(KFFLocalization.Get("!!FORMAT_RESPONSE_DATA"), response.Data);
        }
        	}
    }
    else if (response.StatusCode == HttpStatusCode.NotFound && isUserInfo)
    {
        isValid = false;
        popupText = KFFLocalization.Get("!!ERROR_SERVERDOWNFORMAINTENANCE");
    }
}

	public void showPopup(string title, string text)
	{
		if (showingPopup)
		{
			return;
		}
		showingPopup = true;
		GameObject gameObject = (GameObject)SLOTGame.InstantiateFX(SLOTGameSingleton<SLOTResourceManager>.GetInstance().LoadResource("Scrollable_Popup"));
		GameObject gameObject2 = (GameObject)SLOTGame.InstantiateFX(SLOTGameSingleton<SLOTResourceManager>.GetInstance().LoadResource("Popup_BlankHalf"));
		Transform transform;
		if (gameObject != null)
		{
			transform = gameObject.transform.Find("Background");
			if (transform != null)
			{
				if (enableInputForTutorialCallback != null)
				{
					Transform transform2 = transform.Find("ScrollBar");
					if (transform2 != null)
					{
						Component[] componentsInChildren = transform2.GetComponentsInChildren(typeof(Collider));
						if (componentsInChildren != null)
						{
							Component[] array = componentsInChildren;
							for (int i = 0; i < array.Length; i++)
							{
								Collider collider = (Collider)array[i];
								if (collider != null)
								{
									enableInputForTutorialCallback(collider.gameObject);
								}
							}
						}
					}
				}
				transform = transform.Find("Button");
				if (transform != null)
				{
					if (enableInputForTutorialCallback != null)
					{
						enableInputForTutorialCallback(transform.gameObject);
					}
					UILabel componentInChildren = transform.GetComponentInChildren<UILabel>();
					if (componentInChildren != null)
					{
						componentInChildren.text = KFFLocalization.Get("!!BACK");
					}
					UIInputEnabler component = transform.GetComponent<UIInputEnabler>();
					if (component != null)
					{
						component.permanent = true;
					}
					transform.gameObject.AddComponent<RestartSceneScript>();
				}
				transform = gameObject.transform.Find("Background");
				transform = transform.transform.Find("Title_Label");
				if (transform != null)
				{
					UILabel componentInChildren2 = transform.GetComponentInChildren<UILabel>();
					if (componentInChildren2 != null)
					{
						componentInChildren2.text = title;
					}
				}
				transform = gameObject.transform.Find("PopupText");
				if (transform != null)
				{
					UILabel componentInChildren3 = transform.GetComponentInChildren<UILabel>();
					if (componentInChildren3 != null)
					{
						componentInChildren3.text = text;
					}
					UIDraggablePanel component2 = transform.GetComponent<UIDraggablePanel>();
					if (component2 != null)
					{
						component2.ResetPosition();
					}
				}
			}
		}
		if (!(gameObject2 != null))
		{
			return;
		}
		transform = gameObject2.transform.Find("BlackPanelHalf");
		if (transform != null)
		{
			UISprite component3 = transform.GetComponent<UISprite>();
			if (component3 != null)
			{
				component3.alpha = 0.75f;
			}
		}
	}
}

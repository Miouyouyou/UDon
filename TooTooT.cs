using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// System.Runtime.Serialization -> Used to save the app basic credentials ( /v1/api/apps )
// System.IO                    -> Idem 
using UnityEngine.UI;

[Serializable]
public class TootAccount {
	// The ID of the account (Not Null)
	public long id;
	// The Username of the account (Not Null)
	public string username;
	// Equals username for local users, includes @domain for remote ones (Not Null)
	public string acct;
	// The account's display name (Not Null)
	public string display_name;
	// Boolean for when the account cannot be followed without waiting for approval first (Not Null)
	public bool locked;
	// The time the account was created (Not Null)
	public long created_at;
	// The number of followers for the account (Not Null)
	public long followers_count;
	// The number of accounts the given account is following (Not Null)
	public long following_count;
	// The number of statuses the account has made (Not Null)
	public long statuses_count;
	// Biography of user (Not Null)
	public string note;
	// URL of the user's profile page (can be remote) (Not Null)
	public string url;
	// URL to the avatar image (Not Null)
	public string avatar;
	// URL to the avatar static image (gif) (Not Null)
	public string avatar_static;
	// URL to the header image (Not Null)
	public string header;
	//	URL to the header static image (gif)
	public string header_static;
}

[Serializable]
public class TootApplication {
	// Name of the app (Not Null)
	public string name;
	// Homepage URL of the app ( ! Can be null ! )
	public string website;
}

[Serializable]
public class TootAttachment {
	// ID of the attachment (Not Null)
	public long id;
	// One of: "image", "video", "gifv" (Not Null)
	public string type;
	// URL of the locally hosted version of the image (Not Null)
	public string url;
	// For remote images, the remote URL of the original image ( ! Can be null ! )
	public string remote_url;
	// URL of the preview image (Not Null)
	public string preview_url;
	// Shorter URL for the image, for insertion into text
	// (only present on local images) ( ! Can be null ! )
	public string text_url;
	// width, height, size (width x height), aspect ( ! Can be null ! )
	public string meta;
}

[Serializable]
public class TootCard {
	// The url associated with the card (Not Null)
	public string url;
	// The title of the card (Not Null)
	public string title;
	// The card description (Not Null)
	public string description;
	// The image associated with the card, if any ( ! Can be null ! )
	public string image;
	// "link", "photo", "video", or "rich" (Not Null)
	public string type;
	// OEmbed data ( ! Can be null ! )
	public string author_name;
	// OEmbed data ( ! Can be null ! )
	public string author_url;
	// OEmbed data ( ! Can be null ! )
	public string provider_name;
	// OEmbed data ( ! Can be null ! )
	public string provider_url;
	// OEmbed data ( ! Can be null ! )
	public string html;
	// OEmbed data ( ! Can be null ! )
	public int width;
	// OEmbed data ( ! Can be null ! )
	public int height;
}

[Serializable]
public class TootContext {
	// The ancestors of the status in the conversation, as a list of Statuses (Not Null)
	public TootStatus[] ancestors;
	// The descendants of the status in the conversation, as a list of Statuses (Not Null)
	public TootStatus[] descendants;
}

[Serializable]
public class TootError {
	// A textual description of the error (Not Null)
	public string error;
}

[Serializable]
public class TootInstance {
	// URI of the current instance (Not Null)
	public string uri;
	// The instance's title (Not Null)
	public string title;
	// A description for the instance (Not Null)
	public string description;
	// An email address which can be used to contact the instance administrator (Not Null)
	public string email;
	// The Mastodon version used by instance. (Not Null)
	public string version;

	override public string ToString() {
		return string.Format(
			"URI         : {0}\n" +
			"Title       : {1}\n" +
			"Description : {2}\n" +
			"Email       : {3}\n" +
			"Version     : {4}\n",
			uri, title, description, email, version
		);
	}
}

[Serializable]
public class TootMention {
	// URL of user's profile (can be remote) (Not Null)
	public string url;
	// The username of the account (Not Null)
	public string username;
	// Equals username for local users, includes @domain for remote ones (Not Null)
	public string acct;
	// Account ID (Not Null)
	public long id;
}

[Serializable]
public class TootNotification {
	// The notification ID (Not Null)
	public long id;
	// One of: "mention", "reblog", "favourite", "follow" (Not Null)
	public string type;
	// The time the notification was created (Not Null)
	public long created_at;
	// The Account sending the notification to the user (Not Null)
	public TootAccount account;
	// The Status associated with the notification, if applicable ( ! Can be null ! )
	public TootStatus status;
}

[Serializable]
public class TootRelationship {
	// Target account id (Not Null)
	public long id;
	// Whether the user is currently following the account (Not Null)
	public long following;
	// Whether the user is currently being followed by the account (Not Null)
	public long followed_by;
	// Whether the user is currently blocking the account (Not Null)
	public bool blocking;
	// Whether the user is currently muting the account (Not Null)
	public bool muting;
	// Whether the user is currently muting boosts from the account (Not Null)
	public bool muting_boosts;
	// Whether the user has requested to follow the account (Not Null)
	public bool requested;
	// Whether the user is currently blocking the user's domain (Not Null)
	public bool domain_blocking;
}

[Serializable]
public class TootReport {
	// The ID of the report (Not Null)
	public long id;
	// The action taken in response to the report (Not Null)
	public string action_taken;
}

[Serializable]
public class TootResult {
	// An array of matched Accounts ( ! Can be null ! )
	public TootAccount[] accounts;
	// An array of matchhed Statuses ( ! Can be null ! )
	public TootStatus[] statuses;
	// An array of matched hashtags, as strings ( ! Can be null ! )
	public string[] hashtags;
}

[Serializable]
public class TootStatus {
	// The ID of the status (Not Null)
	public long id;
	// A Fediverse-unique resource ID (Not Null)
	public string uri;
	// URL to the status page (can be remote) (Not Null)
	public string url;
	// The Account which posted the status (Not Null)
	public string account;
	// null or the ID of the status it replies to ( ! Can be null ! )
	public Nullable<long> in_reply_to_id;
	// null or the ID of the account it replies to ( ! Can be null ! )
	public Nullable<long> in_reply_to_account_id;
	// null or the reblogged Status ( ! Can be null ! )
	public string reblog;
	// Body of the status; this will contain HTML (remote HTML already sanitized) (Not Null)
	public string content;
	// The time the status was created (Not Null)
	public long created_at;
	// The number of reblogs for the status (Not Null)
	public long reblogs_count;
	// The number of favourites for the status (Not Null)
	public long favourites_count;
	// Whether the authenticated user has reblogged the status ( ! Can be null ! )
	public Nullable<bool> reblogged;
	// Whether the authenticated user has favourited the status ( ! Can be null ! )
	public Nullable<bool> favourited;
	// Whether media attachments should be hidden by default ( ! Can be null ! )
	public Nullable<bool> sensitive;
	// If not empty, warning text that should be displayed before the actual content (Not Null)
	public string spoiler_text;
	// One of: public, unlisted, private, direct (Not Null)
	public string visibility;
	// An array of Attachments (Not Null)
	public TootAttachment[] media_attachments;
	// An array of Mentions (Not Null)
	public TootMention[] mentions;
	// An array of Tags (Not Null)
	public TootTag[] tags;
	// 	Application from which the status was posted ( ! Can be null ! )
	public TootApplication application;
	// The detected language for the status (default: en) (Not Null)
	public string language;
}

[Serializable]
public class TootStatusToSend {

	// The text of the status (Not Null)
	public string status;
	// local ID of the status you want to reply to ( ! Can be null ! )
	public Nullable<long> in_reply_to_id;
	// Array of media IDs to attach to the status (maximum 4) ( ! Can be null ! )
	public Nullable<long>[] media_ids;
	// Set this to mark the media of the status as NSFW ( ! Can be null ! )
	public Nullable<bool> sensitive;
	// Text to be shown as a warning before the actual content ( ! Can be null ! )
	public string spoiler_text;
	// Either "direct", "private", "unlisted" or "public" ( ! Can be null ! )
	public string visibility;

	private string message_to_html(string message) {
		return "<p>" + message + "</p>";
	}

	public TootStatusToSend(string message) {
		this.status = message;
		this.visibility = "public";
	}
}

[Serializable]
public class TootTag {
	// The hashtag, not including the preceding # (Not Null)
	public string name;
	// The URL of the hashtag (Not Null)
	public string url;
}

[Serializable]
public class TootAppAuthInfos {
	// Name of your application (Not Null)
	public string client_name;
	// Where the user should be redirected after authorization
	// (for no redirect, use urn:ietf:wg:oauth:2.0:oob) (Not Null)
	public string redirect_uris;
	// This can be a space-separated list of the following items: 
	//   "read", "write" and "follow" (see this page for details on what the scopes do) (Not Null)
	public string scopes;
	// URL to the homepage of your app
	public string website;

	public TootAppAuthInfos() {
		this.client_name   = "UDon";
		this.redirect_uris = "urn:ietf:wg:oauth:2.0:oob";
		this.scopes        = "read write follow";
		this.website       = "https://github.com/Miouyouyou/UDon";
	}
}

public class MyySaveHelpers {

	// Utility function
	private static string rel_abs_from_app_path(string filename) {
		return Application.persistentDataPath + '/' + filename;
	}

	public static void SaveInFile(string filename, object obj) {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(
			rel_abs_from_app_path(filename),
			FileMode.OpenOrCreate
		);
		bf.Serialize(file, obj);
		file.Close();
	}

	public static T LoadFromFile<T>(string filename) {
		Debug.Log("Went there, done that !");
		string abs_file_path = rel_abs_from_app_path(filename);
		T creds = default(T);
		if (File.Exists(abs_file_path)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(abs_file_path, FileMode.Open);
			creds = (T) bf.Deserialize(file);
			file.Close();

			Debug.Log("Deserialized !");
			Debug.Log(creds.ToString());
		}
		return creds;
	}
}

[Serializable]
public class TootAppAuthCreds {
	public long id;
	public string redirect_uri;
	public string client_id;
	public string client_secret;

	override public string ToString() {
		return String.Format(
			"ID            : {0}\n" +
			"Redirect URI  : {1}\n" +
			"Client ID     : {2}\n" +
			"Client Secret : {3}\n",
			id, redirect_uri, client_id, client_secret
		);
	}
}

[Serializable]
public class TootUserAppCreds {
	public string access_token;
	public string token_type;
	public string scope;
	public long created_at;

	override public string ToString() {
		return String.Format(
			"Access token  : {0}\n" +
			"Redirect URI  : {1}\n" +
			"Client ID     : {2}\n" +
			"Client Secret : {3}\n",
			access_token != null && access_token != "",
			token_type,
			scope,
			created_at
		);
	}
}

public class TooTooT : MonoBehaviour {

	public Canvas login_pane;
	public Canvas chat_pane;
	public InputField server;
	public InputField login;
	public InputField password;

	public InputField typed_toot;
	public Text displayed_toots;

	private TootAppAuthCreds app_creds = null;
	private TootUserAppCreds app_user_creds = null;

	private static byte[] string_to_bytes(string data) {
		return System.Text.Encoding.UTF8.GetBytes(data);
	}

	private static IEnumerator post_as_json_with_auth
	(string endpoint_uri, object toot_object_to_send, TootUserAppCreds user_creds)
	{
		string jsonsed_toot_object = JsonUtility.ToJson(toot_object_to_send);
		UnityWebRequest mastodon_api_req = new UnityWebRequest(
			endpoint_uri, 
			"POST",
			new DownloadHandlerBuffer(),
			new UploadHandlerRaw(string_to_bytes(jsonsed_toot_object))
		);
		mastodon_api_req.SetRequestHeader("Content-Type", "application/json");
		mastodon_api_req.SetRequestHeader(
			"Authorization",
			String.Format("Bearer {0}", user_creds.access_token)
		);

		yield return mastodon_api_req.Send();

		if (mastodon_api_req.isError && mastodon_api_req.responseCode >= 400) {
			// This should call a Lambda or Delegate or whatever in the future
			Debug.LogFormat(
				"(´・ω・｀)・・・失敗\nJSON Message was :\n{0}",
				jsonsed_toot_object
			);
		}
		else {
			// Same here
			Debug.LogFormat("Sending a {0} : Success !", toot_object_to_send.GetType());
			Debug.LogFormat("Response : {0}\n", mastodon_api_req.downloadHandler.text);
		}
	}

	private void start_chatting() {
		login_pane.enabled = false;
		chat_pane.enabled = true;
	}

	private bool can_load_valid_user_app_creds() {
		app_user_creds = 
			MyySaveHelpers.LoadFromFile<TootUserAppCreds>("udon_app_user_creds.dat");
		return app_user_creds != null;
	}

	// Use this for initialization
	void Start () {
		//StartCoroutine(GetText());

		if (can_load_valid_user_app_creds()) start_chatting();
		else {
			app_creds = MyySaveHelpers.LoadFromFile<TootAppAuthCreds>("udon_app_creds.dat");
			if (app_creds == null)
				StartCoroutine(GetGlobalAppCredentials());
		}
	}

	IEnumerator GetText() {
		UnityWebRequest req = UnityWebRequest.Get("https://friends.nico/api/v1/instance");

		yield return req.Send();

		if(req.isError) {
			Debug.Log(req.error);
		}
		else {
			string result_text = req.downloadHandler.text;
			// Show results as text
			Debug.Log(result_text);

			// Or retrieve results as binary data
			byte[] results = req.downloadHandler.data;

			Debug.LogFormat("Results length : {0}\n", results.Length);
			TootInstance instance = JsonUtility.FromJson<TootInstance>(result_text);

			Debug.Log(instance.ToString());
		}
	}

	IEnumerator GetGlobalAppCredentials() {
		
		TootAppAuthInfos app_infos = new TootAppAuthInfos();
		string app_req_json_data = JsonUtility.ToJson(app_infos, false);

		Debug.Log(app_req_json_data);

		// Voodoo programming : Copy pasted this from 
		// https://qiita.com/mattak/items/d01926bc57f8ab1f569a
		// However, I have no idea why he uses a UploadHandlerRaw
		// AND a DownloadHandlerBuffer (and not any other) ...
		UnityWebRequest auth_req = new UnityWebRequest(
			"https://friends.nico/api/v1/apps",
			"POST",
			new DownloadHandlerBuffer(),
			new UploadHandlerRaw(string_to_bytes(app_req_json_data))
		);
		auth_req.SetRequestHeader("Content-Type", "application/json");

		yield return auth_req.Send();

		if (auth_req.isError) {	Debug.Log(auth_req.error); }
		else {
			string result_text = auth_req.downloadHandler.text;
			app_creds = JsonUtility.FromJson<TootAppAuthCreds>(result_text);
			MyySaveHelpers.SaveInFile("udon_app_creds.dat", app_creds);
		}

	}

	IEnumerator GetUserAppCredentials(string login, string password) {
		string request_string = String.Format(
			"client_id={0}&client_secret={1}" +
			"&grant_type=password&scope=write&username={2}&password={3}",
			app_creds.client_id, app_creds.client_secret, login, password
		);

		Debug.Log(request_string);

		/* Okay ! Okay ! ... Okay...
		   This *NEEDS* to be documented.
		   
		   If you use UnityWebRequest.Post, the string will automatically be
		   passed through an HTML special characters mangler that will mangle
		   the authenticating string, failing  the authentication.
		   
		   If you use new
		   UnityWebRequest(
		     url, "POST", DownloadHandlerBuffer, UploadHandlerRaw
		   ),
		   the string won't get mangled BUT the Content-Type won't be set
		   and this will also fail the authentication.

		   So, basically, you need BOTH new UnityWebRequest with a Raw
		   Uploader, converting your authenticating string into a byte array
		   AND
		   Setup the Content-Type to applicatino/x-www-form-urlencoded.

		   Lost 1 hour because of this !
		   
		   That reminds me that debugging HTTP issue without a server at
		   hand is extremely difficult, and finding a way to run a quick web
		   server on Windows is stupidly hard. I'll need to know how to
		   sniff SSL content coming from my own machine with Wireshark.

		   If you want to debug similar issues, do NOT change the URL to
		   something other than an address of a peripheral that you
		   *entirely* own. Remember that you're passing all your credentials
		   in the strings. Anyone catching that has a free access to your
		   account.
		 */
		UnityWebRequest user_app_auth_req = new UnityWebRequest(
			"https://friends.nico/oauth/token", 
			"POST",
			new DownloadHandlerBuffer(),
			new UploadHandlerRaw(string_to_bytes(request_string))
		);

		user_app_auth_req.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		yield return user_app_auth_req.Send();

		if (user_app_auth_req.isError || user_app_auth_req.responseCode >= 400) {
			Debug.Log("(´・ω・｀)・・・失敗");
			Debug.Log(user_app_auth_req.downloadHandler.text);
		}
		else {
			string returned_user_app_creds = user_app_auth_req.downloadHandler.text;
			Debug.Log(returned_user_app_creds);
			Debug.Log(user_app_auth_req.responseCode);

			app_user_creds = JsonUtility.FromJson<TootUserAppCreds>(returned_user_app_creds);
			Debug.Log(app_user_creds.ToString());
			MyySaveHelpers.SaveInFile("udon_app_user_creds.dat", app_user_creds);
			start_chatting();
		}
	}

	// Update is called once per frame
	void Update () {
	}

	public void nya() {
		Debug.LogFormat(
			"Server   : {0}\n" +
			"Login    : {1}\n" +
			"Password : {2}\n",
			server.text,
			login.text,
			password.text != null && password.text != ""
		);
		StartCoroutine(GetUserAppCredentials(login.text, password.text));
	}

	public void sendToot() {
		// A 'toot' is actually a Status...
		// So that could be read : TootTootToSend
		// -- Useless comment
		TootStatusToSend toot = new TootStatusToSend(typed_toot.text);

		StartCoroutine(
			post_as_json_with_auth(
				"https://friends.nico/api/v1/statuses",
				toot,
				app_user_creds
			)
		);
	}
}

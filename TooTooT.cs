using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using NUnit.Framework.Internal.Commands;
using System.Configuration;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEditor.VersionControl;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using UnityEditor.Sprites;
using NUnit.Framework.Constraints;



// System.Runtime.Serialization -> Used to save the app basic credentials ( /v1/api/apps )
// System.IO                    -> Idem 
using System.Text;

[Serializable]
public class TootAccount {
	/// The ID of the account (Not Null)
	public long id;
	/// The Username of the account (Not Null)
	public string username;
	/// Equals username for local users, includes @domain for remote ones (Not Null)
	public string acct;
	/// The account's display name (Not Null)
	public string display_name;
	/// Boolean for when the account cannot be followed without waiting for approval first (Not Null)
	public bool locked;
	/// The time the account was created (Not Null)
	public string created_at;
	/// The number of followers for the account (Not Null)
	public long followers_count;
	/// The number of accounts the given account is following (Not Null)
	public long following_count;
	/// The number of statuses the account has made (Not Null)
	public long statuses_count;
	/// Biography of user (Not Null)
	public string note;
	/// URL of the user's profile page (can be remote) (Not Null)
	public string url;
	/// URL to the avatar image (Not Null)
	public string avatar;
	/// URL to the avatar static image (gif) (Not Null)
	public string avatar_static;
	/// URL to the header image (Not Null)
	public string header;
	///	URL to the header static image (gif)
	public string header_static;
}

[Serializable]
public class TootApplication {
	/// Name of the app (Not Null)
	public string name;
	/// Homepage URL of the app ( ! Can be null ! )
	public string website;

	override public string ToString() {
		return "name :" + name + "\nwebsite : " + website;
	}
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
	public string created_at;
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
	public TootAccount account;
	// null or the ID of the status it replies to ( ! Can be null ! )
	public Nullable<long> in_reply_to_id;
	// null or the ID of the account it replies to ( ! Can be null ! )
	public Nullable<long> in_reply_to_account_id;
	// null or the reblogged Status ( ! Can be null ! )
	public string reblog;
	// Body of the status; this will contain HTML (remote HTML already sanitized) (Not Null)
	public string content;
	// The time the status was created (Not Null)
	public string created_at;
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

	override public string ToString() {
		return String.Format("<{0}> {1}", account.display_name, content);
	}
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
			System.IO.FileMode.OpenOrCreate
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
			FileStream file = 
				File.Open(abs_file_path, System.IO.FileMode.Open);
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
	public string created_at;

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

public class MyyUtilities {
	public static byte[] StringToBytes(string data) {
		return System.Text.Encoding.UTF8.GetBytes(data);
	}
		
	public static UnityWebRequest PreparePostRequestTo
	(string endpoint_uri, string content, string content_type)
	{
		UnityWebRequest req = new UnityWebRequest(
			endpoint_uri,
			"POST",
			new DownloadHandlerBuffer(),
			new UploadHandlerRaw(StringToBytes(content))
		);
		req.SetRequestHeader("Content-Type", content_type);
		return req;
	}

	public static UnityWebRequest PrepareJsonPostRequestTo
	(string endpoint_uri, object object_to_convert)
	{
		return PreparePostRequestTo(
			endpoint_uri,
			JsonUtility.ToJson(object_to_convert, false),
			"application/json"
		);
	}

	public static string StringFromBytes(byte[] data) {
		return System.Text.Encoding.UTF8.GetString(data);
	}

	public static string StringFromBytesRange
	(byte[] data, int start, int n_bytes_from_start)
	{
		return System.Text.Encoding.UTF8.GetString(
			data, start, n_bytes_from_start
		);
	}
}

public class MyyUDonServerCredentials
{
	/* The application-specific credentials, irrelevant of the user
	 * 
	 * The app must be known to the server before it can be used by anything.
	 * When the 'application' is registered to the server, it receives specific tokens
	 * that must be used to authenticate the user afterwards.
	 */
	public TootAppAuthCreds app_creds = null;

	/* The per-user per-server credentials.
	 * 
	 * The user must authenticate itself to the server before issuing any command.
	 */
	public TootUserAppCreds user_creds = null;

	public bool IsTheAppRegistered() {
		return app_creds != null;
	}

	public bool IsTheUserAuthenticated() {
		return user_creds != null;
	}

	public bool CanRead() {
		return user_creds.scope.Contains("read");
	}

	public bool CanWrite() {
		return user_creds.scope.Contains("write");
	}

	public bool CanFollow() {
		return user_creds.scope.Contains("follow");
	}
}

public class MyyDownloadHandler : DownloadHandlerScript {

	private bool previous_event_unfinished = false;
	private StringBuilder previous_unfinished_event_data = new StringBuilder(8192);

	public TootEventParser.got_toot_callback toot_callback;

	// Standard scripted download handler - allocates memory on each ReceiveData callback
	public MyyDownloadHandler
	(TootEventParser.got_toot_callback callback): base()
	{
		toot_callback = callback;
	}

	// Pre-allocated scripted download handler
	// reuses the supplied byte array to deliver data.
	// Eliminates memory allocation.

	public MyyDownloadHandler
	(TootEventParser.got_toot_callback callback, byte[] buffer): base(buffer)
	{
		toot_callback = callback;
	}

	// Required by DownloadHandler base class. Called when you address the 'bytes' property.

	protected override byte[] GetData() { return null; }

	// Called once per frame when data has been received from the network.

	protected override bool ReceiveData(byte[] data, int dataLength) {
		
		var data_string = MyyUtilities.StringFromBytesRange(data, 0, dataLength);
		var events_data = TootEventParser.split_events(data_string);

		int already_parsed_events = 0;

		if (previous_event_unfinished) {
			previous_unfinished_event_data.Append(events_data[0]);
			
			if (TootEventParser.at_least_one_complete_event_in(events_data)) {
				// That means that the first event is AT LEAST completed.
				// We expect that first event to complete the previously
				// unfinished event...

				TootEventParser.parse_toot_in_event_data(
					previous_unfinished_event_data.ToString(), toot_callback
				);

				// Start to 'parse_toots_from' 1 after that
				already_parsed_events = 1;

				// All events completed for now
				previous_event_unfinished = false;
				previous_unfinished_event_data.Length = 0;
			}
		}

		// In the case that there's only one unfinished event, nothing will be parsed
		TootEventParser.parse_toots_from(
			events_data, toot_callback, already_parsed_events
		);

		/* Case 1 : 
		 * - The previous event was already unfinished
		 *   In this case
		 *   - if the current data completed this event
		 *     then that event is now processed.
		 *   - if the current data did not complete this event
		 *     then there were only one 'event' string and that
		 *          string was appended to the unfinished event
		 * Case 2 :
		 * - All previous events are completed and there's an unfinished
		 *   event in the downloaded data
		 *   Then this unfinished event data is currently not processed !
		 */ 
		if (TootEventParser.last_event_unfinished(events_data) &&
			!previous_event_unfinished)
		{
			previous_event_unfinished = true;
			previous_unfinished_event_data.Append(events_data[events_data.Length-1]);
		}
			

		return true;
	}

	// Called when all data has been received from the server and delivered via ReceiveData.

	protected override void CompleteContent() {
		Debug.Log("LoggingDownloadHandler :: CompleteContent - DOWNLOAD COMPLETE!");
	}

	// Called when a Content-Length header is received from the server.

	protected override void ReceiveContentLength(int contentLength) {
		Debug.Log(string.Format("LoggingDownloadHandler :: ReceiveContentLength - length {0}", contentLength));
	}
}

public class MyyUDonClient {

	/**
	 * This wonderful moment when you discover that Coroutines are executed
	 * through the main UI thread...
	 * 
	 * Yep... The thing is :
	 * - When doing an operation, you'll have to know the result. If you
	 *   authenticate :
	 *    * you'll want to do something if everything went fine,
	 *    * and do something else if something went wrong !
	 * - Every function will use UnityWebRequest to send HTTP requests.
	 * 
	 * UnityWebRequest#Send() is an AsyncOperation, meaning that the
	 * operation will return and do it's job in background.
	 * 
	 *  
	 * Now, if you do :
	 *   UnityWebRequest request = // preparation code...
	 *   AsyncOperation op = request.Send();
	 *   while (!op.isDone);
	 * 
	 * This will block the Coroutines. If you execute the Coroutine from
	 * the main UI thread, THIS WILL BLOCK THE MAIN UI THREAD !
	 * 
	 * However, if do 'yield return request.Send()', things will go 
	 * smoothly. Now, that only works if the calling function has a 
	 * Coroutines signature...
	 * 
	 * So, in order to coerce Coroutines signatures, performance yielding
	 * and status communication, I opted for a callback system. This fits
	 * well with the Async operations pattern, and is clean.
	 * 
	 * This is how most of the JavaScript libraries work, anyway.
	 */
	public delegate void ClientCallback(bool status, string message);

	private static TootAppAuthInfos app_infos    = new TootAppAuthInfos();
	private MyyUDonServerCredentials credentials = new MyyUDonServerCredentials();
	private string setup_server_domain = "";
	private string api_endpoint_base = "";

	private string endpoint(string name) {
		return api_endpoint_base + name;
	}

	private static bool has_request_succeeded
	(UnityWebRequest request)
	{
		return (!request.isError && request.responseCode < 400);
	}

	public IEnumerator RegisterAppTo
	(string server_domain, ClientCallback callback)
	{
		setup_server_domain = server_domain;
		api_endpoint_base = "https://" + server_domain + "/api/v1/";

		Debug.Log("Registering the application to : " + endpoint("apps"));
		UnityWebRequest app_reg_req = MyyUtilities.PrepareJsonPostRequestTo(
			endpoint("apps"), app_infos
		);

		/* ... Switching to IEnumerator + callbacks might be the best
		 * way after all. Waiting manually for Async Operation to end
		 * is not the way to go, IMHO */
		// FIXME : Pretty sure that it's NOT the way to do it.
		yield return app_reg_req.Send();

		Debug.Log(MyyUtilities.StringFromBytes(app_reg_req.uploadHandler.data));
		Debug.Log(app_reg_req.responseCode);

		bool status = has_request_succeeded(app_reg_req);
		string message = "";

		if (status) {
			message = app_reg_req.downloadHandler.text;
			Debug.Log(message);
			credentials.app_creds = 
				JsonUtility.FromJson<TootAppAuthCreds>(message);
			Debug.Log("Application registered.\n" + credentials.app_creds.ToString());
		}
		else {
			message = app_reg_req.error;
		}

		if (callback != null)
			callback(status, message);
	}

	public IEnumerator LoginTo
	(string server_domain, string login, string password,
	 ClientCallback callback)
	{

		if (!credentials.IsTheAppRegistered()) {
			yield return RegisterAppTo(server_domain, null);
			if (!credentials.IsTheAppRegistered()) {
				Debug.Log("Could not register the application (´・ω・｀)");
				yield return null;
			}
		}

		// TODO : The scope MUST be user defined
		string app_user_auth_string = string.Format(
			"client_id={0}&client_secret={1}" +
			"&grant_type=password&scope=read write follow&username={2}&password={3}",
			credentials.app_creds.client_id,
			credentials.app_creds.client_secret,
			login, password
		);

		UnityWebRequest user_auth_req = MyyUtilities.PreparePostRequestTo(
			"https://" + server_domain + "/oauth/token",
			app_user_auth_string,
			"application/x-www-form-urlencoded"
		);

		yield return user_auth_req.Send();

		bool status = has_request_succeeded(user_auth_req);
		string message = user_auth_req.downloadHandler.text;

		Debug.Log(message);

		if (status) {
			Debug.Log(message);

			credentials.user_creds = 
				JsonUtility.FromJson<TootUserAppCreds>(message);
			Debug.Log(" Authentication OK ! We're good to go !");
		}
		else {
			Debug.Log("Authentication failed (´・ω・｀)・・・");
			Debug.Log(message);
		}

		if (callback != null)
			callback(status, message);

	}


	/* BLOCKING CALL */
	public IEnumerator api_write
	(string endpoint_uri, object toot_object_to_send,
     ClientCallback callback)
	{
		UnityWebRequest mastodon_api_req = MyyUtilities.PrepareJsonPostRequestTo(
			endpoint_uri, toot_object_to_send
		);
		// TODO : No need to recreate the SAME header EVERY time.
		mastodon_api_req.SetRequestHeader(
			"Authorization",
			String.Format("Bearer {0}", credentials.user_creds.access_token)
		);

		yield return mastodon_api_req.Send();

		bool status = 
			(mastodon_api_req.isError || mastodon_api_req.responseCode >= 400);
		string message =
			mastodon_api_req.downloadHandler.text;

		Debug.LogFormat(
			"Was able to send {0} ? {1} !", toot_object_to_send.GetType(), status
		);
		Debug.LogFormat(
			"Response : {0}\n", message
		);

		if (callback != null)
			callback(status, message);
	}

	public IEnumerator stream(TootEventParser.got_toot_callback callback) {
		UnityWebRequest mastodon_api_req = new UnityWebRequest(
			endpoint("streaming/public/local"),
			"GET"
		);

		mastodon_api_req.downloadHandler =
			new MyyDownloadHandler(callback, new byte[8192]);
		mastodon_api_req.SetRequestHeader(
			"Authorization",
			String.Format("Bearer {0}", credentials.user_creds.access_token)
		);

		yield return mastodon_api_req.Send();
	}

	/// <summary>
	/// Toot the specified message and call callback.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="callback">Callback.</param>

	public IEnumerator Toot(string message, ClientCallback callback)
	{
		TootStatusToSend toot = new TootStatusToSend(message);
		yield return api_write(endpoint("statuses"), toot, callback);
	}

}

public class TooTooT : MonoBehaviour {

	public Canvas login_pane;
	public Canvas chat_pane;
	public InputField server;
	public InputField login;
	public InputField password;

	public InputField typed_toot;
	public Button send_toot_button;
	public Text displayed_toots;

	private MyyUDonClient udon_client;

	private void start_chatting()
	{
		login_pane.gameObject.SetActive(false);
		chat_pane.gameObject.SetActive(true);
		StartCoroutine(udon_client.stream(received_toot_from_stream_cb));
	}

	protected void on_login_cb
	(bool logged_in, string error_message)
	{
		if (logged_in) start_chatting();
	}

	protected void on_toot_sent_cb
	(bool sent_correctly, string error_message)
	{
		if (sent_correctly)
			typed_toot.text = "";
		send_toot_button.enabled = true;
	}

	protected void received_toot_from_stream_cb
	(TootStatus toot)
	{
		displayed_toots.text += (toot.account.display_name + ": " + toot.content + "\n");
	}

	// Use this for initialization
	void Start () {
		udon_client = new MyyUDonClient();

	}

	public void TryToLogin() {
		
		Debug.LogFormat(
			"Server   : {0}\n" +
			"Login    : {1}\n" +
			"Password : {2}\n",
			server.text,
			login.text,
			password.text != null && password.text != ""
		);
		StartCoroutine(
			udon_client.LoginTo(
				server.text, login.text, password.text, on_login_cb
			)
		);
	}

	public void SendToot() {
		send_toot_button.enabled = false;
		StartCoroutine(
			udon_client.Toot(typed_toot.text, on_toot_sent_cb)
		);
	}
}


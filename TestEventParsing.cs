using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.VersionControl;
using System.Text;

/// <summary>
/// Tests various parts of the UDon Events Parser
/// </summary>
public class TestEventParsing : MonoBehaviour {

	// Turns out that using Unity related Utility functions like JsonUtility
	// in Unit tests is a big no-no. Trying to do that will result in
	// various SecurityExceptions...
	// So... Here we are ! Launching tests inside a MonoBehaviour !

	// Use this for initialization
	void Start () {
		test_events_and_checks();
		test_last_event_state_check();
		test_parse_statuses();
		test_reset_string_builder();
	}

	void test_reset_string_builder() {
		Debug.Log("Testing StringBuilders");
		StringBuilder a = new StringBuilder(8192);
		a.Append("a");
		Assert.AreEqual<string>("a", a.ToString());
		Assert.AreEqual<int>(1, a.Length);
		a.Length = 0;
		Assert.AreEqual<string>("", a.ToString());
		Assert.AreEqual<int>(0, a.Length);
		a.Append("bcd");
		Assert.AreEqual<string>("bcd", a.ToString());
		Assert.AreEqual<int>(3, a.Length);
	}

	static readonly string good_event = 
		"event: update\n" +
		"data: {" +
		"\"id\":29443," +
		"\"created_at\":\"2017-04-14T18:05:28.000Z\"," +
		"\"in_reply_to_id\":null," +
		"\"in_reply_to_account_id\":null," +
		"\"sensitive\":false," +
		"\"spoiler_text\":\"\"," +
		"\"visibility\":\"public\"," +
		"\"application\":null," +
		"\"account\":{" +
		"\"id\":1217," +
		"\"username\":\"tester\"," +
		"\"acct\":\"tester@mastodon.social\"," +
		"\"display_name\":\"tester\"," +
		"\"locked\":false," +
		"\"created_at\":\"2017-04-07T19:20:26.126Z\"," +
		"\"followers_count\":1," +
		"\"following_count\":1," +
		"\"statuses_count\":201," +
		"\"note\":\"test data\"," +
		"\"url\":\"https://mastodon.social/@tester\"," +
		"\"avatar\":\"https://botsinspace.s3.amazonaws.com/accounts/avatars/000/001/123/original/test.png?1249392826\"," +
		"\"avatar_static\":\"https://botsinspace.s3.amazonaws.com/accounts/avatars/000/001/123/original/test.png\"," +
		"\"header\":\"/headers/original/missing.png\"," +
		"\"header_static\":\"/headers/original/missing.png\"" +
		"}," +
		"\"media_attachments\":[]," +
		"\"mentions\":[]," +
		"\"tags\":[]," +
		"\"uri\":\"tag:mastodon.social,2017-04-14:objectId=2548011:objectType=Status\"," +
		"\"content\":\"<p>here is a toot</p>\"," +
		"\"url\":\"https://mastodon.social/users/tester/updates/285376\"," +
		"\"reblogs_count\":0," +
		"\"favourites_count\":0," +
		"\"reblog\":null}\n\n";
	static readonly string good_event_single_line_term = "event: update\ndata: {\"id\":29443,\"created_at\":\"2017-04-14T18:05:28.000Z\",\"in_reply_to_id\":null,\"in_reply_to_account_id\":null,\"sensitive\":false,\"spoiler_text\":\"\",\"visibility\":\"public\",\"application\":null,\"account\":{\"id\":1217,\"username\":\"tester\",\"acct\":\"tester@mastodon.social\",\"display_name\":\"tester\",\"locked\":false,\"created_at\":\"2017-04-07T19:20:26.126Z\",\"followers_count\":1,\"following_count\":1,\"statuses_count\":201,\"note\":\"test data\",\"url\":\"https://mastodon.social/@tester\",\"avatar\":\"https://botsinspace.s3.amazonaws.com/accounts/avatars/000/001/123/original/test.png?1249392826\",\"avatar_static\":\"https://botsinspace.s3.amazonaws.com/accounts/avatars/000/001/123/original/test.png\",\"header\":\"/headers/original/missing.png\",\"header_static\":\"/headers/original/missing.png\"},\"media_attachments\":[],\"mentions\":[],\"tags\":[],\"uri\":\"tag:mastodon.social,2017-04-14:objectId=2548011:objectType=Status\",\"content\":\"<p>here is a toot</p>\",\"url\":\"https://mastodon.social/users/tester/updates/285376\",\"reblogs_count\":0,\"favourites_count\":0,\"reblog\":null}\n";
	static readonly string good_events = good_event + good_event;

	static readonly string good_delete_event = "event: delete\ndata: {\"id\": 78945}\n\n";
	static readonly string thumped_good_event = ":thump\n" + good_event_single_line_term;

	static readonly string unfinished_event_hdr_only = "event: update\n";
	static readonly string unfinished_event = "event:update\ndata: {\"id\": 43227";
	static readonly string good_event_and_unfinished_event = good_event + unfinished_event_hdr_only;

	// UnityAssertions can't compare Arrays...
	private static void AreArraysEqual<T>(T[] expected, T[] got)
	{
		Assert.IsTrue(got != null);
		Assert.IsTrue(got.Length == expected.Length);
		for (var i = 0; i < expected.Length; i++) {
			Assert.AreEqual(expected[i], got[i]);
		}
	}

	public void good_event_check_callback(TootStatus status) {
		// Generic assert methods... It's a thing now...
		callback_test_value = 1;
		Assert.AreEqual<long>(29443, status.id);
		Assert.AreEqual<string>("2017-04-14T18:05:28.000Z", status.created_at);
		Assert.IsTrue(status.in_reply_to_id == null);
		Assert.IsTrue(status.in_reply_to_account_id == null);

		// IsTrue cannot check bool? types ! And confuse false with null ! Yay !
		// WONTPASS due to fucked deserialization
		Assert.IsTrue(status.sensitive == false);
		Assert.AreEqual<int>(0, status.spoiler_text.Length);
		Assert.AreEqual<string>("public", status.visibility);

		// Turns out that fucking up JSON Deserialization by transforming null
		// values into empty intialized objects  is a "feature" with JsonUtility !
		// And a DOCUMENTED ONE !
		// https://blogs.unity3d.com/jp/2014/06/24/serialization-in-unity/
		// "No support for null for custom classes"
		// Brilliant !
		// WONTPASS due to fucked deserialization
		Assert.IsNull<TootApplication>(status.application);
		Debug.Log(status.application);
		Assert.AreEqual<long>(1217, status.account.id);
		Assert.AreEqual<string>("tester", status.account.username);
		Assert.AreEqual<string>("tester@mastodon.social", status.account.acct);
		Assert.AreEqual<string>("tester", status.account.display_name);
		Assert.AreEqual<string>("2017-04-07T19:20:26.126Z", status.account.created_at);
		Assert.AreEqual<long>(1, status.account.followers_count);
		Assert.AreEqual<long>(1, status.account.following_count);
		Assert.AreEqual<long>(201, status.account.statuses_count);
		Assert.AreEqual<string>("test data", status.account.note);
		Assert.AreEqual<string>("https://mastodon.social/@tester", status.account.url);
		Assert.AreEqual<string>("https://botsinspace.s3.amazonaws.com/accounts/avatars/000/001/123/original/test.png", status.account.avatar_static);
		Assert.AreEqual<string>("https://botsinspace.s3.amazonaws.com/accounts/avatars/000/001/123/original/test.png?1249392826", status.account.avatar);
		Assert.AreEqual<string>("/headers/original/missing.png", status.account.header);
		Assert.AreEqual<string>("/headers/original/missing.png", status.account.header_static);

		// UnityAssertions can't compare Arrays by default...
		AreArraysEqual<TootAttachment>(new TootAttachment[] {}, status.media_attachments);
		AreArraysEqual<TootMention>(new TootMention[] {}, status.mentions);
		AreArraysEqual<TootTag>(new TootTag[] {}, status.tags);

		Assert.AreEqual<string>("tag:mastodon.social,2017-04-14:objectId=2548011:objectType=Status", status.uri);
		Assert.AreEqual<string>("<p>here is a toot</p>", status.content);
		Assert.AreEqual<string>("https://mastodon.social/users/tester/updates/285376", status.url);
		Assert.AreEqual<long>(0, status.reblogs_count);
		Assert.AreEqual<long>(0, status.favourites_count);

		// WONTPASS due to fucked deserialization
		Assert.IsNull<string>(status.reblog);

	}

	public void test_events_and_checks() {
		var splitted_good_event = TootEventParser.split_events(good_event);
		Assert.IsNotNull(splitted_good_event);
		Assert.AreEqual<int>(2, splitted_good_event.Length);
		Assert.IsTrue(TootEventParser.at_least_one_complete_event_in(splitted_good_event));

		var bad_events_not_fully_terminated =
			TootEventParser.split_events(good_event_single_line_term);
		Assert.IsNotNull(bad_events_not_fully_terminated);
		Assert.AreEqual<int>(1, bad_events_not_fully_terminated.Length);
		Assert.IsFalse(TootEventParser.at_least_one_complete_event_in(bad_events_not_fully_terminated));

		var splitted_good_events = TootEventParser.split_events(good_events);
		Assert.IsNotNull(splitted_good_events);
		Assert.AreEqual<int>(3, splitted_good_events.Length);
		Assert.IsTrue(TootEventParser.at_least_one_complete_event_in(splitted_good_events));
	}
		
	public void test_last_event_state_check() {
		var splitted_good_event = TootEventParser.split_events(good_event);
		var bad_events_not_fully_terminated = TootEventParser.split_events(good_event_single_line_term);
		var splitted_good_events = TootEventParser.split_events(good_events);
		var bad_event_unfinished = TootEventParser.split_events(unfinished_event_hdr_only);
		var bad_event_other_unfinished = TootEventParser.split_events(unfinished_event);
		var good_event_and_unfinished = TootEventParser.split_events(good_event_and_unfinished_event);

		Assert.IsFalse(TootEventParser.last_event_unfinished(splitted_good_event));
		Assert.IsFalse(TootEventParser.last_event_unfinished(splitted_good_events));

		Assert.IsTrue(TootEventParser.last_event_unfinished(bad_events_not_fully_terminated));
		Assert.IsTrue(TootEventParser.last_event_unfinished(bad_event_unfinished));
		Assert.IsTrue(TootEventParser.last_event_unfinished(bad_event_other_unfinished));
		Assert.IsTrue(TootEventParser.last_event_unfinished(good_event_and_unfinished));
	}

	uint callback_test_value = 0;

	public void test_parse_statuses() {
		string valid_complete_event = TootEventParser.split_events(good_event)[0];

		bool got_toot = TootEventParser.parse_toot_in_event_data(
			valid_complete_event, good_event_check_callback
		);

		Assert.IsTrue(got_toot);
		Assert.AreEqual<uint>(1, callback_test_value);

		// Test with a delete event
		callback_test_value = 0;
		Assert.AreEqual<uint>(0, callback_test_value);

		valid_complete_event = TootEventParser.split_events(good_delete_event)[0];
		got_toot = TootEventParser.parse_toot_in_event_data(
			valid_complete_event, good_event_check_callback
		);

		Assert.IsFalse(got_toot);
		Assert.AreEqual<uint>(0, callback_test_value);

		// Test with a thumped valid update event
		callback_test_value = 0;
		Assert.AreEqual<uint>(0, callback_test_value);

		valid_complete_event = TootEventParser.split_events(thumped_good_event)[0];
		got_toot = TootEventParser.parse_toot_in_event_data(
			valid_complete_event, good_event_check_callback
		);

		Assert.IsTrue(got_toot);
		Assert.AreEqual<uint>(1, callback_test_value);
	}

	public void test_event_type() {

	}
}

public class TootEventParser {

	public delegate void got_toot_callback(TootStatus status);

	static readonly string event_update_header =
		"event: update\ndata: {";
	static readonly int event_update_json_start_offset =
		event_update_header.Length - 1;


	public static string[] split_events
	(string event_data)
	{
		return Regex.Split(event_data, "\n\n");
	}

	public static bool at_least_one_complete_event_in(string[] events)
	{
		// split_events(string) will split the data and return
		// *at least* 2 strings if the string contain
		// *at least* 1 full event.
		return events.Length > 1;
	}

	public static bool last_event_unfinished(string[] events)
	{
		return events[events.Length-1] != "";
	}

	public static int update_event_start(string event_data) {
		int event_start = event_data.IndexOf(event_update_header);
		if (event_start >= 0)
			event_start += event_update_json_start_offset;

		return event_start;
	}


	public static bool parse_toot_in_event_data
	(string event_data, got_toot_callback toot_callback)
	{
		int event_json_start = update_event_start(event_data);
		bool parsed_new_status = false;

		if (event_json_start > 0) {
			Console.Error.WriteLine(event_data.Substring(event_json_start));
			try {
				var status = JsonUtility.FromJson<TootStatus>(
					event_data.Substring(event_json_start)
				);
				toot_callback(status);
				parsed_new_status = true;
			}
			catch (Exception e) {
				Debug.LogException(e);
			}
		}

		Debug.LogFormat("Had toot ? {0}\n{1}\n", parsed_new_status, event_data);

		return parsed_new_status;
	}

	public static void parse_toots_from
	(string[] events_data, got_toot_callback toot_callback, int from_e)
	{
		int last_good_e = events_data.Length - 2;

		for (int i = from_e; i < last_good_e; i++)
			parse_toot_in_event_data(events_data[i], toot_callback);
	}
}

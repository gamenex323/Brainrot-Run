using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using Steamworks;
using UnityEngine;

public class PlayFabManager : MonoBehaviour
{
	public SteamController steamController;

	public static bool loggedIn;

	public static bool userLeaderboardInfoRetrieved;

	public static string userPlayFabId;

	public static string userDisplayName;

	public static int userPosition;

	public static int userScore;

	public static string userRacerData;

	public static string userRacerName;

	public static string userDate;

	public static string userTotalScore;

	public static bool leaderboardSent;

	public static bool leaderboardLoaded;

	public static string leaderboardString;

	public static bool thisUserInfoRetrieved;

	public static string thisUserPlayFabId;

	public static string thisUserDisplayName;

	public static int thisUserPosition;

	public static int thisUserScore;

	public static bool loginError;

	public static bool leaderboardGetError;

	public static bool leaderboardSendFailure;

	private static bool gettingUser;

	public static int selectedRaceEvent;

	private static PlayFabManager instance;

	private static HAuthTicket hTicket;

	public static void login()
	{
		if (SteamManager.Initialized)
		{
			loginError = false;
			PlayFabClientAPI.LoginWithSteam(new LoginWithSteamRequest
			{
				CreateAccount = true,
				SteamTicket = getSteamAuthTicket()
			}, onLoginSuccess, onLoginError);
		}
	}

	private static void onLoginSuccess(LoginResult result)
	{
		thisUserPlayFabId = result.PlayFabId;
		loggedIn = true;
		Debug.Log("login/account create success");
	}

	public static string getSteamAuthTicket()
	{
		byte[] array = new byte[1024];
		hTicket = SteamUser.GetAuthSessionTicket(array, array.Length, out var pcbTicket);
		Array.Resize(ref array, (int)pcbTicket);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.AppendFormat("{0:x2}", b);
		}
		return stringBuilder.ToString();
	}

	public static void logout()
	{
		SteamUser.CancelAuthTicket(hTicket);
		loggedIn = false;
	}

	public static void unlinkAccount()
	{
		PlayFabClientAPI.UnlinkCustomID(new UnlinkCustomIDRequest
		{
			CustomId = thisUserPlayFabId
		}, onUnlinkAccountSuccess, onUnlinkAccountError);
	}

	private static void onUnlinkAccountSuccess(UnlinkCustomIDResult result)
	{
		Debug.Log("account unlinked");
	}

	private static void onUnlinkAccountError(PlayFabError error)
	{
		Debug.Log("error unlinking account");
		error.GenerateErrorReport();
	}

	public static void setUserDisplayName(string username)
	{
		if (!loggedIn)
		{
			return;
		}
		PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
		{
			DisplayName = username
		}, delegate(UpdateUserTitleDisplayNameResult result)
		{
			string text = result.DisplayName;
			if (text.Length < 3)
			{
				text.PadRight(3 - text.Length);
			}
			else if (text.Length > 25)
			{
				text = text.Substring(0, 25);
			}
			thisUserDisplayName = text;
			Debug.Log("The player's display name is now: " + text);
		}, delegate
		{
			Debug.LogError("Error setting display name");
		});
	}

	public static void sendLeaderboard(string packagedData)
	{
		leaderboardSendFailure = false;
		leaderboardSent = false;
		string[] array = packagedData.Split('#');
		int num = int.Parse(array[0]);
		float num2 = float.Parse(array[1]);
		string value = array[2];
		string value2 = array[3];
		int num3 = int.Parse(array[4]);
		int num4 = int.Parse(array[5]);
		if (loggedIn)
		{
			selectedRaceEvent = num;
			string text = "";
			if (num == RaceManager.RACE_EVENT_100M)
			{
				text = "100m";
			}
			else if (num == RaceManager.RACE_EVENT_200M)
			{
				text = "200m";
			}
			else if (num == RaceManager.RACE_EVENT_400M)
			{
				text = "400m";
			}
			else if (num == RaceManager.RACE_EVENT_60M)
			{
				text = "60m";
			}
			List<StatisticUpdate> list = new List<StatisticUpdate>
			{
				new StatisticUpdate
				{
					StatisticName = text,
					Value = (int)(num2 * -10000f)
				}
			};
			if (num3 != -1)
			{
				list.Add(new StatisticUpdate
				{
					StatisticName = "Total Score",
					Value = num3
				});
			}
			if (num4 != -1)
			{
				list.Add(new StatisticUpdate
				{
					StatisticName = "Total Score (Best Racer)",
					Value = num4
				});
			}
			PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
			{
				Statistics = list
			}, onLeaderboardSend, onLeaderboardSendError);
			Dictionary<string, string> dictionary = new Dictionary<string, string>
			{
				{
					"RacerName_" + text,
					value
				},
				{
					"RacerData_" + text,
					value2
				},
				{
					"Date_" + text,
					DateGetter.getDate()
				}
			};
			if (num3 != -1)
			{
				dictionary.Add("UserTotalScore", num3.ToString());
				dictionary.Add("Date_UserTotalScore", DateGetter.getDate());
			}
			if (num4 != -1)
			{
				dictionary.Add("RacerName_Total Score (Best Racer)", value);
				dictionary.Add("Date_Total Score (Best Racer)", DateGetter.getDate());
			}
			PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
			{
				Data = dictionary,
				Permission = UserDataPermission.Public
			}, onRacerDataSend, onUpdateUserDataError);
		}
	}

	public static void getLeaderboard(int raceEvent, bool friendsOnly, int startPos, int maxEntries)
	{
		if (!loggedIn)
		{
			return;
		}
		selectedRaceEvent = raceEvent;
		string statisticName = "";
		if (raceEvent == RaceManager.RACE_EVENT_100M)
		{
			statisticName = "100m";
		}
		else if (raceEvent == RaceManager.RACE_EVENT_200M)
		{
			statisticName = "200m";
		}
		else if (raceEvent == RaceManager.RACE_EVENT_400M)
		{
			statisticName = "400m";
		}
		else if (raceEvent == RaceManager.RACE_EVENT_60M)
		{
			statisticName = "60m";
		}
		else
		{
			switch (raceEvent)
			{
			case 4:
				statisticName = "Total Score";
				break;
			case 5:
				statisticName = "Total Score (Best Racer)";
				break;
			}
		}
		if (friendsOnly)
		{
			PlayFabClientAPI.GetFriendLeaderboard(new GetFriendLeaderboardRequest
			{
				StatisticName = statisticName,
				StartPosition = startPos,
				MaxResultsCount = maxEntries
			}, instance.onLeaderboardGet, onLeaderboardGetError);
		}
		else
		{
			PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
			{
				StatisticName = statisticName,
				StartPosition = startPos,
				MaxResultsCount = maxEntries
			}, instance.onLeaderboardGet, onLeaderboardGetError);
		}
	}

	private void onLeaderboardGet(GetLeaderboardResult result)
	{
		StartCoroutine(generateLeaderboardResultString(result));
	}

	private IEnumerator generateLeaderboardResultString(GetLeaderboardResult result)
	{
		string resultString = "";
		string data_racerName = "";
		string data_date = "";
		foreach (PlayerLeaderboardEntry entry in result.Leaderboard)
		{
			bool dataRetrieved = false;
			string leaderboard_event = "";
			if (selectedRaceEvent == RaceManager.RACE_EVENT_100M)
			{
				leaderboard_event = "100m";
			}
			else if (selectedRaceEvent == RaceManager.RACE_EVENT_200M)
			{
				leaderboard_event = "200m";
			}
			else if (selectedRaceEvent == RaceManager.RACE_EVENT_400M)
			{
				leaderboard_event = "400m";
			}
			else if (selectedRaceEvent == RaceManager.RACE_EVENT_60M)
			{
				leaderboard_event = "60m";
			}
			else if (selectedRaceEvent == 4)
			{
				leaderboard_event = "Total Score";
			}
			else if (selectedRaceEvent == 5)
			{
				leaderboard_event = "Total Score (Best Racer)";
			}
			PlayFabClientAPI.GetUserData(new GetUserDataRequest
			{
				PlayFabId = entry.PlayFabId,
				Keys = null
			}, delegate(GetUserDataResult titleDataResult)
			{
				if (titleDataResult.Data != null)
				{
					try
					{
						if (selectedRaceEvent <= 3)
						{
							data_racerName = titleDataResult.Data["RacerName_" + leaderboard_event].Value;
							data_date = titleDataResult.Data["Date_" + leaderboard_event].Value;
						}
						else if (selectedRaceEvent == 5)
						{
							data_racerName = titleDataResult.Data["RacerName_" + leaderboard_event].Value;
							data_date = titleDataResult.Data["Date_" + leaderboard_event].Value;
						}
						else
						{
							List<string> list = new List<string>();
							try
							{
								string value = titleDataResult.Data["RacerName_100m"].Value;
								list.Add(value + " <i>(100m)</i>");
							}
							catch (KeyNotFoundException)
							{
								list.RemoveAt(list.Count - 1);
							}
							try
							{
								string value = titleDataResult.Data["RacerName_200m"].Value;
								list.Add(value + " <i>(200m)</i>");
							}
							catch (KeyNotFoundException)
							{
								list.RemoveAt(list.Count - 1);
							}
							try
							{
								string value = titleDataResult.Data["RacerName_400m"].Value;
								list.Add(value + " <i>(400m)</i>");
							}
							catch (KeyNotFoundException)
							{
								list.RemoveAt(list.Count - 1);
							}
							try
							{
								string value = titleDataResult.Data["RacerName_60m"].Value;
								list.Add(value + " <i>(60m)</i>");
							}
							catch (KeyNotFoundException)
							{
								list.RemoveAt(list.Count - 1);
							}
							data_racerName = string.Join(", ", list);
							data_date = titleDataResult.Data["Date_UserTotalScore"].Value;
						}
					}
					catch (Exception ex5)
					{
						Debug.Log(ex5.ToString());
					}
					dataRetrieved = true;
				}
			}, delegate(PlayFabError error)
			{
				Debug.Log("Error retrieving RacerData:");
				Debug.Log(error.GenerateErrorReport());
			});
			yield return new WaitUntil(() => dataRetrieved);
			resultString = resultString + entry.PlayFabId + "*" + entry.Position + "*" + entry.DisplayName + "*" + entry.StatValue;
			resultString = resultString + "*" + data_racerName + "*" + data_date;
			resultString += ":";
		}
		leaderboardString = resultString;
		leaderboardLoaded = true;
		leaderboardGetError = false;
	}

	public static void getLeaderboardEntryInfo(int raceEvent, string pfid)
	{
		if (!loggedIn)
		{
			return;
		}
		selectedRaceEvent = raceEvent;
		string statisticName = "";
		if (raceEvent == RaceManager.RACE_EVENT_100M)
		{
			statisticName = "100m";
		}
		else if (raceEvent == RaceManager.RACE_EVENT_200M)
		{
			statisticName = "200m";
		}
		else if (raceEvent == RaceManager.RACE_EVENT_400M)
		{
			statisticName = "400m";
		}
		else if (raceEvent == RaceManager.RACE_EVENT_60M)
		{
			statisticName = "60m";
		}
		else
		{
			switch (raceEvent)
			{
			case 4:
				statisticName = "Total Score";
				break;
			case 5:
				statisticName = "Total Score (Best Racer)";
				break;
			}
		}
		PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
		{
			PlayFabId = pfid,
			StatisticName = statisticName,
			MaxResultsCount = 1
		}, onLeaderboardEntryGet, onLeaderboardGetError);
	}

	private static void onLeaderboardEntryGet(GetLeaderboardAroundPlayerResult result)
	{
		string leaderboard_event = "";
		if (selectedRaceEvent == RaceManager.RACE_EVENT_100M)
		{
			leaderboard_event = "100m";
		}
		else if (selectedRaceEvent == RaceManager.RACE_EVENT_200M)
		{
			leaderboard_event = "200m";
		}
		else if (selectedRaceEvent == RaceManager.RACE_EVENT_400M)
		{
			leaderboard_event = "400m";
		}
		else if (selectedRaceEvent == RaceManager.RACE_EVENT_60M)
		{
			leaderboard_event = "60m";
		}
		else if (selectedRaceEvent == 4)
		{
			leaderboard_event = "Total Score";
		}
		else if (selectedRaceEvent == 5)
		{
			leaderboard_event = "Total Score (Best Racer)";
		}
		PlayerLeaderboardEntry playerLeaderboardEntry = result.Leaderboard[0];
		userPlayFabId = playerLeaderboardEntry.PlayFabId;
		userDisplayName = playerLeaderboardEntry.DisplayName;
		userPosition = playerLeaderboardEntry.Position;
		userScore = playerLeaderboardEntry.StatValue;
		PlayFabClientAPI.GetUserData(new GetUserDataRequest
		{
			PlayFabId = playerLeaderboardEntry.PlayFabId,
			Keys = null
		}, delegate(GetUserDataResult titleDataResult)
		{
			if (titleDataResult.Data == null)
			{
				Debug.Log("No RacerData found");
			}
			try
			{
				if (selectedRaceEvent <= 3)
				{
					userRacerData = titleDataResult.Data["RacerData_" + leaderboard_event].Value;
					userRacerName = titleDataResult.Data["RacerName_" + leaderboard_event].Value;
					userDate = titleDataResult.Data["Date_" + leaderboard_event].Value;
				}
				else if (selectedRaceEvent == 4)
				{
					userRacerData = "NA";
					userRacerName = "NA";
					userDate = titleDataResult.Data["Date_UserTotalScore"].Value;
				}
				else
				{
					userRacerData = "NA";
					userRacerName = titleDataResult.Data["RacerName_" + leaderboard_event].Value;
					userDate = titleDataResult.Data["Date_" + leaderboard_event].Value;
				}
			}
			catch (Exception ex)
			{
				userRacerData = "Not found";
				userRacerName = "Not found";
				userDate = "Not found";
				userTotalScore = "Not found";
				Debug.Log(ex.ToString());
			}
			userLeaderboardInfoRetrieved = true;
		}, delegate(PlayFabError error)
		{
			Debug.Log("Error retrieving RacerData:");
			Debug.Log(error.GenerateErrorReport());
		});
	}

	public static void getThisUserLeaderboardInfo()
	{
		if (loggedIn)
		{
			string statisticName = "";
			if (selectedRaceEvent == RaceManager.RACE_EVENT_100M)
			{
				statisticName = "100m";
			}
			else if (selectedRaceEvent == RaceManager.RACE_EVENT_200M)
			{
				statisticName = "200m";
			}
			else if (selectedRaceEvent == RaceManager.RACE_EVENT_400M)
			{
				statisticName = "400m";
			}
			else if (selectedRaceEvent == RaceManager.RACE_EVENT_60M)
			{
				statisticName = "60m";
			}
			else if (selectedRaceEvent == 4)
			{
				statisticName = "Total Score";
			}
			else if (selectedRaceEvent == 5)
			{
				statisticName = "Total Score (Best Racer)";
			}
			PlayFabClientAPI.GetLeaderboardAroundPlayer(new GetLeaderboardAroundPlayerRequest
			{
				PlayFabId = thisUserPlayFabId,
				StatisticName = statisticName,
				MaxResultsCount = 1
			}, onThisUserLeaderboardInfoGet, onLeaderboardGetError);
		}
	}

	private static void onThisUserLeaderboardInfoGet(GetLeaderboardAroundPlayerResult result)
	{
		if (selectedRaceEvent != RaceManager.RACE_EVENT_100M && selectedRaceEvent != RaceManager.RACE_EVENT_200M && selectedRaceEvent != RaceManager.RACE_EVENT_400M && selectedRaceEvent != RaceManager.RACE_EVENT_60M && selectedRaceEvent != 4)
		{
			_ = selectedRaceEvent;
			_ = 4;
		}
		PlayerLeaderboardEntry playerLeaderboardEntry = result.Leaderboard[0];
		thisUserDisplayName = playerLeaderboardEntry.DisplayName;
		thisUserPosition = playerLeaderboardEntry.Position;
		thisUserScore = playerLeaderboardEntry.StatValue;
		thisUserInfoRetrieved = true;
	}

	public static void getWorldRecordInfo(int raceEvent)
	{
		if (loggedIn)
		{
			selectedRaceEvent = raceEvent;
			instance.StartCoroutine(retrieveWR(raceEvent));
		}
	}

	private static IEnumerator retrieveWR(int raceEvent)
	{
		leaderboardLoaded = false;
		getLeaderboard(raceEvent, friendsOnly: false, 0, 1);
		yield return new WaitUntil(() => leaderboardLoaded);
		string pfid = leaderboardString.Split('*')[0];
		userLeaderboardInfoRetrieved = false;
		getLeaderboardEntryInfo(raceEvent, pfid);
		yield return new WaitUntil(() => userLeaderboardInfoRetrieved);
	}

	private static void onLeaderboardSend(UpdatePlayerStatisticsResult result)
	{
		leaderboardSent = true;
	}

	private static void onRacerDataSend(UpdateUserDataResult result)
	{
	}

	private static void onLoginError(PlayFabError error)
	{
		loggedIn = false;
		loginError = true;
		Debug.Log("Error logging in/creating account");
		Debug.Log(error.GenerateErrorReport());
	}

	private static void onLeaderboardGetError(PlayFabError error)
	{
		loggedIn = false;
		leaderboardGetError = true;
		Debug.Log("Error getting leaderboard");
		Debug.Log(error.GenerateErrorReport());
	}

	private static void onLeaderboardSendError(PlayFabError error)
	{
		loggedIn = false;
		leaderboardSendFailure = true;
		Debug.Log("Error sending time to leaderboard");
		Debug.Log(error.GenerateErrorReport());
	}

	private static void onUpdateUserDataError(PlayFabError error)
	{
		loggedIn = false;
		Debug.Log("Error sending user data to leaderboard");
		Debug.Log(error.GenerateErrorReport());
	}

	private static void onLeaderboardEntryGetError(PlayFabError error)
	{
		loggedIn = false;
		Debug.Log("Error getting leaderboard entry");
		Debug.Log(error.GenerateErrorReport());
	}

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
	}
}

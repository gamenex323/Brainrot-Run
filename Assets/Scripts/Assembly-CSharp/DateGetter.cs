using System;
using Steamworks;
using UnityEngine;

public class DateGetter : MonoBehaviour
{
	public static string getDate()
	{
		DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds((int)SteamUtils.GetServerRealTime()).DateTime;
		return string.Join("/", dateTime.Month, dateTime.Day, dateTime.Year);
	}
}

using Steamworks;
using UnityEngine;

public class SteamController : MonoBehaviour
{
	public CSteamID getThisUserSteamID()
	{
		if (!SteamManager.Initialized)
		{
			return CSteamID.Nil;
		}
		return SteamUser.GetSteamID();
	}

	public string getThisUserSteamName()
	{
		if (!SteamManager.Initialized)
		{
			return "";
		}
		return SteamFriends.GetPersonaName();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}

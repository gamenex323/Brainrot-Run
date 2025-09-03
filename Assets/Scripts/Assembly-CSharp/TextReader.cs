using System.IO;
using UnityEngine;

public class TextReader : MonoBehaviour
{
	private static StreamReader reader;

	private void Start()
	{
	}

    //public static string getRandomName()
    //{
    //	string text = "";
    //	reader = new StreamReader(Path.Combine(Application.streamingAssetsPath, "txt_first_names.txt"));
    //	int num = Random.Range(0, 2943);
    //	for (int i = 0; i < num; i++)
    //	{
    //		reader.ReadLine();
    //	}
    //	text += reader.ReadLine();
    //	reader = new StreamReader(Path.Combine(Application.streamingAssetsPath, "txt_last_names.txt"));
    //	num = Random.Range(0, 2000);
    //	for (int j = 0; j < num; j++)
    //	{
    //		reader.ReadLine();
    //	}
    //	return text + " " + reader.ReadLine();
    //}

    public static string getRandomName()
    {
        // Load text assets from Resources folder (no extension needed)
        TextAsset firstNamesFile = Resources.Load<TextAsset>("txt_first_names");
        TextAsset lastNamesFile = Resources.Load<TextAsset>("txt_last_names");

        // Split into arrays
        string[] firstNames = firstNamesFile.text.Split('\n');
        string[] lastNames = lastNamesFile.text.Split('\n');

        // Pick random entries
        string firstName = firstNames[Random.Range(0, firstNames.Length)].Trim();
        string lastName = lastNames[Random.Range(0, lastNames.Length)].Trim();

        return firstName + " " + lastName;
    }
}

using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
	public static int calculateScore(float[] times)
	{
		float[] array = new float[4] { 10.7f, 17f, 35.5f, 79f };
		float[] array2 = new float[4] { 68.6f, 24.63f, 5.08f, 1.021f };
		int num = 0;
		for (int i = 0; i < times.Length; i++)
		{
			float num2 = times[i];
			if (num2 != -1f)
			{
				num2 = Mathf.Clamp(num2, 0f, array[i]);
				num += (int)(array2[i] * Mathf.Pow(array[i] - num2, 2f));
			}
		}
		return num;
	}

	public static int calculateScore_user()
	{
		float[] array = new float[4]
		{
			GlobalController.getUserPB(RaceManager.RACE_EVENT_60M),
			GlobalController.getUserPB(RaceManager.RACE_EVENT_100M),
			GlobalController.getUserPB(RaceManager.RACE_EVENT_200M),
			GlobalController.getUserPB(RaceManager.RACE_EVENT_400M)
		};
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == float.MaxValue)
			{
				array[i] = -1f;
			}
		}
		return calculateScore(array);
	}
}

using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
	public GlobalController gc;

	public TaskManager taskManager;

	public GameObject selectionButtonPrefab;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void loadSelectedGhosts()
	{
		taskManager.addTask(TaskManager.LOAD_SELECTED_RACERS);
	}

	public void loadSelectedPlayer()
	{
		taskManager.addTask(TaskManager.LOAD_SELECTED_PLAYER);
	}

	public void createCharacter()
	{
		taskManager.addTask(TaskManager.CREATE_RACER);
	}

	public void clearRacersFromScene()
	{
		taskManager.addTask(TaskManager.CLEAR_RACERS_FROM_SCENE);
	}
}

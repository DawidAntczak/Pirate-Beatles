using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    // scoreboard will not update every frame, only on enabling

	[SerializeField] private GameObject playerScoreboardItem;
	[SerializeField] private Transform playerScoreboardList;

    void OnEnable()
	{
		Player[] players = FindObjectsOfType<Player>();

		foreach (Player player in players)
		{
			GameObject itemGO = Instantiate(playerScoreboardItem, playerScoreboardList) as GameObject;
			PlayerScoreboardItem item = itemGO.GetComponent<PlayerScoreboardItem>();
			if (item)
			{
				item.Setup(player.GetComponent<PlayerID>().PlayerNickname, player.Kills, player.Deaths);
			}
		}
	}

	public void OnDisable ()
	{
		foreach (Transform child in playerScoreboardList)
		{
			Destroy(child.gameObject);
		}
	}
}

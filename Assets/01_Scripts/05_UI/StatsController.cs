using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsController : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI enemyIst;
	[SerializeField] TextMeshProUGUI enemySoll;

	[SerializeField] TextMeshProUGUI keysIst;
	[SerializeField] TextMeshProUGUI keysSoll;

	[SerializeField] TextMeshProUGUI secretsIst;
	[SerializeField] TextMeshProUGUI secretsSoll;

	[SerializeField] TextMeshProUGUI hitsIst;
	[SerializeField] TextMeshProUGUI hitsSoll;

	[SerializeField] TextMeshProUGUI itemsIst;
	[SerializeField] TextMeshProUGUI itemsSoll;

    // Start is called before the first frame update
    void Start()
    {
        enemyIst.text = PlayerStats.Me().enemiesKilled.ToString();
		enemySoll.text = PlayerStats.Me().maxEnemy.ToString();

		keysIst.text = PlayerStats.Me().keysFound.ToString();
		keysSoll.text = PlayerStats.Me().maxKeys.ToString();

		secretsIst.text = PlayerStats.Me().secretsFound.ToString();
		secretsSoll.text = PlayerStats.Me().maxSecrets.ToString();

		hitsIst.text = PlayerStats.Me().hits.ToString();
		hitsSoll.text = PlayerStats.Me().shotsFired.ToString();

		itemsIst.text = PlayerStats.Me().items.ToString();
		itemsSoll.text = PlayerStats.Me().maxItems.ToString();
    }
}

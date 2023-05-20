using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDead: MonoBehaviour
{
	[SerializeField] Inventory player;
	[SerializeField] Image imgBlood;

	[SerializeField] AnimationCurve tintCurve;


	[SerializeField] Image imgBlackOut;
	[Range (0f, 1f)]
	[SerializeField] float speedTaint;

    // Update is called once per frame
    void Update()
    {
		if (player.health > 0f)
		{
			float alpha = tintCurve.Evaluate ((100f - player.health) / 100f);
			imgBlood.color = new Color (imgBlood.color.r, imgBlood.color.g, imgBlood.color.b, alpha);
		}
		else
		{
			Time.timeScale = 0f;
			imgBlackOut.gameObject.SetActive (true);

			if (imgBlackOut.color.a >= 1f)
			{
				Time.timeScale = 1f;
				SceneManager.LoadScene (3);
			}
			else
				imgBlackOut.color = new Color (imgBlackOut.color.r, imgBlackOut.color.g, imgBlackOut.color.b, imgBlackOut.color.a + (Time.unscaledDeltaTime * speedTaint));
		}
    }
}

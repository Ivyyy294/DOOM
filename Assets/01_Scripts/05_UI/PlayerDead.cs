using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerDead: MonoBehaviour
{
	[SerializeField] Image img;
	[Range (0f, 1f)]
	[SerializeField] float maxTaint;
	[Range (0f, 1f)]
	[SerializeField] float speedTaint;

    // Start is called before the first frame update
    void OnEnable()
    {
        Time.timeScale = 0f;
		img.color = new Color (img.color.r, img.color.g, img.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (img.color.a < maxTaint)
			img.color = new Color (img.color.r, img.color.g, img.color.b, img.color.a + (Time.unscaledDeltaTime * speedTaint));
		else
		{
			Time.timeScale = 1f;
			SceneManager.LoadScene (2);
		}

    }
}

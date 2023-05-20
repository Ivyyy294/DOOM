using UnityEngine.SceneManagement;
using UnityEngine;

public class Credits : MonoBehaviour
{
	[SerializeField] RectTransform targetPos;
	[SerializeField] float speed = 1f;

	RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent <RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rectTransform.anchoredPosition != targetPos.anchoredPosition)
			rectTransform.anchoredPosition = Vector2.MoveTowards (rectTransform.anchoredPosition, targetPos.anchoredPosition, speed * Time.deltaTime);
		else
			SceneManager.LoadScene(1);
    }
}

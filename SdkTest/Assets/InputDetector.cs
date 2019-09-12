using UnityEngine;

public class InputDetector : MonoBehaviour
{
	public KeyCode input;
	private SpriteRenderer sprite;

	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
	}


	void Update()
	{
		if (Input.GetKey(input))
		{
			sprite.enabled = true;
		}
		else
		{
			sprite.enabled = false;
		}
	}
}

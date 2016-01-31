using UnityEngine;
using UnityEngine.UI;

public class SpeedIndicator : MonoBehaviour
{
	public Sprite bicycle;
	public Sprite car;
	public Sprite horse;
	public Sprite rabbit;
	public Sprite racing;
	public Sprite snail;
	public Sprite train;
	public Sprite turtle;

	public void SetNumNotifications(float numNotifications)
	{
		Sprite wantedSprite;

		if (numNotifications > 25)
		{
			wantedSprite = snail;
		}
		else if (numNotifications > 20)
		{
			wantedSprite = turtle;
		}
		else if (numNotifications > 15)
		{
			wantedSprite = rabbit;
		}
		else if (numNotifications > 10)
		{
			wantedSprite = horse;
		}
		else if (numNotifications > 5)
		{
			wantedSprite = racing;
		}
		else if (numNotifications > 3)
		{
			wantedSprite = bicycle;
		}
		else if (numNotifications >= 1)
		{
			wantedSprite = car;
		}
		else
		{
			wantedSprite = train;
		}

		GetComponent<Image>().sprite = wantedSprite;
	}
}
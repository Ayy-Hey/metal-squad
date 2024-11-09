using System;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
	private void Start()
	{
		
	}

	
	private void PushNotification()
	{
		long num = 86400000L;
		if (ProfileManager.unlockAll)
		{
			num = 60000L;
		}
		int num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(1, num, this.title, this.message[num2]);
		num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(2, num * 3L, this.title, this.message[num2]);
		num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(3, num * 5L, this.title, this.message[num2]);
		num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(4, num * 7L, this.title, this.message[num2]);
		num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(5, num * 10L, this.title, this.message[num2]);
		num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(6, num * 14L, this.title, this.message[num2]);
		num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(7, num * 18L, this.title, this.message[num2]);
		num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(8, num * 18L, this.title, this.message[num2]);
		num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(9, num * 22L, this.title, this.message[num2]);
		num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(10, num * 26L, this.title, this.message[num2]);
		num2 = UnityEngine.Random.Range(0, this.message.Length - 1);
		this.Repeating(11, num * 30L, this.title, this.message[num2]);
	}

	private void Repeating(int id, long timeDelay, string title, string message)
	{
		LocalNotification.SendNotification(id, timeDelay, title, message, new Color32(byte.MaxValue, 68, 68, byte.MaxValue), true, true, true, "app_icon", null, "default", new LocalNotification.Action[0]);
	}

	private string messageBestSale = "Big Sale. Up to 50%. Today only!";

	private string title = "Metal Squad!";

	private string[] message = new string[]
	{
		"Big Sale. Up to 50%. Today only!",
		"Daily Gift is ready!",
		"There are so many levels to beat ! Come and play Metal Squad!",
		"It's time to play Metal Squad!",
		"Hey! You are missing out lots of new stuff on Metal Squad. Come and play now! ",
		"We need you today. Let's get back and destroy new enemies !!!",
		"We haven't heard you for a while.Comeback and fight now? We miss you."
	};
}

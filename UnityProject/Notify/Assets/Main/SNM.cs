using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


// Simple Notification Manager
public class SNM : MonoBehaviour
{
	static int UtcSeconds
	{
		get
		{
			DateTime m = new DateTime( 2016,1,1,0,0,0, DateTimeKind.Utc );
			DateTime n = DateTime.UtcNow;

			TimeSpan s = n-m;
			return (int)s.TotalSeconds;
		}
	}

	//----------------------------------------------------------------------------------
	// Member Declarations
	//----------------------------------------------------------------------------------

	[System.Serializable]
	public class ScheduledNotification
	{
		public string name = null;
		public int secondsAt = 0;
		public AppBehaviour onApp = null;

		Notification submittedNotification = null;

		public void Submit()
		{
			if( submittedNotification != null || onApp == null || UtcSeconds < secondsAt )
				return;

			submittedNotification = new Notification( name );
			//submittedNotification.text = "from manager";

			onApp.SendNotification( submittedNotification );
		}

		public void Cancel()
		{
			if( submittedNotification == null )
				return;
			
			onApp.DismissNotification( submittedNotification );
		}
	}

	//----------------------------------------------------------------------------------
	// Statics ( Singleton )
	//----------------------------------------------------------------------------------

	protected static SNM singletonInstance;

	public static SNM Instance
	{
		get
		{
			if (singletonInstance == null)
			{
				singletonInstance = FindObjectOfType<SNM>();
#if UNITY_EDITOR
				if( singletonInstance == null )
					Debug.LogWarning("Trying to access singleton, but cannot be found in the scene [CollectorMain]");
#endif
			}

			return singletonInstance;
		}
	}

	void Awake()
	{
		if( singletonInstance != null && singletonInstance != this )
		{
#if UNITY_EDITOR
			Debug.LogWarning("Singleton instance already exists within the scene. Destroying self but leaving the gameObject intact : " + this.gameObject.name );
#endif
			Destroy( this );
		}
		else
			singletonInstance = this;
	}

	protected void OnApplicationQuit()
	{
		singletonInstance = null;
	}

	//----------------------------------------------------------------------------------
	// Inspector Variables
	//----------------------------------------------------------------------------------

	[SerializeField] private AppManager appManager = null;

	//----------------------------------------------------------------------------------
	// Member Variables
	//----------------------------------------------------------------------------------

	List<ScheduledNotification> notifications = new List<ScheduledNotification>();

	//----------------------------------------------------------------------------------
	// Notifications
	//----------------------------------------------------------------------------------

	public void ScheduleNotification( string gameName, string notifName, int afterSeconds )
	{
		AppBehaviour app = appManager.GetAppBehaviour( gameName );

		if( app == null )
		{
			Debug.LogError( "Add the app" );
			return;
		}

		ScheduledNotification n = new ScheduledNotification();
		n.name = notifName;
		n.secondsAt = UtcSeconds + afterSeconds;
		n.onApp = app;

		notifications.Add( n );
	}

	public void CancelNotification( string gameName, string notifName )
	{
		for( int i=notifications.Count-1; i>=0; --i )
		{
			if( notifications[i].name != notifName || notifications[i].onApp == null || notifications[i].onApp.name != gameName+"(Clone)" )
				continue;

			notifications[i].Cancel();
			notifications.RemoveAt( i );
		}
	}

	void Update()
	{
		for( int i=0; i<notifications.Count; ++i )
		{
			notifications[i].Submit();
		}
	}
}

using System;
using UnityEngine;
using System.Collections;

public class GameCollectorApp : AppBehaviour
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

	[System.Serializable]
	public class GameDataSetup
	{
		public string gameName = "";
		public int purchased = 0;
		public int collectAfterSeconds = 0;
	}


	[SerializeField] private GameDataSetup[] setupData = null;

	void Start()
	{
		PlayerPrefs.SetInt( "GameCollector.MainController.m_Coins", 200 );

		int now = UtcSeconds;
		// save the setup data
		for( int i=0; i<setupData.Length; ++i )
		{
			PlayerPrefs.SetInt( "GameCollector.CollectorGameManager.m_LastCollectedAtSeconds." + setupData[i].gameName, now );
			PlayerPrefs.SetInt( "GameCollector.CollectorGameManager.m_Purchased." + setupData[i].gameName, setupData[i].purchased );

			SNM.Instance.ScheduleNotification( "Game Collector", setupData[i].gameName, setupData[i].collectAfterSeconds );
		}
	}

	public override void Launch ()
	{
	}


	public override void Cleanup ()
	{
	}
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Balloons
{
	
	public class BalloonsApp : AppBehaviour
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
		// Member Variables
		//----------------------------------------------------------------------------------

		int m_MaxCount = 3;

		int m_NotificationsActive = 0;
		Coroutine m_CounterRoutine = null;

		public int SpawnBalloonCount
		{
			get{ return m_NotificationsActive; }
		}

		//----------------------------------------------------------------------------------
		// Methods ( Game Setup )
		//----------------------------------------------------------------------------------

		void Start()
		{
			// setup the initial preferences and notifications
			PlayerPrefs.SetInt( "Balloons.BalloonsApp.m_BalloonCount", m_MaxCount );

			BeginCounter();
		}


		public void StopCounter()
		{
			if( this == null || m_CounterRoutine == null )
				return;

			Debug.Log( "Stop" );
			StopCoroutine( m_CounterRoutine );
			m_CounterRoutine = null;
		}


		public void BeginCounter()
		{
			if( m_CounterRoutine != null )
				return;

			Debug.Log( "Begin" );
			m_CounterRoutine = StartCoroutine( NotifyRoutine() );
		}

		IEnumerator NotifyRoutine()
		{
			while( true )
			{
				yield return null;
				while( m_NotificationsActive < m_MaxCount )
				{
					// wait random times and add a ball + notification
					yield return new WaitForSeconds( UnityEngine.Random.Range( 1f, 3f ) );

					m_NotificationsActive++;
					// assign the player pref also
					SNM.Instance.ScheduleNotification( "Balloons", "BalloonNotif_" + m_NotificationsActive, 0 );
				}
			}

			yield break;
		}


		public void ReduceBalloons()
		{
			SNM.Instance.CancelNotification( "Balloons", "BalloonNotif_" + m_NotificationsActive );
			m_NotificationsActive--;
		}

		public void GameCleared()
		{
			m_MaxCount++;
			PlayerPrefs.SetInt( "Balloons.BalloonsApp.m_BalloonCount", m_MaxCount );
		}

		//----------------------------------------------------------------------------------
		// AppBehaviour Overrides
		//----------------------------------------------------------------------------------

		public override void Launch()
		{
		}

		public override void Cleanup()
		{
		}


	}

}
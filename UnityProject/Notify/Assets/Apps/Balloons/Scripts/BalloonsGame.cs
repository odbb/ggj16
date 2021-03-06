﻿using UnityEngine;
using System.Collections.Generic;

namespace Balloons
{
	
	public class BalloonsGame : MonoBehaviour
	{
		//----------------------------------------------------------------------------------
		// Inspector Variables
		//----------------------------------------------------------------------------------

		[SerializeField] private Balloon[] balloonPrefabs = null;
		[SerializeField] private BoxCollider2D spawnRegion = null;

		//----------------------------------------------------------------------------------
		// Member Variables
		//----------------------------------------------------------------------------------

		List<Balloon> m_Balloons = new List<Balloon>();
		int m_BalloonCount = 0;

		BalloonsApp m_AppController = null;

		//----------------------------------------------------------------------------------
		// Methods ( Game Initialisation )
		//----------------------------------------------------------------------------------

		void OnEnable()
		{
			m_AppController = ( AppManager.GetSingleton().GetAppBehaviour( "Balloons" ) ) as BalloonsApp;
			m_AppController.StopCounter();
		}

		void OnDisable()
		{
			m_AppController.BeginCounter();
		}

		//----------------------------------------------------------------------------------
		// Methods ( Balloons )
		//----------------------------------------------------------------------------------

		void Start()
		{
			int createCount = m_AppController.SpawnBalloonCount;
			//if( PlayerPrefs.HasKey( "Balloons.BalloonsApp.m_BalloonCount" ) )
			//	createCount = PlayerPrefs.GetInt( "Balloons.BalloonsApp.m_BalloonCount" );

			if( spawnRegion != null )
			{
				float x = spawnRegion.size.x / 2f;
				float y = spawnRegion.size.y / 2f;

				// create balloons to the count
				for( int i=0; i<createCount; ++i )
				{
					CreateBalloon( Random.Range( -x, x ), Random.Range( -y, y ) );
				}

				Destroy( spawnRegion.gameObject );
			}
		}

		void CreateBalloon( float x, float y )
		{
			Balloon balloon = balloonPrefabs[ Random.Range( 0, balloonPrefabs.Length ) ];
			balloon = Instantiate<Balloon>( balloon );
			balloon.transform.parent = this.transform;
			balloon.transform.localPosition = new Vector3( x, y, 0 );

			SpriteRenderer[] renderers = balloon.GetComponentsInChildren<SpriteRenderer>();
			for( int i=0; i<renderers.Length; ++i )
			{
				renderers[i].sortingOrder = renderers[i].sortingOrder + (m_BalloonCount*2);
			}

			m_Balloons.Add( balloon );
			m_BalloonCount ++;
		}

		public void RemoveBalloon( Balloon b )
		{
			for( int i=0; i<m_Balloons.Count; ++i )
			{
				if( m_Balloons[i] == b )
				{
					m_Balloons.RemoveAt( i );
					break;
				}
			}

			m_AppController.ReduceBalloons();

			if( m_Balloons.Count == 0 )
			{
				// notifications cleared, save values and return to main
				//PlayerPrefs.SetInt( "Balloons.BalloonsApp.m_BalloonCount", m_BalloonCount + 1 );

				/* // TODO submit this part as a bug report
				// Because RemoveBalloon is done during OnDestroy, when unloading the scene it crashes
				AppManager appManager = AppManager.GetSingleton();
				AppBehaviour activeApp = appManager.GetAppBehaviour( "Balloons" );

				if( activeApp != null )
					activeApp.Kill();
				*/

				m_AppController.GameCleared();
				Invoke( "ToKill", 0.5f );
			}
		}

		public void ToKill()
		{
			AppBehaviour activeApp = AppManager.GetSingleton().GetAppBehaviour( "Balloons" );

			if( activeApp != null )
				activeApp.Kill();
		}

	}

}
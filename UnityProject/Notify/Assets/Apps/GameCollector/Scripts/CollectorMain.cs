using UnityEngine;
using System.Collections;

namespace GameCollector
{
	
	public class CollectorMain : MonoBehaviour
	{
		//----------------------------------------------------------------------------------
		// Statics ( Singleton )
		//----------------------------------------------------------------------------------

		protected static CollectorMain singletonInstance;

		public static CollectorMain Instance
		{
			get
			{
				if (singletonInstance == null)
				{
					singletonInstance = FindObjectOfType<CollectorMain>();
#if UNITY_EDITOR
					if( singletonInstance == null )
						Debug.LogWarning("Trying to access singleton, but cannot be found in the scene [CollectorMain]");
#endif
				}

				return singletonInstance;
			}
		}


		//----------------------------------------------------------------------------------
		// Member Variables
		//----------------------------------------------------------------------------------

		int m_Coins = 0;

		//----------------------------------------------------------------------------------
		// Properties
		//----------------------------------------------------------------------------------

		public int Coins
		{
			get{ return m_Coins; }
			set{ m_Coins = value; }
		}

		//----------------------------------------------------------------------------------
		// Methods ( Initialisation )
		//----------------------------------------------------------------------------------

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

		//----------------------------------------------------------------------------------
		// 'OS' Events
		//----------------------------------------------------------------------------------

		public void OnMiniGameQuit()
		{
			// save data
			PlayerPrefs.SetInt( "GameCollector.MainController.m_Coins", m_Coins );
		}

		protected void OnApplicationQuit()
		{
			singletonInstance = null;
		}
	}

}

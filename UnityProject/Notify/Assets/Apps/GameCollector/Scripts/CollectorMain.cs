using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
		// Inspector Variables
		//----------------------------------------------------------------------------------

		[SerializeField] private Text coinDisplay = null;
		[SerializeField] private AppBehaviour app = null;

		//----------------------------------------------------------------------------------
		// Member Variables
		//----------------------------------------------------------------------------------

		int m_Coins = 0;
		string m_CoinFormat = null;

		//----------------------------------------------------------------------------------
		// Properties
		//----------------------------------------------------------------------------------

		public int Coins
		{
			get{ return m_Coins; }
			set
			{
				m_Coins = value;
				coinDisplay.text = string.Format( m_CoinFormat, m_Coins.ToString() );
			}
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

			m_CoinFormat = coinDisplay.text;
			Coins = 10;
		}

		//----------------------------------------------------------------------------------
		// Interaction Methods
		//----------------------------------------------------------------------------------

		public void Close()
		{
			app.Kill();
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

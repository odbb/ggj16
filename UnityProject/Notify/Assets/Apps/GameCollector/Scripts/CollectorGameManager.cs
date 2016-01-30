﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace GameCollector
{
	
	public class CollectorGameManager : MonoBehaviour
	{
		//----------------------------------------------------------------------------------
		// Inspector Variables
		//----------------------------------------------------------------------------------

		[Header( "General" )]
		[SerializeField] private Text ui_name = null;

		[Header( "Progress Slider" )]
		[SerializeField] private Slider ui_ProgressSlider = null;
		[SerializeField] private Text ui_CollectTime = null;
		[SerializeField] private Button ui_CollectButton = null;

		[Header( "Purchases" )]
		[SerializeField] private Text ui_PurchaseCost = null;
		[SerializeField] private int startCost = 100;
		[SerializeField] private float increaseCostRate = 1.3f;
		[SerializeField] private Button ui_PurchaseButton = null;
		
		[Header( "Collection" )]
		[SerializeField] private Text ui_Reward = null;
		[SerializeField] private float collectAfterSeconds = 60;
		[SerializeField] private int coinsPerOwned = 10;

		//----------------------------------------------------------------------------------
		// Member Variables
		//----------------------------------------------------------------------------------

		// these values are recovered on game load
		float m_LastCollectedAtTime = 0;
		int m_Purchased = 0;

		int m_NextPurchaseCost = 1;
		bool m_ReadyForCollection = false;

		string m_NameFormat = "";
		string m_TimerFormat = "";
		string m_CostFormat = "";
		string m_rewardFormat = "";

		//----------------------------------------------------------------------------------
		// Methods ( Initialisation )
		//----------------------------------------------------------------------------------

		void Awake()
		{
			m_TimerFormat = ui_CollectTime.text;
			m_CostFormat = ui_PurchaseCost.text;
			m_rewardFormat = ui_Reward.text;
			m_NameFormat = ui_name.text;
		}

		void Start()
		{
			// load old data
			m_LastCollectedAtTime = PlayerPrefs.GetFloat( "GameCollector.CollectorGameManager.m_LastCollectedAtTime" );
			m_Purchased = PlayerPrefs.GetInt( "GameCollector.CollectorGameManager.m_Purchased" );

			ui_CollectTime.text = "";
				
			UpdateCost();

			if( m_Purchased == 0 )
				return;

			float secondsPassed = Time.time - m_LastCollectedAtTime;
			if( secondsPassed > (float)collectAfterSeconds )
			{
				SetCollectable();
			}
		}

		//----------------------------------------------------------------------------------
		// Updates
		//----------------------------------------------------------------------------------

		void Update()
		{
			if( m_ReadyForCollection || m_Purchased == 0 )
			{
				if( m_NextPurchaseCost <= CollectorMain.Instance.Coins )
					ui_PurchaseButton.interactable = true;
				else
					ui_PurchaseButton.interactable = false;
				return;
			}
			
			float secondsPassed = Time.time - m_LastCollectedAtTime;
			if( secondsPassed > collectAfterSeconds )
			{
				SetCollectable();
			}
			else
			{
				float progress = secondsPassed / collectAfterSeconds;
				ui_ProgressSlider.value = progress;
				ui_CollectTime.text = string.Format( m_TimerFormat, ( (int)(collectAfterSeconds - secondsPassed) ).ToString() );
			}

			if( m_NextPurchaseCost <= CollectorMain.Instance.Coins )
				ui_PurchaseButton.interactable = true;
			else
				ui_PurchaseButton.interactable = false;
		}

		void SetCollectable()
		{
			m_ReadyForCollection = true;
			ui_CollectButton.gameObject.SetActive( true );
		}

		//----------------------------------------------------------------------------------
		// Methods ( Interaction )
		//----------------------------------------------------------------------------------

		public void Purchase()
		{
			// make sure have coins
			if( CollectorMain.Instance.Coins < m_NextPurchaseCost )
			{
				Debug.Log( "Cannot afford another" );
				return;
			}

			// remove the coins
			CollectorMain.Instance.Coins -= m_NextPurchaseCost;

			if( m_Purchased == 0 )
				m_LastCollectedAtTime = Time.time;

			m_Purchased ++;
			UpdateCost();
		}

		void UpdateCost()
		{
			m_NextPurchaseCost = startCost;
			for( int i=0; i<m_Purchased; ++i )
			{
				m_NextPurchaseCost += Mathf.FloorToInt( (float)m_NextPurchaseCost * increaseCostRate );
			}

			ui_PurchaseCost.text = string.Format( m_CostFormat, m_NextPurchaseCost.ToString() );
			ui_Reward.text = string.Format( m_rewardFormat, (m_Purchased * coinsPerOwned).ToString() );
			ui_name.text = string.Format( m_NameFormat, m_Purchased.ToString() );
		}

		public void Collect()
		{
			// reward the coins
			CollectorMain.Instance.Coins += m_Purchased * coinsPerOwned;

			m_ReadyForCollection = false;
			ui_CollectButton.gameObject.SetActive( false );
			m_LastCollectedAtTime = Time.time;
		}

		//----------------------------------------------------------------------------------
		// 'OS' Events
		//----------------------------------------------------------------------------------

		public void OnMiniGameQuit()
		{
			// save data
			PlayerPrefs.SetFloat( "GameCollector.CollectorGameManager.m_LastCollectedAtTime", m_LastCollectedAtTime );
			PlayerPrefs.SetInt( "GameCollector.CollectorGameManager.m_Purchased", m_Purchased );

			// Schedule Notification
			int seconds = Mathf.FloorToInt( collectAfterSeconds - Time.time - m_LastCollectedAtTime );
			if( seconds > 0 )
			{
				// submit
			}
		}
	}

}
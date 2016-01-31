﻿using UnityEngine;
using System.Collections;

namespace Balloons
{
	
	public class Balloon : MonoBehaviour
	{
		public void Pop()
		{
			// TODO display the pop effect

			Destroy( this.gameObject );
		}

		void OnDestroy()
		{
			// inform the BalloonsApp
			BalloonsGame main = FindObjectOfType<BalloonsGame>();
			main.RemoveBalloon( this );
		}
	}

}

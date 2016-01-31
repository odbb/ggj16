using UnityEngine;
using System.Collections;

namespace Balloons
{
	
	public class Balloon : MonoBehaviour
	{
		public void Pop()
		{
			// TODO display the pop effect

			//Destroy( this.gameObject );

			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			sr.enabled = false;
			Collider2D c = GetComponent<Collider2D>();
			c.enabled = false;


			AudioSource s = GetComponent<AudioSource>();
			if( s != null )
				s.Play();

			BalloonsGame main = FindObjectOfType<BalloonsGame>();
			main.RemoveBalloon( this );

			SpriteRenderer csr = GetComponentInChildren<SpriteRenderer>();
			csr.enabled = false;
		}

		void OnDestroy()
		{
			// inform the BalloonsApp

		}
	}

}

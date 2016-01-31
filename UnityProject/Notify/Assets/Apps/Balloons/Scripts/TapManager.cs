using UnityEngine;
using System.Collections;

namespace Balloons
{
		
	public class TapManager : MonoBehaviour
	{
		//----------------------------------------------------------------------------------
		// Inspector Variables
		//----------------------------------------------------------------------------------

		[SerializeField] private Camera castingCamera = null;

		//----------------------------------------------------------------------------------
		// Touch input casting
		//----------------------------------------------------------------------------------

		void LateUpdate()
		{
#if ! UNITY_EDITOR && ( UNITY_IOS || UNITY_ANDROID )
			Touch[] touches = Input.touches;
			for( int i=0; i<touches.Length; ++i )
			{
				if( touches[i].phase != TouchPhase.Began )
					continue;

				RaycastHit2D hit = Physics2D.Raycast( castingCamera.ScreenToWorldPoint( touches[i].position ), Vector2.zero, 0f );
				if( hit.transform != null )
				{
					Balloon balloon = hit.transform.GetComponent<Balloon>();
					if( balloon != null )
						balloon.Pop();
				}
			}
#else
			if( Input.GetMouseButtonDown( 0 ) )
			{
				RaycastHit2D hit = Physics2D.Raycast( castingCamera.ScreenToWorldPoint( Input.mousePosition ), Vector2.zero, 0f );
				if( hit.transform != null )
				{
					Balloon balloon = hit.transform.GetComponent<Balloon>();
					if( balloon != null )
						balloon.Pop();
				}
			}
#endif
		}
	}

}

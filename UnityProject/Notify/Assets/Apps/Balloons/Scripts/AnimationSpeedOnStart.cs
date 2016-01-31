using UnityEngine;
using System.Collections;

public class AnimationSpeedOnStart : MonoBehaviour
{
	//----------------------------------------------------------------------------------
	// Inspector Variables
	//----------------------------------------------------------------------------------

	[SerializeField] private Vector2 speedRange = Vector2.one;
	[SerializeField] private AnimationClip clip = null;

	//----------------------------------------------------------------------------------
	// Member Variables
	//----------------------------------------------------------------------------------

	Animation anim = null;
		
	//----------------------------------------------------------------------------------
	// Methods ( Initialisation )
	//----------------------------------------------------------------------------------

	void Start()
	{
		if( clip == null )
			return;

		anim = GetComponent<Animation>();
		if( anim == null )
			return;

		anim[clip.name].speed = Random.Range( speedRange.x, speedRange.y );
		StartCoroutine( LerpToSpeedRange() );
	}

	IEnumerator LerpToSpeedRange()
	{
		yield return new WaitForSeconds( Random.Range( 0f, 5f ) );

		while( true )
		{
			float startSpeed = anim[clip.name].speed;
			float endSpeed = Random.Range( speedRange.x, speedRange.y );
			float v = 0;
			float length = Random.Range( 3, 5 );

			for( float t=0; t<length; t += Time.deltaTime )
			{
				v = t/length;
				anim[clip.name].speed = Mathf.SmoothStep( startSpeed, endSpeed, v );

				yield return null;
			}
		}
	}
}

using UnityEngine;
using System.Collections;

namespace Balloons
{
	
	public class RandomFace : MonoBehaviour
	{
		//----------------------------------------------------------------------------------
		// Inspector Variables
		//----------------------------------------------------------------------------------

		[SerializeField] private SpriteRenderer spriteRenderer = null;
		[SerializeField] private Sprite[] sprites = null;

		//----------------------------------------------------------------------------------
		// Methods ( Random Setup )
		//----------------------------------------------------------------------------------

		void Start()
		{
			spriteRenderer.sprite = sprites[ Random.Range( 0, sprites.Length ) ];
			
			Destroy( this );
		}
	}

}

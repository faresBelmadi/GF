using UnityEngine;
using System.Collections;

namespace Synapse
{
	namespace Demo
	{
		[AddComponentMenu("Synapse/Demo/NPC/Collision Manager")]
		public class CollisionManager : MonoBehaviour
		{
			void OnControllerColliderHit(ControllerColliderHit a_collision)
			{
				CollectibleValue collectible = a_collision.collider.GetComponent<CollectibleValue>();
					
				if(collectible != null  &&  CollectibleValue.m_instances.Contains(collectible))
				{
					Points componentPoints = GetComponent<Points>();
					componentPoints.Add(collectible.Points);
					
					CollectibleValue.m_instances.Remove(collectible);
					Destroy(collectible.gameObject);
				}
			}
		}
	}
}
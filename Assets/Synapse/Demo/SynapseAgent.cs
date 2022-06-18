using UnityEngine;
using System.Collections;

using Synapse.Runtime;

namespace Synapse
{
	namespace Demo
	{
		[AddComponentMenu("Synapse/Demo/NPC/Synapse Agent")]
		public class SynapseAgent : MonoBehaviour
		{
			public float m_speed;
			
			private Brain m_synapseBrain;
			private CharacterController m_controller;
			private Heat m_heatComponent;
			
			private CollectibleValue m_target;
			
			IEnumerator Start()
			{
				m_target = null;
				
				m_controller = GetComponent<CharacterController>();
				m_heatComponent = GetComponent<Heat>();
				
				m_synapseBrain = new SynapseLibrary_SynapseDemo.Demo.NPC(this);
				
				while(Application.isPlaying  &&  m_synapseBrain != null)
				{
					AIUpdate();
					yield return new WaitForSeconds(1);
				}
			}
			
			void AIUpdate()
			{
				if(m_synapseBrain.Process() == false)
				{
					m_target = null;
				}
			}
			
			void Update()
			{
				transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
				
				if(m_target != null)
				{
					Vector3 velocity = m_target.transform.position - transform.position;
					velocity.y = 0.0f;
					velocity.Normalize();
					Vector3 currentVelocity = velocity * m_speed;
					
					m_controller.Move(currentVelocity * Time.deltaTime);
				}
			}
			
			object[] GetLayerCollectiblesData()
			{
				return CollectibleValue.m_instances.ToArray();
			}
			
			void GetSensorPositionData(out Vector3 a_position)
			{
				a_position = gameObject.transform.position;
			}
			
			void GetSensorHeatValueData(out float a_heatValue)
			{
				a_heatValue = m_heatComponent.HeatValue;
			}
			
			void GetSensorHeatSpeedData(out float a_heatSpeed)
			{
				a_heatSpeed = m_heatComponent.m_heatSpeed;
			}
			
			void DesirePickCallback(object a_collectible)
			{		
				m_target = a_collectible as CollectibleValue;
			}
			
			void DesireCoolCallback()
			{
				m_target = null;
			}
		}
	}
}
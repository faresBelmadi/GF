using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Synapse
{
	namespace Demo
	{
		[AddComponentMenu("Synapse/Demo/Collectible/Value")]
		public class CollectibleValue : MonoBehaviour
		{
			[System.Serializable]
			public class Value
			{
				public int m_point;
				public Color m_color;
			}
			
			public Value[] m_values;
			
			public static List<CollectibleValue> m_instances = new List<CollectibleValue>();
			private int m_index;	
			
			public void Init()
			{
				m_index = Random.Range(0, m_values.Length);
				GetComponent<Renderer>().material.color = m_values[m_index].m_color;
				
				m_instances.Add(this);
			}
			
			public int Points
			{
				get
				{
					return m_values[m_index].m_point;
				}
			}
			
			void GetSensorPositionData(out Vector3 a_position)
			{
				a_position = gameObject.transform.position;
			}
			
			void GetSensorValueData(out int a_value)
			{
				a_value = m_values[m_index].m_point;
			}
		}
	}
}
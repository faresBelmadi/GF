using UnityEngine;
using System.Collections;

namespace Synapse
{
	namespace Demo
	{
		[AddComponentMenu("Synapse/Demo/NPC/Points")]
		public class Points : MonoBehaviour
		{
			private int m_points;
			
			void Start()
			{
				m_points = 0;
			}
			
			void OnGUI()
			{
				GUILayout.Label("Points : " + m_points);
			}
			
			public void Add(int a_points)
			{
				m_points += a_points;
			}
		}
	}
}
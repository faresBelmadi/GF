using UnityEngine;
using System.Collections;

namespace Synapse
{
	namespace Demo
	{
		[AddComponentMenu("Synapse/Demo/World/Spawner")]
		public class RandomSpawner : MonoBehaviour
		{
			public GameObject m_prefabToSpawn;
			public float m_spawnPeriod;
			
			public float m_spawnDemiWidth;
			public float m_spawnDemiLength;
			
			private float m_timeElapsedSinceLastSpawn;
			
			void Start()
			{
				m_timeElapsedSinceLastSpawn = Random.Range(0.0f, m_spawnPeriod);
			}
			
			void Update()
			{
				m_timeElapsedSinceLastSpawn += Time.deltaTime;
				
				while(m_timeElapsedSinceLastSpawn >= m_spawnPeriod)
				{
					Spawn();
					m_timeElapsedSinceLastSpawn -= m_spawnPeriod;
				}
			}
			
			void Spawn()
			{
				float x = Random.Range(-m_spawnDemiWidth, m_spawnDemiWidth);
				float z = Random.Range(-m_spawnDemiLength, m_spawnDemiLength);
				GameObject instance = GameObject.Instantiate(m_prefabToSpawn, new Vector3(x, 0.25f, z), Quaternion.identity) as GameObject;
				CollectibleValue componentValue = instance.GetComponent<CollectibleValue>();
				componentValue.Init();
			}
		}
	}
}
using System.Collections.Generic;
using UnityEngine;


namespace NVIDIA.Flex
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class simuData : MonoBehaviour
    {
        int length;
        public static Vector4[] _particles = new Vector4[125001];
        public static vertexSystem.vertexIndex[] groups;
        int[] testDraw;
        #region Messages
        void OnEnable()
        {
            m_actor = GetComponent<FlexActor>();
            if (m_actor)
            {
                m_actor.onFlexUpdate += OnFlexUpdate;
            }
        }

        void OnDisable()
        {
            if (m_actor)
            {
                m_actor.onFlexUpdate -= OnFlexUpdate;
                m_actor = null;
            }
        }


        #endregion

        #region Private
        public Vector4[] GetParticles()
        {
            return _particles;
        }
        public int[] GetIndices()
        {
      
            return m_actor.indices;
        }
        void Update()
        {
            _vertexSystem.SetData(GetIndices(), GetParticles(), GetBounds(), m_actor.container.radius / 3,ref groups);
            _vertexSystem.GroupByCells();
            _surfaceRecognition.SetData(_particles, GetBounds(), ref groups, m_actor.container.radius / 3);
            this.testDraw =  _surfaceRecognition.findAreaCells(0);
        }
        public Bounds GetBounds()
        {
            Bounds b = new Bounds();
            Vector3 min = m_actor.bounds.min;
            Vector3 max = m_actor.bounds.max;

            min.x -= (m_actor.container.radius / 3);
            min.z -= (m_actor.container.radius / 3);
            min.y -= (m_actor.container.radius / 3);
            max.x += (m_actor.container.radius / 3);
            max.z += (m_actor.container.radius / 3);
            max.y += (m_actor.container.radius / 3);
            b.SetMinMax(min, max);
            return b;
        }

  
        void OnFlexUpdate(FlexContainer.ParticleData _particleData)
        {
            if (m_actor && m_actor.container)
            {
                length = m_actor.indexCount;
                _particleData.GetParticles(m_actor.indices[0], 15625, _particles);
            }
         
        }
        public virtual void OnDrawGizmos()
        {
            ////////////////////////////////////////////////////////////////////
            Bounds b = new Bounds();
            b = GetBounds();
            

            for (int i = 0; i < testDraw.Length; i++)
            {
                if(testDraw[i] > groups.Length)
                {
                    Debug.Log(testDraw[i]);
                    Debug.Log(groups.Length);
                }
                for (int j = 0; j < groups[testDraw[i]].pointIndice.Length; j++)
                {
                    if(groups[testDraw[i]].pointIndice[j] != -1)
                    {
                        Gizmos.color = Color.red;
                        //Gizmos.DrawSphere(new Vector3(GetParticles()[groups[i].pointIndice[j]].x, GetParticles()[groups[i].pointIndice[j]].y, GetParticles()[groups[i].pointIndice[j]].z), m_actor.container.radius / 3);
                        Gizmos.DrawSphere(new Vector3(GetParticles()[groups[testDraw[i]].pointIndice[j]].x, GetParticles()[groups[testDraw[i]].pointIndice[j]].y, GetParticles()[groups[testDraw[i]].pointIndice[j]].z),m_actor.container.radius / 3);

                    }
                }
            }
            //Gizmos.DrawWireCube(testDraw.center, testDraw.size);
            ////////////////////////////////////////////////////////////////////////////
        }


        FlexActor m_actor;
        SurfaceRecognition _surfaceRecognition = new SurfaceRecognition();
        vertexSystem _vertexSystem = new vertexSystem();
        #endregion
    }
}

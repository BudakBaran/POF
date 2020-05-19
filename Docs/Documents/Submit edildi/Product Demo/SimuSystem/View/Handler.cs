using System.Collections.Generic;
using UnityEngine;


namespace NVIDIA.Flex
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    public class Handler : MonoBehaviour
    {
        int length;
        public static Vector4[] _particles = new Vector4[4096];
        public static HashSystem.HashModel[] groups;
        //int[]
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

            _vertexSystem.SetData(GetIndices(), GetParticles(), GetBounds(), (m_actor.container.radius*2) / 3, ref groups);
            _vertexSystem.GroupByCells();

            _surfaceRecognition.SetData(_particles, GetBounds(), ref groups, (m_actor.container.radius * 2) / 3);
            int[] particles = new int[1] { 0 };
            this.testDraw =  _surfaceRecognition.findBoundary();
            // _marchingCubes.StartSimu(testDraw)
        }

        // Decide AABB
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

        public virtual void OnDrawGizmos()
        {
            ////////////////////////////////////////////////////////////////////

            for (int i = 0; i < testDraw.Length; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(new Vector3(_particles[testDraw[i]].x, _particles[testDraw[i]].y, _particles[testDraw[i]].z), m_actor.container.radius / 2);
            }

        }

        /// Get particle data when nFlex updated
        void OnFlexUpdate(FlexContainer.ParticleData _particleData)
        {
            if (m_actor && m_actor.container)
            {
                length = m_actor.indexCount;
                _particleData.GetParticles(m_actor.indices[0], 4096, _particles);
              
            }
         
        }

        FlexActor m_actor;
        SurfaceRecognition _surfaceRecognition = new SurfaceRecognition();
        HashSystem _vertexSystem = new HashSystem();
        #endregion
    }
}

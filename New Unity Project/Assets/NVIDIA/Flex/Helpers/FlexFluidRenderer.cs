

using System.Collections.Generic;
using UnityEngine;

namespace NVIDIA.Flex
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("NVIDIA/Flex/Flex Fluid Renderer")]
    public class FlexFluidRenderer : MonoBehaviour
    {
        #region Messages
        void Start()
        {
            _simuData = GetComponent<simuData>();
        }

        void OnDisable()
        {
 
        }

        void Update()
        {

        }
        #endregion

        #region Private

        public Vector4 particle;

        public Vector4 get()
        {
            return particle;
        }

        #endregion
        simuData _simuData;
      

    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Game
{
    public class ContentLoader : MonoBehaviour
    {
        private void Start()
        {
            Load();
        }
        
        public void Load()
        {
            if(Application.platform == RuntimePlatform.LinuxServer)
                Debug.Log("LinuxServer");
            else if(Application.platform == RuntimePlatform.WindowsEditor)
                Debug.Log("WindowsEditor");
            else if(Application.platform == RuntimePlatform.WindowsPlayer)
                Debug.Log("WindowsPlayer");

#if UNITY_STANDALONE
            Debug.Log("UNITY_STANDALONE");
#elif Unity_server
            Debug.Log("Unity_server");
#endif

        }
    }
}

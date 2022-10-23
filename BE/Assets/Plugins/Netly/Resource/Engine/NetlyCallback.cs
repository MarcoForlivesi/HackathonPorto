using Netly.Core;
using UnityEngine;

namespace Netly.Unity
{
    public class NetlyCallback : MonoBehaviour
    {
        private static NetlyCallback instance;

        private void Awake()
        {
            Active();

            // verify, only instance
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // this instance is default instance
            instance = this;

            // allow unity "machine" manager, netly callback's
            Call.Automatic = false;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            Active();
            // release netly callback's to unity "machine"            
            Call.Publish();
        }

        private void Active()
        {
            enabled = true;
            gameObject.SetActive(true);
        }
    }
}

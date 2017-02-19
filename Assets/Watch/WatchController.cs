using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace MagicDuel
{
    public class WatchController : MonoBehaviour
    {
        public GameObject healthDisplay;
        public GameObject manaDisplay;

        private Material healthMaterial;
        private Material manaMaterial;
        
        private void Start()
        {
            Assert.IsNotNull(healthDisplay, "Health Display not set");
            Assert.IsNotNull(healthDisplay, "Mana Display not set");

            healthMaterial = healthDisplay.GetComponent<MeshRenderer>().material;
            manaMaterial = manaDisplay.GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            healthMaterial.SetFloat("_Percent", Mathf.PingPong(Time.time, 1));
            manaMaterial.SetFloat("_Percent", Mathf.PingPong(Time.time * 1.5f, 1));
        }
    }
}

using UnityEngine;
using UnityEngine.Assertions;

namespace MagicDuel.Wizards
{
    public class Cover : MonoBehaviour
    {
        public new Collider collider { get; private set; }

        public void Awake()
        {
            collider = GetComponent<Collider>();

            Assert.IsNotNull(collider, "Collider not found");
        }
    }
}

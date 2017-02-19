using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

namespace MagicDuel
{
    public class TestTarget : MonoBehaviour
    {
        private Wizards.Wizard[] wizards;

        private void Awake()
        {
            wizards = FindObjectsOfType<Wizards.Wizard>();
        }

        private void SetTarget(object sender)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                var o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                o.name = "Movement Target";
                o.transform.position = hit.point;
                o.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                Destroy(o, 3);

                foreach (var wizard in wizards)
                {
                    //wizard.TakeCoverFrom(o.transform);
                    wizard.SetTarget(o);
                }
            }
        }
    }
}

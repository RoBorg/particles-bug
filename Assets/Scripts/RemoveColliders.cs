using UnityEngine;

[ExecuteInEditMode]
public class RemoveColliders : MonoBehaviour
{
    private void Start()
    {
        foreach (Transform child in transform)
        {
            bool hasCollider = false;

            do
            {
                hasCollider = false;
                Component collider = child.gameObject.GetComponent<Collider>();

                if (collider != null)
                {
                    hasCollider = true;
                    DestroyImmediate(collider);
                }
            } while (hasCollider);
        }
    }
}

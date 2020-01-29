using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootShadow : MonoBehaviour
{
    public GameObject origin;
    public float originOffset = 0.4f;
    public float offset = -0.2f;
    public float maxHeight = 3.0f;

    public LayerMask levelLayermask;

    protected SpriteRenderer SpriteRenderer;
    protected Vector3 OriginalSize;
    protected ContactFilter2D ContactFilter;
    protected RaycastHit2D[] ContactCache = new RaycastHit2D[6];

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        OriginalSize = transform.localScale;

        ContactFilter = new ContactFilter2D();
        ContactFilter.layerMask = levelLayermask;
        ContactFilter.useLayerMask = true;
    }

    void LateUpdate()
    {
        int count = Physics2D.Raycast((Vector2)origin.transform.position + Vector2.up * originOffset, Vector2.down, ContactFilter, ContactCache);

        if (count > 0)
        {
            SpriteRenderer.enabled = true;
            transform.position = ContactCache[0].point + ContactCache[0].normal * offset;

            float height = Vector3.SqrMagnitude(origin.transform.position - transform.position);
            float ratio = Mathf.Clamp(1.0f - height / (maxHeight * maxHeight), 0.0f, 1.0f);

            transform.localScale = OriginalSize * ratio;
        }
        else
        {
            SpriteRenderer.enabled = false;
        }
    }
}

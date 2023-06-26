using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesappearingPlatform : MonoBehaviour
{
    public float disappearingCD = 3f;
    public float reappearingCD = 5f;
    public bool isTriggered = false;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {

            StartCoroutine(disappearingEvent());
        }
    }

    public IEnumerator disappearingEvent()
    {
        isTriggered = true;
        //Audio ?
        yield return new WaitForSeconds(disappearingCD / 3f);
        float blinkDuration = 0.1f;
        int timesToBlink = (int)Mathf.Round(((disappearingCD*(2f/3f)) / blinkDuration)/2f);
        for (int i = 0; i < timesToBlink; i++)
        {
            setActiveRenderers(false);
            yield return new WaitForSeconds(blinkDuration);
            setActiveRenderers(true);
            yield return new WaitForSeconds(blinkDuration);
        }
        setActiveRenderers(false);
        setActiveColliders(false);
        yield return new WaitForSeconds(reappearingCD);
        setActiveColliders(true);
        setActiveRenderers(true);
        isTriggered = false;
    }

    private void setActiveColliders(bool value)
    {
        foreach (Collider collider in transform.GetComponentsInChildren<Collider>())
        {
            if (collider != null)
            {
                collider.enabled = value;

            }
        }
    }

    

    private void setActiveRenderers(bool value)
    {
        foreach (Renderer rend in transform.GetComponentsInChildren<MeshRenderer>())
        {
            if (rend != null)
            {
                rend.enabled = value;
            }
        }
        if(TryGetComponent(out Renderer renderer))
        {
            renderer.enabled = value;
        }
    }
}

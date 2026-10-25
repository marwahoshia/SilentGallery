using System.Collections;
using UnityEngine;

public class ShowAfterDelay : MonoBehaviour
{
    public GameObject target;
    public float delay = 1f;

    public void Show()
    {
        StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        yield return new WaitForSeconds(delay);
        target.SetActive(true);
    }
}
using UnityEngine;
using System.Collections;

public class WarningUIController : MonoBehaviour
{
    [SerializeField] private GameObject warningUI;
    [SerializeField] private float showTime = 2f;

    public void ShowWarning()
    {
        StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        warningUI.SetActive(true);

        yield return new WaitForSeconds(showTime);

        warningUI.SetActive(false);
    }

}
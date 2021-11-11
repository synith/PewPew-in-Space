using System.Collections;
using TMPro;
using UnityEngine;

public class UIShake : MonoBehaviour
{
    private GameObject objectToShake;
    private bool canShake;
    private bool canShakeUp;
    private float offset = 0.8f;
    Vector3 startPos;

    private void Update()
    {
        if (canShake)
            Shake();
    }
    private void Shake()
    {
        if (canShakeUp)
        {
            objectToShake.transform.position = new Vector3(startPos.x, startPos.y + offset);
            canShakeUp = false;
        }
        else
        {
            objectToShake.transform.position = new Vector3(startPos.x, startPos.y - offset);
            canShakeUp = true;
        }        
    }

    private IEnumerator ActivateShake()
    {
        canShake = true;
        yield return new WaitForSeconds(0.5f);
        objectToShake.transform.position = startPos;
        canShake = false;
    }

    public void ShakeUI(GameObject objectToShake)
    {
        this.objectToShake = objectToShake;
        startPos = objectToShake.transform.position;
        StartCoroutine(ActivateShake());
    }
}

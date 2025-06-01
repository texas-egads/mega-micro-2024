using UnityEngine;
using System.Collections;

namespace Panoodlers
{

public class InstructionScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(flashWords());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator flashWords()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);

    }
}

}

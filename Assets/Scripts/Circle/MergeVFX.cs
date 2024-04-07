using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeVFX : MonoBehaviour
{
   [SerializeField] private ParticleSystem mainVFX;

    public ParticleSystem MainVFX { get {  return mainVFX; } } 
    private void Awake()
    {
        mainVFX = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
    }
    public void SetTransform(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
    public void PlayParticle()
    {
        MainVFX.Play();
        StartCoroutine(SetParticleOffWhenDone());
    }
    public IEnumerator SetParticleOffWhenDone() {
        if(!MainVFX.IsAlive())
        {
            
            gameObject.SetActive(false);
        }
        yield return new WaitUntil(() => !mainVFX.IsAlive());
        gameObject.SetActive(false);
    }
}

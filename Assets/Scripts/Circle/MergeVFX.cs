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
        SetParticleOffWhenDone();
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
    }
    public void SetParticleOffWhenDone() {
        if(!MainVFX.IsAlive())
        {
            //Debug.Log("Particle system has finished playing");

            // Do something when the particle system has finished playing
            // For example, disable the particle system or trigger another event
            gameObject.SetActive(false);
        }
    }
}

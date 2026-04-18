using UnityEngine;
using System.Collections.Generic;

public class ParticleContainer : MonoBehaviour
{
    [Header("Configuración")]
    public ParticleType containerType;
    public int maxParticles = 20;
    public GameObject particlePrefab;

    [Header("Spawn")]
    public Transform spawnPoint;

    private List<GameObject> particles = new List<GameObject>();
    private int currentCount = 0;

    public int CurrentCount => currentCount;

    public void AddParticle()
    {
        if (currentCount >= maxParticles) return;

        Vector3 spawnPos = transform.position;

        // Pequeño offset aleatorio para que no spawnen en el mismo punto
        spawnPos += new Vector3(
            Random.Range(-0.2f, 0.2f),
            Random.Range(-0.2f, 0.2f),
            0
        );

        GameObject p = Instantiate(particlePrefab, spawnPos, Quaternion.identity);
        Particle particle = p.GetComponent<Particle>();

        if (particle != null)
        {
            particle.particleType = containerType;
            particle.SetInContainer(true);
        }

        particles.Add(p);
        currentCount++;
    }

    public void RemoveParticle()
    {
        if (currentCount <= 0) return;

        GameObject last = particles[particles.Count - 1];
        particles.RemoveAt(particles.Count - 1);
        Destroy(last);
        currentCount--;
    }

    public void SetCount(int count)
    {
        count = Mathf.Clamp(count, 0, maxParticles);

        while (currentCount < count) AddParticle();
        while (currentCount > count) RemoveParticle();
    }

    public void Clear()
    {
        foreach (GameObject p in particles)
            if (p != null) Destroy(p);

        particles.Clear();
        currentCount = 0;
    }
}
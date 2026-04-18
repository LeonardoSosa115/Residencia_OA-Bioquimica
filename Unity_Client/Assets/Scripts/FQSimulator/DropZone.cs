using UnityEngine;
using UnityEngine.InputSystem;

public class DropZone : MonoBehaviour
{
    [Header("Referencias")]
    public AtomController atomController;
    public ParticleSpawner particleSpawner;

    private bool absorcionEsteFrame = false;

    void LateUpdate()
    {
        absorcionEsteFrame = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (absorcionEsteFrame) return;

        Particle particle = other.GetComponent<Particle>();
        if (particle == null) return;

        // Solo absorbe la partícula que el usuario está arrastrando
        if (!particle.IsDragging) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            AbsorberParticula(particle);
            absorcionEsteFrame = true;
        }
    }

    public void AbsorberParticula(Particle particle)
    {
        Debug.Log($"[DropZone] AbsorberParticula llamado. Tipo: {particle.particleType}, Frame: {Time.frameCount}");
        
        switch (particle.particleType)
        {
            case ParticleType.Proton:
                atomController.AddProton();
                particleSpawner.ContainerProtones.RemoveSpecific(particle.gameObject);
                break;
            case ParticleType.Neutron:
                atomController.AddNeutron();
                particleSpawner.ContainerNeutrones.RemoveSpecific(particle.gameObject);
                break;
            case ParticleType.Electron:
                atomController.AddElectron();
                particleSpawner.ContainerElectrones.RemoveSpecific(particle.gameObject);
                break;
        }

        particleSpawner.ActualizarUI();
        Destroy(particle.gameObject);
    }
}
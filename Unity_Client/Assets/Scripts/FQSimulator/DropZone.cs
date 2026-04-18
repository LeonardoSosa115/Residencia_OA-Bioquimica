using UnityEngine;
using UnityEngine.InputSystem;

public class DropZone : MonoBehaviour
{
    [Header("Referencias")]
    public AtomController atomController;
    public ParticleSpawner particleSpawner;

    private void OnTriggerStay2D(Collider2D other)
    {
        Particle particle = other.GetComponent<Particle>();
        if (particle == null) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            AbsorberParticula(particle);
        }
    }

    public void AbsorberParticula(Particle particle)
    {
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
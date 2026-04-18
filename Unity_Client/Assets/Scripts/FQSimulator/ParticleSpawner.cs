using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ParticleSpawner : MonoBehaviour
{
    [Header("Contenedores")]
    public ParticleContainer containerProtones;
    public ParticleContainer containerNeutrones;
    public ParticleContainer containerElectrones;

    [Header("UI - Contadores")]
    public TMP_InputField inputProtones;
    public TMP_InputField inputNeutrones;
    public TMP_InputField inputElectrones;

    public ParticleContainer ContainerProtones   => containerProtones;
    public ParticleContainer ContainerNeutrones  => containerNeutrones;
    public ParticleContainer ContainerElectrones => containerElectrones;

    void Start()
    {
        ActualizarUI();
    }

    // ── Protones ──────────────────────────────
    public void OnAddProton()
    {
        containerProtones.AddParticle();
        ActualizarUI();
    }

    public void OnRemoveProton()
    {
        containerProtones.RemoveParticle();
        ActualizarUI();
    }

    public void OnInputProton(string value)
    {
        if (int.TryParse(value, out int count))
        {
            containerProtones.SetCount(count);
            ActualizarUI();
        }
    }

    // ── Neutrones ─────────────────────────────
    public void OnAddNeutron()
    {
        containerNeutrones.AddParticle();
        ActualizarUI();
    }

    public void OnRemoveNeutron()
    {
        containerNeutrones.RemoveParticle();
        ActualizarUI();
    }

    public void OnInputNeutron(string value)
    {
        if (int.TryParse(value, out int count))
        {
            containerNeutrones.SetCount(count);
            ActualizarUI();
        }
    }

    // ── Electrones ────────────────────────────
    public void OnAddElectron()
    {
        containerElectrones.AddParticle();
        ActualizarUI();
    }

    public void OnRemoveElectron()
    {
        containerElectrones.RemoveParticle();
        ActualizarUI();
    }

    public void OnInputElectron(string value)
    {
        if (int.TryParse(value, out int count))
        {
            containerElectrones.SetCount(count);
            ActualizarUI();
        }
    }

    // ── Utilidades ────────────────────────────
    public void ActualizarUI()
    {
        if (inputProtones != null)
            inputProtones.text = containerProtones.CurrentCount.ToString();
        if (inputNeutrones != null)
            inputNeutrones.text = containerNeutrones.CurrentCount.ToString();
        if (inputElectrones != null)
            inputElectrones.text = containerElectrones.CurrentCount.ToString();
    }

    public void LimpiarTodo()
    {
        containerProtones.Clear();
        containerNeutrones.Clear();
        containerElectrones.Clear();
        ActualizarUI();
    }
}
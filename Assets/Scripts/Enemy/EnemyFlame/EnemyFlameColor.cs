using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using static UnityEngine.ParticleSystem.Particle;

public class EnemyFlameColor : MonoBehaviour
{
    public Color color;

    private ParticleSystem fireParticleSystem;
    private Light flameLight;
    private Renderer sphereRenderer;

    // Start is called before the first frame update
    void Start()
    {
        fireParticleSystem = GetComponentInChildren<ParticleSystem>();
        flameLight = GetComponentInChildren<Light>();
        sphereRenderer = GetComponentInChildren<Renderer>();

        setParticleColor();
        setFlameLightColor();
    }

    void setParticleColor()
    {
        Particle[] particles = new Particle[1];
        fireParticleSystem.GetParticles(particles);
        ParticleSystem.ColorOverLifetimeModule colorOverLifetimeModule = fireParticleSystem.colorOverLifetime;
        particles[0].startColor = color;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0.60f), new GradientColorKey(Color.white, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.70f), new GradientAlphaKey(1f, 1.0f) }
        );

        // Apply the gradient.
        colorOverLifetimeModule.enabled = true;
        colorOverLifetimeModule.color = new ParticleSystem.MinMaxGradient(gradient);
    }

    void setFlameLightColor()
    {
        flameLight.color = color;
    }

}

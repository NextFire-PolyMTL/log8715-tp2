using UnityEngine;

// inspired from https://unity3d.com/learn/tutorials/topics/scripting/displaying-particles-script
public class ParticlePool : MonoBehaviour
{
    public struct ParticleDecalData
    {
        public Vector3 position;
        public float size;
        public Vector3 rotation;
        public Color color;
    }

    public const int maxParticle = 10000;

    private ParticleSystem _particleSystem;
    private ParticleDecalData[] particleData;
    private ParticleSystem.Particle[] particles;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[maxParticle];
        particleData = new ParticleDecalData[maxParticle];
        for (int i = 0; i < maxParticle; i++)
        {
            particleData[i] = new ParticleDecalData();
        }
    }

    public void CreateParticle(uint index, Vector3 position, float size, Color particleColor)
    {
        particleData[index].position = position;
        particleData[index].size = size;
        particleData[index].color = particleColor;
    }

    public void SetParticlePosition(uint index, Vector3 position)
    {
        particleData[index].position = position;
    }
    public void SetParticleSize(uint index, float size)
    {
        particleData[index].size = size;
    }
    public void SetParticleColor(uint index, Color color)
    {
        particleData[index].color = color;
    }

    // to set the color
    public void DisplayParticlesFirst()
    {
        for (int i = 0; i < particleData.Length; i++)
        {
            particles[i].position = particleData[i].position;
            particles[i].startSize = particleData[i].size;
            particles[i].startColor = particleData[i].color;
        }

        _particleSystem.SetParticles(particles, particles.Length);
    }

    public void DisplayParticles()
    {
        for (int i = 0; i < particleData.Length; i++)
        {
            particles[i].position = particleData[i].position;
            particles[i].startSize = particleData[i].size;
            particles[i].startColor = particleData[i].color;
        }

        _particleSystem.SetParticles(particles, particles.Length);
    }
}
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuoyancyObject : MonoBehaviour
{
    public Transform[] floaters;

    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;

    public Material oceanMaterial; // Assign your Shader Graph material here

    private Rigidbody m_Rigidbody;
    private int floatersUnderWater;
    private bool underwater;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        floatersUnderWater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float waveHeight = GetWaveHeightAtPosition(floaters[i].position);
            float difference = floaters[i].position.y - waveHeight;
            if (difference < 0f)
            {
                m_Rigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floaters[i].position, ForceMode.Force);
                floatersUnderWater++;
                if (!underwater)
                {
                    underwater = true;
                    SwitchState(true);
                }
            }
        }

        if (underwater && floatersUnderWater == 0)
        {
            underwater = false;
            SwitchState(false);
        }
    }

    void SwitchState(bool isUnderWater)
    {
        if (isUnderWater)
        {
            m_Rigidbody.linearDamping = underWaterDrag;
            m_Rigidbody.angularDamping = underWaterAngularDrag;
        }
        else
        {
            m_Rigidbody.linearDamping = airDrag;
            m_Rigidbody.angularDamping = airAngularDrag;
        }
    }

    float GetWaveHeightAtPosition(Vector3 worldPosition)
    {
        float speedX = oceanMaterial.GetFloat("_WaveSpeedA");
        float speedY = oceanMaterial.GetFloat("_WaveSpeedB");
        float waveHeight = oceanMaterial.GetFloat("_WaveHeight"); // single float now

        float time = Time.time;

        // Animate UV offset like shader
        float offsetX = time * speedX;
        float offsetY = time * speedY;

        // Compute UV coordinates based on X and Z world space
        float uvX = worldPosition.x + offsetX;
        float uvY = worldPosition.z + offsetY;

        // Use the same scale values used in Shader Graph for noise
        float noiseScaleA = 10f;
        float noiseScaleB = 8f;

        // Use PerlinNoise as approximation of gradient noise
        float noiseA = Mathf.PerlinNoise(uvX * noiseScaleA, uvY * noiseScaleA); // for remapA (X channel)
        float noiseB = Mathf.PerlinNoise(uvX * noiseScaleB, uvY * noiseScaleB); // for remapB (Y channel)

        // Remap noise values to match shader logic
        float remapB = Mathf.Lerp(-0.15f, 0.15f, noiseB); // final Y displacement

        return remapB * waveHeight;
    }
}

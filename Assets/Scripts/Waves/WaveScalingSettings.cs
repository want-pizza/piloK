[System.Serializable]
public class WaveScalingSettings
{
    public float startPower = 10f;
    public float maxPower = 35;
    public float powerGrowth = 1.25f;

    public float baseDuration = 10f;
    public float maxDuration = 30f;
    public float durationPerPower = 0.5f;
    public float durationPerWaveIndex = 1.25f;
}

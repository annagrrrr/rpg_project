using UnityEngine;

public class MobilePerformanceOptimizer : MonoBehaviour
{
    [System.Serializable]
    public class QualityTier
    {
        public string name = "Medium";
        public int targetFPS = 45;
        public float farClipPlane = 60f;
        public float farCullDistance = 800f;
        public ShadowQuality shadows = ShadowQuality.HardOnly;
        public int pixelLightCount = 1;
        public int antiAliasing = 0;
        public bool softParticles = false;
        public float shadowDistance = 25f;
        public float textureQuality = 1f;
    }

    [Header("Device Detection")]
    public bool autoDetectQuality = true;
    public int forcedQualityLevel = -1;

    [Header("Quality Tiers")]
    public QualityTier[] qualityTiers = new QualityTier[]
    {
        new QualityTier()
        {
            name = "Low",
            targetFPS = 30,
            farClipPlane = 40f,
            farCullDistance = 600f,
            shadows = ShadowQuality.Disable,
            pixelLightCount = 0,
            antiAliasing = 0,
            softParticles = false,
            shadowDistance = 15f,
            textureQuality = 1f
        },
        new QualityTier()
        {
            name = "Medium",
            targetFPS = 45,
            farClipPlane = 60f,
            farCullDistance = 800f,
            shadows = ShadowQuality.HardOnly,
            pixelLightCount = 1,
            antiAliasing = 0,
            softParticles = false,
            shadowDistance = 25f,
            textureQuality = 1f
        },
        new QualityTier()
        {
            name = "High",
            targetFPS = 60,
            farClipPlane = 100f,
            farCullDistance = 1000f,
            shadows = ShadowQuality.All,
            pixelLightCount = 2,
            antiAliasing = 2,
            softParticles = true,
            shadowDistance = 40f,
            textureQuality = 0f
        }
    };

    [Header("Camera Settings")]
    public Camera targetCamera;
    public float nearClipPlane = 0.1f;
    public bool disableHDR = true;
    public bool disableMSAA = true;
    public float fieldOfView = 60f;

    [Header("VSync Settings")]
    public bool enableVSync = false;
    public int vSyncCount = 0;

    [Header("Layer Culling")]
    public bool enableLayerCulling = true;
    public string[] closeRangeLayers = { "Details", "Effects" };
    public float closeCullDistance = 20f;

    private int currentQualityLevel = 1;
    private float fpsUpdateTime = 0f;
    private int fpsFrameCount = 0;
    private float currentFPS = 0f;

    void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        InitializeGraphicsAPI();
        ApplyBaseSettings();
        DetectQualityLevel();
        ApplyQualitySettings();
        OptimizeCamera();
        SetupLayerCulling();
    }

    void Update()
    {
        UpdateFPS();
    }

    void InitializeGraphicsAPI()
    {
        QualitySettings.vSyncCount = enableVSync ? vSyncCount : 0;
        Application.targetFrameRate = qualityTiers[currentQualityLevel].targetFPS;
    }

    void ApplyBaseSettings()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void DetectQualityLevel()
    {
        if (forcedQualityLevel >= 0 && forcedQualityLevel < qualityTiers.Length)
        {
            currentQualityLevel = forcedQualityLevel;
            return;
        }

        if (!autoDetectQuality)
        {
            currentQualityLevel = 1;
            return;
        }

        int processorScore = SystemInfo.processorFrequency > 2300 ? 2 :
                           SystemInfo.processorFrequency > 1800 ? 1 : 0;

        int memoryScore = SystemInfo.systemMemorySize > 6000 ? 2 :
                         SystemInfo.systemMemorySize > 3000 ? 1 : 0;

        int gpuScore = SystemInfo.graphicsMemorySize > 3000 ? 2 :
                      SystemInfo.graphicsMemorySize > 1500 ? 1 : 0;

        int totalScore = processorScore + memoryScore + gpuScore;

        if (totalScore >= 5) currentQualityLevel = 2;
        else if (totalScore >= 2) currentQualityLevel = 1;
        else currentQualityLevel = 0;
    }

    void ApplyQualitySettings()
    {
        QualityTier tier = qualityTiers[currentQualityLevel];

        Application.targetFrameRate = tier.targetFPS;

        QualitySettings.shadows = tier.shadows;
        QualitySettings.pixelLightCount = tier.pixelLightCount;
        QualitySettings.antiAliasing = tier.antiAliasing;
        QualitySettings.softParticles = tier.softParticles;
        QualitySettings.shadowDistance = tier.shadowDistance;
        QualitySettings.globalTextureMipmapLimit = (int)tier.textureQuality;

        if (targetCamera != null)
        {
            targetCamera.farClipPlane = tier.farClipPlane;
        }

        Debug.Log($"Applied quality: {tier.name} (Level {currentQualityLevel})");
    }

    void OptimizeCamera()
    {
        if (targetCamera == null) return;

        targetCamera.nearClipPlane = nearClipPlane;
        targetCamera.fieldOfView = fieldOfView;

        if (disableHDR)
            targetCamera.allowHDR = false;

        if (disableMSAA)
            targetCamera.allowMSAA = false;
    }

    void SetupLayerCulling()
    {
        if (!enableLayerCulling || targetCamera == null) return;

        float[] layerDistances = new float[32];
        float farCull = qualityTiers[currentQualityLevel].farCullDistance;

        for (int i = 0; i < layerDistances.Length; i++)
        {
            layerDistances[i] = farCull;
        }

        foreach (string layerName in closeRangeLayers)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if (layer >= 0)
            {
                layerDistances[layer] = closeCullDistance;
            }
        }

        targetCamera.layerCullDistances = layerDistances;
        targetCamera.layerCullSpherical = true;
    }

    void UpdateFPS()
    {
        fpsFrameCount++;
        fpsUpdateTime += Time.unscaledDeltaTime;

        if (fpsUpdateTime >= 0.5f)
        {
            currentFPS = fpsFrameCount / fpsUpdateTime;
            fpsFrameCount = 0;
            fpsUpdateTime = 0f;
        }
    }

    public void SetQualityLevel(int level)
    {
        if (level >= 0 && level < qualityTiers.Length)
        {
            currentQualityLevel = level;
            ApplyQualitySettings();
            SetupLayerCulling();
        }
    }

    void OnGUI()
    {
        if (!Debug.isDebugBuild) return;

        Rect rect = new Rect(10, 10, 200, 25);
        GUI.Label(rect, $"FPS: {currentFPS:F1}");
        rect.y += 20;
        GUI.Label(rect, $"Quality: {qualityTiers[currentQualityLevel].name}");
        rect.y += 20;
        GUI.Label(rect, $"Render Distance: {qualityTiers[currentQualityLevel].farCullDistance}m");
        
    }
}
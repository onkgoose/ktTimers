using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerController : MonoBehaviour
{
    [Header("radial таймер")]
    [SerializeField] private Image clockImage;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private float duration = 5f;

    [Header("линейный таймеr")]
    [SerializeField] private Image linearTimerImage;
    [SerializeField] private float linearDuration = 3f; 

    [Header("ресурс")]
    [SerializeField] private TMP_Text resourceText;
    [SerializeField] private int startResource = 0;
    [SerializeField] private int resourcePerFill = 3;    
    [SerializeField] private int costPerLaunch = 5;      
    private bool isRadialRunning;
    private float radialTimeLeft;
    private int launchCount;
    private float linearTime;
    private int resource;

    private void Start()
    {
        clockImage.fillAmount = 1f;
        counterText.text = "0";
        launchCount = 0;
        linearTimerImage.fillAmount = 0f;
        linearTime = 0f;
        resource = startResource;
        UpdateResourceUI();
        UpdateButtonState();

        startButton.onClick.AddListener(StartRadialTimer);
    }

    private void Update()
    {
        UpdateRadial();
        UpdateLinear();
    }

    private void UpdateRadial()
    {
        if (!isRadialRunning) return;

        radialTimeLeft -= Time.deltaTime;
        clockImage.fillAmount = Mathf.Clamp01(radialTimeLeft / duration);

        if (radialTimeLeft <= 0f)
        {
            isRadialRunning = false;
            clockImage.fillAmount = 1f;
            UpdateButtonState();
        }
    }

    private void StartRadialTimer()
    {
        if (isRadialRunning) return;
        if (resource < costPerLaunch) return;   

        resource -= costPerLaunch;
        UpdateResourceUI();
        UpdateButtonState();

        isRadialRunning = true;
        radialTimeLeft = duration;
        clockImage.fillAmount = 1f;

        launchCount++;
        counterText.text = launchCount.ToString();
    }
    private void UpdateLinear()
    {
        linearTime += Time.deltaTime;
        linearTimerImage.fillAmount = Mathf.Clamp01(linearTime / linearDuration);

        if (linearTime >= linearDuration)
        {
            linearTime = 0f;                    
            linearTimerImage.fillAmount = 0f;   

            resource += resourcePerFill;        
            UpdateResourceUI();
            UpdateButtonState();
        }
    }
    private void UpdateResourceUI()
    {
        resourceText.text = resource.ToString();
    }
    private void UpdateButtonState()
    {
        bool canPress = !isRadialRunning && resource >= costPerLaunch;
        startButton.interactable = canPress;
    }
}
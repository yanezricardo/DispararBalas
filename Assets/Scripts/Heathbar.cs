using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Heathbar : MonoBehaviour
{
    private Slider _slider;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;
   
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    public void SetHeath(float health)
    {
        _slider.value = health;

        if (_gradient != null)
            _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

    public void SetMaxHeath(float health)
    {
        _slider.maxValue = health;
        _slider.value = health;

        if (_gradient != null)
            _fill.color = _gradient.Evaluate(1f);
    }
}

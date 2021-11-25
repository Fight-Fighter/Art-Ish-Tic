using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{

	public Slider slider;
	//public Gradient gradient;
	public Image fill;



	public void SetMaxValue(float mv)
	{
		slider.maxValue = mv;

		//fill.color = gradient.Evaluate(1f);
	}

	public void SetValue(float v)
	{
		slider.value = v;

		//fill.color = gradient.Evaluate(slider.normalizedValue);
	}

	public void SetColor(Color c)
    {
		fill.color = c;
    }

}
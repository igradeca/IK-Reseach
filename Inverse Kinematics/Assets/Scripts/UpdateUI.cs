using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour {

    private GameObject sliderArmLength;
    private GameObject textArmLegth;

    private GameObject sliderSegmentsNum;
    private GameObject textSegVal;

    public ArmLogicScript armLogicScript;

    public float armLength;

    // Use this for initialization
    void Awake () {

        sliderArmLength = GameObject.Find("SliderArmLength");
        textArmLegth = GameObject.Find("TextLengthVal");
        ChangeLengthValueText();
        sliderSegmentsNum = GameObject.Find("SliderSegmentsNum");
        textSegVal = GameObject.Find("TextSegVal");
        ChangeSegValText();

        armLogicScript = GameObject.Find("GM").GetComponent<ArmLogicScript>();
        SetNumberOfSegmets();
        SetSegmentLength();
    }

    public void ChangeLengthValueText() {

        textArmLegth.GetComponent<Text>().text = sliderArmLength.GetComponent<Slider>().value.ToString();
    }

    public void SetSegmentLength() {

        armLength = (float)(sliderArmLength.GetComponent<Slider>().value * 0.3f);
        armLogicScript.armLength = armLength;
        armLogicScript.SetNewArmLenghts();
    }

    public void ChangeSegValText() {

        textSegVal.GetComponent<Text>().text = sliderSegmentsNum.GetComponent<Slider>().value.ToString();
    }

    public void SetNumberOfSegmets() {

        armLogicScript.numberOfSegments = (int)sliderSegmentsNum.GetComponent<Slider>().value;
    }
    

}

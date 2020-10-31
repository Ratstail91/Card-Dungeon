using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour {
	//components
	public Image background;

	public TMP_Text eventHeader; //"Event"
	public TMP_Text actionHeader; //"Action"
	public TMP_Text complicationHeader; //"Complication:"

	public TMP_Text eventText;
	public TMP_Text actionText;
	public TMP_Text complicationText; //starts with 24 space characters

	public TMP_Text tagTypeHeader;
	public Image tagTypeImage;

	//input members
	[TextArea]
	public string eventRules;
	[TextArea]
	public string actionRules;
	[TextArea]
	public string complicationRules;
	[TextArea]
	public string tagTypeRules; //change this to an enumeration?

	public void Populate() {
		//TODO: handle too-long box sizes
		eventText.text = eventRules;
		actionText.text = actionRules;

		//handle complication rules
		if (complicationRules.Length == 0) {
			complicationHeader.enabled = false;
			complicationText.enabled = false;
		} else {
			complicationHeader.enabled = true;
			complicationText.enabled = true;

			complicationText.text = "                        " + complicationRules;
		}

		//handle tag types
		if (tagTypeRules.Length == 0) {
			tagTypeImage.enabled = false;
			tagTypeHeader.enabled = false;
		} else {
			tagTypeImage.enabled = true;
			tagTypeHeader.enabled = true;
			tagTypeHeader.text = tagTypeRules;
		}
	}
}
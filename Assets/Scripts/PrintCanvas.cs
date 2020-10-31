using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintCanvas : MonoBehaviour {
	public class CardData {
		public string eventRules;
		public string actionRules;
		public string complicationRules;
		public string tagTypeRules;
	}

	const int width = 750 * 3;
	const int height = 1050 * 3;

	public GameObject cardPrefab;

	public Camera renderCamera;

	public string inputCSV;

	List<CardData> cardData;

	void Start() {
		ReadCards(inputCSV);
		StartCoroutine(RenderCards());
	}

	void ReadCards(string fname) {
		cardData = new List<CardData>();

		using(var reader = new StreamReader(fname)) {
		while (!reader.EndOfStream)
		{
			var line = reader.ReadLine();
			var values = line.Split(';');

			CardData cd = new CardData();

			cd.eventRules = values[0];
			cd.actionRules = values[1];
			cd.complicationRules = values[2];
			cd.tagTypeRules = values[3];

			cardData.Add(cd);
		}
	}
	}

	IEnumerator RenderCards() {
		int fname = 0;

		while (cardData.Count > 0) {
			//respawn the cards
			foreach (Transform t in transform) {
				Destroy(t.gameObject);
			}

			for (int i = 0; i < 9; i++) {
				Instantiate(cardPrefab, transform);
			}

			yield return null;

			//fill the cards with data, and render them after 9 are done
			for (int i = 0; i < 9; i++) {
				CardData cd = cardData[0];

				cardData.RemoveAt(0);

				Card card = transform.GetChild(i).gameObject.GetComponent<Card>();

				card.eventRules = cd.eventRules;
				card.actionRules = cd.actionRules;
				card.complicationRules = cd.complicationRules;
				card.tagTypeRules = cd.tagTypeRules;

				card.Populate();

				if (cardData.Count == 0) {
					break;
				}
			}

			yield return null;

			PrintToFile(fname.ToString() + ".png");

			fname++;

			yield return null;
		}

		Debug.Log("finished");
	}

	void PrintToFile(string fname) {
		RenderTexture rt = new RenderTexture(width, height, 32, RenderTextureFormat.ARGB32);
		rt.Create();

		renderCamera.targetTexture = rt;

		renderCamera.Render();

		renderCamera.targetTexture = null;

		SaveTextureAsPNG(ToTexture2D(rt), fname);
	}

	Texture2D ToTexture2D(RenderTexture rTex) {
		Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		RenderTexture.active = rTex;
		tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
		tex.Apply();
		return tex;
	}

	void SaveTextureAsPNG(Texture2D _texture, string _fullPath) {
		byte[] _bytes =_texture.EncodeToPNG();
		System.IO.File.WriteAllBytes(_fullPath, _bytes);
		Debug.Log(_bytes.Length/1024  + "Kb was saved as: " + _fullPath);
	}
}
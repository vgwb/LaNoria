using UnityEngine;
using UnityEngine.UI;

public class SaveLoadItem : MonoBehaviour {

	public SaveLoadMenu Menu { get; set; }

	public string MapName {
		get => mapName;
		set {
			mapName = value;
			transform.GetChild(0).GetComponent<Text>().text = value;
		}
	}

	string mapName;

	public void Select () => Menu.SelectItem(mapName);
}
using UnityEngine;

public class NewMapMenu : MonoBehaviour {

	[SerializeField]
	HexGrid hexGrid;

	[SerializeField]
	HexMapGenerator mapGenerator;

	bool generateMaps = true;

	bool wrapping = true;

	public void ToggleMapGeneration (bool toggle) => generateMaps = toggle;

	public void ToggleWrapping (bool toggle) => wrapping = toggle;

	public void Open () {
		gameObject.SetActive(true);
		HexMapCamera.Locked = true;
	}

	public void Close () {
		gameObject.SetActive(false);
		HexMapCamera.Locked = false;
	}

	public void CreateSmallMap () => CreateMap(20, 15);

	public void CreateMediumMap () => CreateMap(40, 30);

	public void CreateLargeMap () => CreateMap(80, 60);

	void CreateMap (int x, int z) {
		if (generateMaps) {
			mapGenerator.GenerateMap(x, z, wrapping);
		}
		else {
			hexGrid.CreateMap(x, z, wrapping);
		}
		HexMapCamera.ValidatePosition();
		Close();
	}
}
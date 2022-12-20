using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HexCell : MonoBehaviour {

	public HexCoordinates Coordinates { get; set; }

	public RectTransform UIRect { get; set; }

	public HexGridChunk Chunk { get; set; }

	public int Index { get; set; }

	public int ColumnIndex { get; set; }

	public int Elevation {
		get => elevation;
		set {
			if (elevation == value) {
				return;
			}
			int originalViewElevation = ViewElevation;
			elevation = value;
			if (ViewElevation != originalViewElevation) {
				ShaderData.ViewElevationChanged();
			}
			RefreshPosition();
			ValidateRivers();

			for (int i = 0; i < roads.Length; i++) {
				if (roads[i] && GetElevationDifference((HexDirection)i) > 1) {
					SetRoad(i, false);
				}
			}

			Refresh();
		}
	}

	public int WaterLevel {
		get => waterLevel;
		set {
			if (waterLevel == value) {
				return;
			}
			int originalViewElevation = ViewElevation;
			waterLevel = value;
			if (ViewElevation != originalViewElevation) {
				ShaderData.ViewElevationChanged();
			}
			ValidateRivers();
			Refresh();
		}
	}

	public int ViewElevation => elevation >= waterLevel ? elevation : waterLevel;

	public bool IsUnderwater => waterLevel > elevation;

	public bool HasIncomingRiver => hasIncomingRiver;

	public bool HasOutgoingRiver => hasOutgoingRiver;

	public bool HasRiver => hasIncomingRiver || hasOutgoingRiver;

	public bool HasRiverBeginOrEnd => hasIncomingRiver != hasOutgoingRiver;

	public HexDirection RiverBeginOrEndDirection =>
		hasIncomingRiver ? incomingRiver : outgoingRiver;

	public bool HasRoads {
		get {
			for (int i = 0; i < roads.Length; i++) {
				if (roads[i]) {
					return true;
				}
			}
			return false;
		}
	}

	public HexDirection IncomingRiver => incomingRiver;

	public HexDirection OutgoingRiver => outgoingRiver;

	public Vector3 Position => transform.localPosition;

	public float StreamBedY =>
		(elevation + HexMetrics.streamBedElevationOffset) * HexMetrics.elevationStep;

	public float RiverSurfaceY =>
		(elevation + HexMetrics.waterElevationOffset) * HexMetrics.elevationStep;

	public float WaterSurfaceY =>
		(waterLevel + HexMetrics.waterElevationOffset) * HexMetrics.elevationStep;

	public int UrbanLevel {
		get => urbanLevel;
		set {
			if (urbanLevel != value) {
				urbanLevel = value;
				RefreshSelfOnly();
			}
		}
	}

	public int FarmLevel {
		get => farmLevel;
		set {
			if (farmLevel != value) {
				farmLevel = value;
				RefreshSelfOnly();
			}
		}
	}

	public int PlantLevel {
		get => plantLevel;
		set {
			if (plantLevel != value) {
				plantLevel = value;
				RefreshSelfOnly();
			}
		}
	}

	public int SpecialIndex {
		get => specialIndex;
		set {
			if (specialIndex != value && !HasRiver) {
				specialIndex = value;
				RemoveRoads();
				RefreshSelfOnly();
			}
		}
	}

	public bool IsSpecial => specialIndex > 0;

	public bool Walled {
		get => walled;
		set {
			if (walled != value) {
				walled = value;
				Refresh();
			}
		}
	}

	public int TerrainTypeIndex {
		get => terrainTypeIndex;
		set {
			if (terrainTypeIndex != value) {
				terrainTypeIndex = value;
				ShaderData.RefreshTerrain(this);
			}
		}
	}

	public bool IsVisible => visibility > 0 && Explorable;

	public bool IsExplored {
		get => explored && Explorable;
		private set => explored = value;
	}

	public bool Explorable { get; set; }

	public int Distance {
		get => distance;
		set => distance = value;
	}

	public HexUnit Unit { get; set; }

	public HexCell PathFrom { get; set; }

	public int SearchHeuristic { get; set; }

	public int SearchPriority => distance + SearchHeuristic;

	public int SearchPhase { get; set; }

	public HexCell NextWithSamePriority { get; set; }

	public HexCellShaderData ShaderData { get; set; }

	int terrainTypeIndex;

	int elevation = int.MinValue;
	int waterLevel;

	int urbanLevel, farmLevel, plantLevel;

	int specialIndex;

	int distance;

	int visibility;

	bool explored;

	bool walled;

	bool hasIncomingRiver, hasOutgoingRiver;
	HexDirection incomingRiver, outgoingRiver;

	[SerializeField]
	HexCell[] neighbors;

	[SerializeField]
	bool[] roads;

	public void IncreaseVisibility () {
		visibility += 1;
		if (visibility == 1) {
			IsExplored = true;
			ShaderData.RefreshVisibility(this);
		}
	}

	public void DecreaseVisibility () {
		visibility -= 1;
		if (visibility == 0) {
			ShaderData.RefreshVisibility(this);
		}
	}

	public void ResetVisibility () {
		if (visibility > 0) {
			visibility = 0;
			ShaderData.RefreshVisibility(this);
		}
	}

	public HexCell GetNeighbor (HexDirection direction) => neighbors[(int)direction];

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

	public HexEdgeType GetEdgeType (HexDirection direction) => HexMetrics.GetEdgeType(
		elevation, neighbors[(int)direction].elevation
	);

	public HexEdgeType GetEdgeType (HexCell otherCell) => HexMetrics.GetEdgeType(
		elevation, otherCell.elevation
	);

	public bool HasRiverThroughEdge (HexDirection direction) =>
		hasIncomingRiver && incomingRiver == direction ||
		hasOutgoingRiver && outgoingRiver == direction;

	public void RemoveIncomingRiver () {
		if (!hasIncomingRiver) {
			return;
		}
		hasIncomingRiver = false;
		RefreshSelfOnly();

		HexCell neighbor = GetNeighbor(incomingRiver);
		neighbor.hasOutgoingRiver = false;
		neighbor.RefreshSelfOnly();
	}

	public void RemoveOutgoingRiver () {
		if (!hasOutgoingRiver) {
			return;
		}
		hasOutgoingRiver = false;
		RefreshSelfOnly();

		HexCell neighbor = GetNeighbor(outgoingRiver);
		neighbor.hasIncomingRiver = false;
		neighbor.RefreshSelfOnly();
	}

	public void RemoveRiver () {
		RemoveOutgoingRiver();
		RemoveIncomingRiver();
	}

	public void SetOutgoingRiver (HexDirection direction) {
		if (hasOutgoingRiver && outgoingRiver == direction) {
			return;
		}

		HexCell neighbor = GetNeighbor(direction);
		if (!IsValidRiverDestination(neighbor)) {
			return;
		}

		RemoveOutgoingRiver();
		if (hasIncomingRiver && incomingRiver == direction) {
			RemoveIncomingRiver();
		}
		hasOutgoingRiver = true;
		outgoingRiver = direction;
		specialIndex = 0;

		neighbor.RemoveIncomingRiver();
		neighbor.hasIncomingRiver = true;
		neighbor.incomingRiver = direction.Opposite();
		neighbor.specialIndex = 0;

		SetRoad((int)direction, false);
	}

	public bool HasRoadThroughEdge (HexDirection direction) => roads[(int)direction];

	public void AddRoad (HexDirection direction) {
		if (
			!roads[(int)direction] && !HasRiverThroughEdge(direction) &&
			!IsSpecial && !GetNeighbor(direction).IsSpecial &&
			GetElevationDifference(direction) <= 1
		) {
			SetRoad((int)direction, true);
		}
	}

	public void RemoveRoads () {
		for (int i = 0; i < neighbors.Length; i++) {
			if (roads[i]) {
				SetRoad(i, false);
			}
		}
	}

	public int GetElevationDifference (HexDirection direction) {
		int difference = elevation - GetNeighbor(direction).elevation;
		return difference >= 0 ? difference : -difference;
	}

	bool IsValidRiverDestination (HexCell neighbor) =>
		neighbor && (elevation >= neighbor.elevation || waterLevel == neighbor.elevation);

	void ValidateRivers () {
		if (
			hasOutgoingRiver &&
			!IsValidRiverDestination(GetNeighbor(outgoingRiver))
		) {
			RemoveOutgoingRiver();
		}
		if (
			hasIncomingRiver &&
			!GetNeighbor(incomingRiver).IsValidRiverDestination(this)
		) {
			RemoveIncomingRiver();
		}
	}

	void SetRoad (int index, bool state) {
		roads[index] = state;
		neighbors[index].roads[(int)((HexDirection)index).Opposite()] = state;
		neighbors[index].RefreshSelfOnly();
		RefreshSelfOnly();
	}

	void RefreshPosition () {
		Vector3 position = transform.localPosition;
		position.y = elevation * HexMetrics.elevationStep;
		position.y +=
			(HexMetrics.SampleNoise(position).y * 2f - 1f) *
			HexMetrics.elevationPerturbStrength;
		transform.localPosition = position;

		Vector3 uiPosition = UIRect.localPosition;
		uiPosition.z = -position.y;
		UIRect.localPosition = uiPosition;
	}

	void Refresh () {
		if (Chunk) {
			Chunk.Refresh();
			for (int i = 0; i < neighbors.Length; i++) {
				HexCell neighbor = neighbors[i];
				if (neighbor != null && neighbor.Chunk != Chunk) {
					neighbor.Chunk.Refresh();
				}
			}
			if (Unit) {
				Unit.ValidateLocation();
			}
		}
	}

	void RefreshSelfOnly () {
		Chunk.Refresh();
		if (Unit) {
			Unit.ValidateLocation();
		}
	}

	public void Save (BinaryWriter writer) {
		writer.Write((byte)terrainTypeIndex);
		writer.Write((byte)(elevation + 127));
		writer.Write((byte)waterLevel);
		writer.Write((byte)urbanLevel);
		writer.Write((byte)farmLevel);
		writer.Write((byte)plantLevel);
		writer.Write((byte)specialIndex);
		writer.Write(walled);

		if (hasIncomingRiver) {
			writer.Write((byte)(incomingRiver + 128));
		}
		else {
			writer.Write((byte)0);
		}

		if (hasOutgoingRiver) {
			writer.Write((byte)(outgoingRiver + 128));
		}
		else {
			writer.Write((byte)0);
		}

		int roadFlags = 0;
		for (int i = 0; i < roads.Length; i++) {
			if (roads[i]) {
				roadFlags |= 1 << i;
			}
		}
		writer.Write((byte)roadFlags);
		writer.Write(IsExplored);
	}

	public void Load (BinaryReader reader, int header) {
		terrainTypeIndex = reader.ReadByte();
		ShaderData.RefreshTerrain(this);
		elevation = reader.ReadByte();
		if (header >= 4) {
			elevation -= 127;
		}
		RefreshPosition();
		waterLevel = reader.ReadByte();
		urbanLevel = reader.ReadByte();
		farmLevel = reader.ReadByte();
		plantLevel = reader.ReadByte();
		specialIndex = reader.ReadByte();
		walled = reader.ReadBoolean();

		byte riverData = reader.ReadByte();
		if (riverData >= 128) {
			hasIncomingRiver = true;
			incomingRiver = (HexDirection)(riverData - 128);
		}
		else {
			hasIncomingRiver = false;
		}

		riverData = reader.ReadByte();
		if (riverData >= 128) {
			hasOutgoingRiver = true;
			outgoingRiver = (HexDirection)(riverData - 128);
		}
		else {
			hasOutgoingRiver = false;
		}

		int roadFlags = reader.ReadByte();
		for (int i = 0; i < roads.Length; i++) {
			roads[i] = (roadFlags & (1 << i)) != 0;
		}

		IsExplored = header >= 3 ? reader.ReadBoolean() : false;
		ShaderData.RefreshVisibility(this);
	}

	public void SetLabel (string text) {
		UnityEngine.UI.Text label = UIRect.GetComponent<Text>();
		label.text = text;
	}

	public void DisableHighlight () {
		Image highlight = UIRect.GetChild(0).GetComponent<Image>();
		highlight.enabled = false;
	}

	public void EnableHighlight (Color color) {
		Image highlight = UIRect.GetChild(0).GetComponent<Image>();
		highlight.color = color;
		highlight.enabled = true;
	}

	public void SetMapData (float data) => ShaderData.SetMapData(this, data);
}
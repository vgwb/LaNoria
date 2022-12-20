using System.Collections.Generic;
using UnityEngine;

public class HexMapGenerator : MonoBehaviour {

	[SerializeField]
	HexGrid grid;

	[SerializeField]
	bool useFixedSeed;

	[SerializeField]
	int seed;

	[SerializeField, Range(0f, 0.5f)]
	float jitterProbability = 0.25f;

	[SerializeField, Range(20, 200)]
	int chunkSizeMin = 30;

	[SerializeField, Range(20, 200)]
	int chunkSizeMax = 100;

	[SerializeField, Range(0f, 1f)]
	float highRiseProbability = 0.25f;

	[SerializeField, Range(0f, 0.4f)]
	float sinkProbability = 0.2f;

	[SerializeField, Range(5, 95)]
	int landPercentage = 50;

	[SerializeField, Range(1, 5)]
	int waterLevel = 3;

	[SerializeField, Range(-4, 0)]
	int elevationMinimum = -2;

	[SerializeField, Range(6, 10)]
	int elevationMaximum = 8;

	[SerializeField, Range(0, 10)]
	int mapBorderX = 5;

	[SerializeField, Range(0, 10)]
	int mapBorderZ = 5;

	[SerializeField, Range(0, 10)]
	int regionBorder = 5;

	[SerializeField, Range(1, 4)]
	int regionCount = 1;

	[SerializeField, Range(0, 100)]
	int erosionPercentage = 50;

	[SerializeField, Range(0f, 1f)]
	float startingMoisture = 0.1f;

	[SerializeField, Range(0f, 1f)]
	float evaporationFactor = 0.5f;

	[SerializeField, Range(0f, 1f)]
	float precipitationFactor = 0.25f;

	[SerializeField, Range(0f, 1f)]
	float runoffFactor = 0.25f;

	[SerializeField, Range(0f, 1f)]
	float seepageFactor = 0.125f;

	[SerializeField]
	HexDirection windDirection = HexDirection.NW;

	[SerializeField, Range(1f, 10f)]
	float windStrength = 4f;

	[SerializeField, Range(0, 20)]
	int riverPercentage = 10;

	[SerializeField, Range(0f, 1f)]
	float extraLakeProbability = 0.25f;

	[SerializeField, Range(0f, 1f)]
	float lowTemperature = 0f;

	[SerializeField, Range(0f, 1f)]
	float highTemperature = 1f;

	public enum HemisphereMode {
		Both, North, South
	}

	[SerializeField]
	HemisphereMode hemisphere;

	[SerializeField, Range(0f, 1f)]
	float temperatureJitter = 0.1f;

	HexCellPriorityQueue searchFrontier;

	int searchFrontierPhase;

	int cellCount, landCells;

	int temperatureJitterChannel;

	struct MapRegion {
		public int xMin, xMax, zMin, zMax;
	}

	List<MapRegion> regions;

	struct ClimateData {
		public float clouds, moisture;
	}

	List<ClimateData> climate = new List<ClimateData>();
	List<ClimateData> nextClimate = new List<ClimateData>();

	List<HexDirection> flowDirections = new List<HexDirection>();

	struct Biome {
		public int terrain, plant;

		public Biome (int terrain, int plant) {
			this.terrain = terrain;
			this.plant = plant;
		}
	}

	static float[] temperatureBands = { 0.1f, 0.3f, 0.6f };

	static float[] moistureBands = { 0.12f, 0.28f, 0.85f };

	static Biome[] biomes = {
		new Biome(0, 0), new Biome(4, 0), new Biome(4, 0), new Biome(4, 0),
		new Biome(0, 0), new Biome(2, 0), new Biome(2, 1), new Biome(2, 2),
		new Biome(0, 0), new Biome(1, 0), new Biome(1, 1), new Biome(1, 2),
		new Biome(0, 0), new Biome(1, 1), new Biome(1, 2), new Biome(1, 3)
	};

	public void GenerateMap (int x, int z, bool wrapping) {
		Random.State originalRandomState = Random.state;
		if (!useFixedSeed) {
			seed = Random.Range(0, int.MaxValue);
			seed ^= (int)System.DateTime.Now.Ticks;
			seed ^= (int)Time.unscaledTime;
			seed &= int.MaxValue;
		}
		Random.InitState(seed);

		cellCount = x * z;
		grid.CreateMap(x, z, wrapping);
		if (searchFrontier == null) {
			searchFrontier = new HexCellPriorityQueue();
		}
		for (int i = 0; i < cellCount; i++) {
			grid.GetCell(i).WaterLevel = waterLevel;
		}
		CreateRegions();
		CreateLand();
		ErodeLand();
		CreateClimate();
		CreateRivers();
		SetTerrainType();
		for (int i = 0; i < cellCount; i++) {
			grid.GetCell(i).SearchPhase = 0;
		}

		Random.state = originalRandomState;
	}

	void CreateRegions () {
		if (regions == null) {
			regions = new List<MapRegion>();
		}
		else {
			regions.Clear();
		}

		int borderX = grid.Wrapping ? regionBorder : mapBorderX;
		MapRegion region;
		switch (regionCount) {
		default:
			if (grid.Wrapping) {
				borderX = 0;
			}
			region.xMin = borderX;
			region.xMax = grid.CellCountX - borderX;
			region.zMin = mapBorderZ;
			region.zMax = grid.CellCountZ - mapBorderZ;
			regions.Add(region);
			break;
		case 2:
			if (Random.value < 0.5f) {
				region.xMin = borderX;
				region.xMax = grid.CellCountX / 2 - regionBorder;
				region.zMin = mapBorderZ;
				region.zMax = grid.CellCountZ - mapBorderZ;
				regions.Add(region);
				region.xMin = grid.CellCountX / 2 + regionBorder;
				region.xMax = grid.CellCountX - borderX;
				regions.Add(region);
			}
			else {
				if (grid.Wrapping) {
					borderX = 0;
				}
				region.xMin = borderX;
				region.xMax = grid.CellCountX - borderX;
				region.zMin = mapBorderZ;
				region.zMax = grid.CellCountZ / 2 - regionBorder;
				regions.Add(region);
				region.zMin = grid.CellCountZ / 2 + regionBorder;
				region.zMax = grid.CellCountZ - mapBorderZ;
				regions.Add(region);
			}
			break;
		case 3:
			region.xMin = borderX;
			region.xMax = grid.CellCountX / 3 - regionBorder;
			region.zMin = mapBorderZ;
			region.zMax = grid.CellCountZ - mapBorderZ;
			regions.Add(region);
			region.xMin = grid.CellCountX / 3 + regionBorder;
			region.xMax = grid.CellCountX * 2 / 3 - regionBorder;
			regions.Add(region);
			region.xMin = grid.CellCountX * 2 / 3 + regionBorder;
			region.xMax = grid.CellCountX - borderX;
			regions.Add(region);
			break;
		case 4:
			region.xMin = borderX;
			region.xMax = grid.CellCountX / 2 - regionBorder;
			region.zMin = mapBorderZ;
			region.zMax = grid.CellCountZ / 2 - regionBorder;
			regions.Add(region);
			region.xMin = grid.CellCountX / 2 + regionBorder;
			region.xMax = grid.CellCountX - borderX;
			regions.Add(region);
			region.zMin = grid.CellCountZ / 2 + regionBorder;
			region.zMax = grid.CellCountZ - mapBorderZ;
			regions.Add(region);
			region.xMin = borderX;
			region.xMax = grid.CellCountX / 2 - regionBorder;
			regions.Add(region);
			break;
		}
	}

	void CreateLand () {
		int landBudget = Mathf.RoundToInt(cellCount * landPercentage * 0.01f);
		landCells = landBudget;
		for (int guard = 0; guard < 10000; guard++) {
			bool sink = Random.value < sinkProbability;
			for (int i = 0; i < regions.Count; i++) {
				MapRegion region = regions[i];
				int chunkSize = Random.Range(chunkSizeMin, chunkSizeMax - 1);
					if (sink) {
					landBudget = SinkTerrain(chunkSize, landBudget, region);
				}
				else {
					landBudget = RaiseTerrain(chunkSize, landBudget, region);
					if (landBudget == 0) {
						return;
					}
				}
			}
		}
		if (landBudget > 0) {
			Debug.LogWarning("Failed to use up " + landBudget + " land budget.");
			landCells -= landBudget;
		}
	}

	int RaiseTerrain (int chunkSize, int budget, MapRegion region) {
		searchFrontierPhase += 1;
		HexCell firstCell = GetRandomCell(region);
		firstCell.SearchPhase = searchFrontierPhase;
		firstCell.Distance = 0;
		firstCell.SearchHeuristic = 0;
		searchFrontier.Enqueue(firstCell);
		HexCoordinates center = firstCell.Coordinates;

		int rise = Random.value < highRiseProbability ? 2 : 1;
		int size = 0;
		while (size < chunkSize && searchFrontier.Count > 0) {
			HexCell current = searchFrontier.Dequeue();
			int originalElevation = current.Elevation;
			int newElevation = originalElevation + rise;
			if (newElevation > elevationMaximum) {
				continue;
			}
			current.Elevation = newElevation;
			if (
				originalElevation < waterLevel &&
				newElevation >= waterLevel && --budget == 0
			) {
				break;
			}
			size += 1;

			for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
				HexCell neighbor = current.GetNeighbor(d);
				if (neighbor && neighbor.SearchPhase < searchFrontierPhase) {
					neighbor.SearchPhase = searchFrontierPhase;
					neighbor.Distance = neighbor.Coordinates.DistanceTo(center);
					neighbor.SearchHeuristic =
						Random.value < jitterProbability ? 1: 0;
					searchFrontier.Enqueue(neighbor);
				}
			}
		}
		searchFrontier.Clear();
		return budget;
	}

	int SinkTerrain (int chunkSize, int budget, MapRegion region) {
		searchFrontierPhase += 1;
		HexCell firstCell = GetRandomCell(region);
		firstCell.SearchPhase = searchFrontierPhase;
		firstCell.Distance = 0;
		firstCell.SearchHeuristic = 0;
		searchFrontier.Enqueue(firstCell);
		HexCoordinates center = firstCell.Coordinates;

		int sink = Random.value < highRiseProbability ? 2 : 1;
		int size = 0;
		while (size < chunkSize && searchFrontier.Count > 0) {
			HexCell current = searchFrontier.Dequeue();
			int originalElevation = current.Elevation;
			int newElevation = current.Elevation - sink;
			if (newElevation < elevationMinimum) {
				continue;
			}
			current.Elevation = newElevation;
			if (
				originalElevation >= waterLevel &&
				newElevation < waterLevel
			) {
				budget += 1;
			}
			size += 1;

			for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
				HexCell neighbor = current.GetNeighbor(d);
				if (neighbor && neighbor.SearchPhase < searchFrontierPhase) {
					neighbor.SearchPhase = searchFrontierPhase;
					neighbor.Distance = neighbor.Coordinates.DistanceTo(center);
					neighbor.SearchHeuristic =
						Random.value < jitterProbability ? 1: 0;
					searchFrontier.Enqueue(neighbor);
				}
			}
		}
		searchFrontier.Clear();
		return budget;
	}

	void ErodeLand () {
		List<HexCell> erodibleCells = ListPool<HexCell>.Get();
		for (int i = 0; i < cellCount; i++) {
			HexCell cell = grid.GetCell(i);
			if (IsErodible(cell)) {
				erodibleCells.Add(cell);
			}
		}

		int targetErodibleCount =
			(int)(erodibleCells.Count * (100 - erosionPercentage) * 0.01f);
		
		while (erodibleCells.Count > targetErodibleCount) {
			int index = Random.Range(0, erodibleCells.Count);
			HexCell cell = erodibleCells[index];
			HexCell targetCell = GetErosionTarget(cell);

			cell.Elevation -= 1;
			targetCell.Elevation += 1;

			if (!IsErodible(cell)) {
				erodibleCells[index] = erodibleCells[erodibleCells.Count - 1];
				erodibleCells.RemoveAt(erodibleCells.Count - 1);
			}

			for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
				HexCell neighbor = cell.GetNeighbor(d);
				if (
					neighbor && neighbor.Elevation == cell.Elevation + 2 &&
					!erodibleCells.Contains(neighbor)
				) {
					erodibleCells.Add(neighbor);
				}
			}

			if (IsErodible(targetCell) && !erodibleCells.Contains(targetCell)) {
				erodibleCells.Add(targetCell);
			}

			for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
				HexCell neighbor = targetCell.GetNeighbor(d);
				if (
					neighbor && neighbor != cell &&
					neighbor.Elevation == targetCell.Elevation + 1 &&
					!IsErodible(neighbor)
				) {
					erodibleCells.Remove(neighbor);
				}
			}
		}

		ListPool<HexCell>.Add(erodibleCells);
	}

	bool IsErodible (HexCell cell) {
		int erodibleElevation = cell.Elevation - 2;
		for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
			HexCell neighbor = cell.GetNeighbor(d);
			if (neighbor && neighbor.Elevation <= erodibleElevation) {
				return true;
			}
		}
		return false;
	}

	HexCell GetErosionTarget (HexCell cell) {
		List<HexCell> candidates = ListPool<HexCell>.Get();
		int erodibleElevation = cell.Elevation - 2;
		for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
			HexCell neighbor = cell.GetNeighbor(d);
			if (neighbor && neighbor.Elevation <= erodibleElevation) {
				candidates.Add(neighbor);
			}
		}
		HexCell target = candidates[Random.Range(0, candidates.Count)];
		ListPool<HexCell>.Add(candidates);
		return target;
	}

	void CreateClimate () {
		climate.Clear();
		nextClimate.Clear();
		ClimateData initialData = new ClimateData();
		initialData.moisture = startingMoisture;
		ClimateData clearData = new ClimateData();
		for (int i = 0; i < cellCount; i++) {
			climate.Add(initialData);
			nextClimate.Add(clearData);
		}

		for (int cycle = 0; cycle < 40; cycle++) {
			for (int i = 0; i < cellCount; i++) {
				EvolveClimate(i);
			}
			List<ClimateData> swap = climate;
			climate = nextClimate;
			nextClimate = swap;
		}
	}

	void EvolveClimate (int cellIndex) {
		HexCell cell = grid.GetCell(cellIndex);
		ClimateData cellClimate = climate[cellIndex];

		if (cell.IsUnderwater) {
			cellClimate.moisture = 1f;
			cellClimate.clouds += evaporationFactor;
		}
		else {
			float evaporation = cellClimate.moisture * evaporationFactor;
			cellClimate.moisture -= evaporation;
			cellClimate.clouds += evaporation;
		}

		float precipitation = cellClimate.clouds * precipitationFactor;
		cellClimate.clouds -= precipitation;
		cellClimate.moisture += precipitation;

		float cloudMaximum = 1f - cell.ViewElevation / (elevationMaximum + 1f);
		if (cellClimate.clouds > cloudMaximum) {
			cellClimate.moisture += cellClimate.clouds - cloudMaximum;
			cellClimate.clouds = cloudMaximum;
		}

		HexDirection mainDispersalDirection = windDirection.Opposite();
		float cloudDispersal = cellClimate.clouds * (1f / (5f + windStrength));
		float runoff = cellClimate.moisture * runoffFactor * (1f / 6f);
		float seepage = cellClimate.moisture * seepageFactor * (1f / 6f);
		for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
			HexCell neighbor = cell.GetNeighbor(d);
			if (!neighbor) {
				continue;
			}
			ClimateData neighborClimate = nextClimate[neighbor.Index];
			if (d == mainDispersalDirection) {
				neighborClimate.clouds += cloudDispersal * windStrength;
			}
			else {
				neighborClimate.clouds += cloudDispersal;
			}

			int elevationDelta = neighbor.ViewElevation - cell.ViewElevation;
			if (elevationDelta < 0) {
				cellClimate.moisture -= runoff;
				neighborClimate.moisture += runoff;
			}
			else if (elevationDelta == 0) {
				cellClimate.moisture -= seepage;
				neighborClimate.moisture += seepage;
			}

			nextClimate[neighbor.Index] = neighborClimate;
		}

		ClimateData nextCellClimate = nextClimate[cellIndex];
		nextCellClimate.moisture += cellClimate.moisture;
		if (nextCellClimate.moisture > 1f) {
			nextCellClimate.moisture = 1f;
		}
		nextClimate[cellIndex] = nextCellClimate;
		climate[cellIndex] = new ClimateData();
	}

	void CreateRivers () {
		List<HexCell> riverOrigins = ListPool<HexCell>.Get();
		for (int i = 0; i < cellCount; i++) {
			HexCell cell = grid.GetCell(i);
			if (cell.IsUnderwater) {
				continue;
			}
			ClimateData data = climate[i];
			float weight =
				data.moisture * (cell.Elevation - waterLevel) /
				(elevationMaximum - waterLevel);
			if (weight > 0.75f) {
				riverOrigins.Add(cell);
				riverOrigins.Add(cell);
			}
			if (weight > 0.5f) {
				riverOrigins.Add(cell);
			}
			if (weight > 0.25f) {
				riverOrigins.Add(cell);
			}
		}

		int riverBudget = Mathf.RoundToInt(landCells * riverPercentage * 0.01f);
		while (riverBudget > 0 && riverOrigins.Count > 0) {
			int index = Random.Range(0, riverOrigins.Count);
			int lastIndex = riverOrigins.Count - 1;
			HexCell origin = riverOrigins[index];
			riverOrigins[index] = riverOrigins[lastIndex];
			riverOrigins.RemoveAt(lastIndex);

			if (!origin.HasRiver) {
				bool isValidOrigin = true;
				for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
					HexCell neighbor = origin.GetNeighbor(d);
					if (neighbor && (neighbor.HasRiver || neighbor.IsUnderwater)) {
						isValidOrigin = false;
						break;
					}
				}
				if (isValidOrigin) {
					riverBudget -= CreateRiver(origin);
				}
			}
		}

		if (riverBudget > 0) {
			Debug.LogWarning("Failed to use up river budget.");
		}

		ListPool<HexCell>.Add(riverOrigins);
	}

	int CreateRiver (HexCell origin) {
		int length = 1;
		HexCell cell = origin;
		HexDirection direction = HexDirection.NE;
		while (!cell.IsUnderwater) {
			int minNeighborElevation = int.MaxValue;
			flowDirections.Clear();
			for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
				HexCell neighbor = cell.GetNeighbor(d);
				if (!neighbor) {
					continue;
				}

				if (neighbor.Elevation < minNeighborElevation) {
					minNeighborElevation = neighbor.Elevation;
				}

				if (neighbor == origin || neighbor.HasIncomingRiver) {
					continue;
				}

				int delta = neighbor.Elevation - cell.Elevation;
				if (delta > 0) {
					continue;
				}

				if (neighbor.HasOutgoingRiver) {
					cell.SetOutgoingRiver(d);
					return length;
				}

				if (delta < 0) {
					flowDirections.Add(d);
					flowDirections.Add(d);
					flowDirections.Add(d);
				}
				if (
					length == 1 ||
					(d != direction.Next2() && d != direction.Previous2())
				) {
					flowDirections.Add(d);
				}
				flowDirections.Add(d);
			}

			if (flowDirections.Count == 0) {
				if (length == 1) {
					return 0;
				}

				if (minNeighborElevation >= cell.Elevation) {
					cell.WaterLevel = minNeighborElevation;
					if (minNeighborElevation == cell.Elevation) {
						cell.Elevation = minNeighborElevation - 1;
					}
				}
				break;
			}

			direction = flowDirections[Random.Range(0, flowDirections.Count)];
			cell.SetOutgoingRiver(direction);
			length += 1;

			if (
				minNeighborElevation >= cell.Elevation &&
				Random.value < extraLakeProbability
			) {
				cell.WaterLevel = cell.Elevation;
				cell.Elevation -= 1;
			}

			cell = cell.GetNeighbor(direction);
		}
		return length;
	}

	void SetTerrainType () {
		temperatureJitterChannel = Random.Range(0, 4);
		int rockDesertElevation =
			elevationMaximum - (elevationMaximum - waterLevel) / 2;
		
		for (int i = 0; i < cellCount; i++) {
			HexCell cell = grid.GetCell(i);
			float temperature = DetermineTemperature(cell);
			float moisture = climate[i].moisture;
			if (!cell.IsUnderwater) {
				int t = 0;
				for (; t < temperatureBands.Length; t++) {
					if (temperature < temperatureBands[t]) {
						break;
					}
				}
				int m = 0;
				for (; m < moistureBands.Length; m++) {
					if (moisture < moistureBands[m]) {
						break;
					}
				}
				Biome cellBiome = biomes[t * 4 + m];

				if (cellBiome.terrain == 0) {
					if (cell.Elevation >= rockDesertElevation) {
						cellBiome.terrain = 3;
					}
				}
				else if (cell.Elevation == elevationMaximum) {
					cellBiome.terrain = 4;
				}

				if (cellBiome.terrain == 4) {
					cellBiome.plant = 0;
				}
				else if (cellBiome.plant < 3 && cell.HasRiver) {
					cellBiome.plant += 1;
				}

				cell.TerrainTypeIndex = cellBiome.terrain;
				cell.PlantLevel = cellBiome.plant;
			}
			else {
				int terrain;
				if (cell.Elevation == waterLevel - 1) {
					int cliffs = 0, slopes = 0;
					for (
						HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++
					) {
						HexCell neighbor = cell.GetNeighbor(d);
						if (!neighbor) {
							continue;
						}
						int delta = neighbor.Elevation - cell.WaterLevel;
						if (delta == 0) {
							slopes += 1;
						}
						else if (delta > 0) {
							cliffs += 1;
						}
					}

					if (cliffs + slopes > 3) {
						terrain = 1;
					}
					else if (cliffs > 0) {
						terrain = 3;
					}
					else if (slopes > 0) {
						terrain = 0;
					}
					else {
						terrain = 1;
					}
				}
				else if (cell.Elevation >= waterLevel) {
					terrain = 1;
				}
				else if (cell.Elevation < 0) {
					terrain = 3;
				}
				else {
					terrain = 2;
				}

				if (terrain == 1 && temperature < temperatureBands[0]) {
					terrain = 2;
				}
				cell.TerrainTypeIndex = terrain;
			}
		}
	}

	float DetermineTemperature (HexCell cell) {
		float latitude = (float)cell.Coordinates.Z / grid.CellCountZ;
		if (hemisphere == HemisphereMode.Both) {
			latitude *= 2f;
			if (latitude > 1f) {
				latitude = 2f - latitude;
			}
		}
		else if (hemisphere == HemisphereMode.North) {
			latitude = 1f - latitude;
		}

		float temperature =
			Mathf.LerpUnclamped(lowTemperature, highTemperature, latitude);

		temperature *= 1f - (cell.ViewElevation - waterLevel) /
			(elevationMaximum - waterLevel + 1f);

		float jitter =
			HexMetrics.SampleNoise(cell.Position * 0.1f)[temperatureJitterChannel];

		temperature += (jitter * 2f - 1f) * temperatureJitter;

		return temperature;
	}

	HexCell GetRandomCell (MapRegion region) {
		return grid.GetCell(
			Random.Range(region.xMin, region.xMax),
			Random.Range(region.zMin, region.zMax)
		);
	}
}
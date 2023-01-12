using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using vgwb;

public class GridManager : SingletonMonoB<GridManager>
{
    #region Struct

    [System.Serializable]
    public class CellInfo
    {
        public bool Occupied;
        public GameObject CellObj;
        public Vector3 Position;

        public CellInfo(bool occupied, GameObject cellObj, Vector3 pos)
        {
            Occupied = occupied;
            CellObj = cellObj;
            Position = pos;
        }
    }
    #endregion

    #region Var
    public List<CellInfo> Cells;
    #endregion

    #region MonoB
    protected override void Awake()
    {
        base.Awake();

        InitCells();
    }

    void Update()
    {

    }
    #endregion

    #region Functions
    private void InitCells()
    {
        int childs = transform.childCount;
        for (int i = 0; i < childs; i++) {
            var obj = transform.GetChild(i).gameObject;
            var box = obj.AddComponent<BoxCollider>();
            box.center = new Vector3(0.0f, 0.5f, 0.0f);
            Vector3 hexPos = RetrieveHexPos(obj.transform.position);
            CellInfo info = new CellInfo(i == 0 ? true : false, obj, hexPos);
            Cells.Add(info);
        }
    }

    public bool IsCellOccupiedByPos(Vector3 pos)
    {
        Vector3 hexPos = RetrieveHexPos(pos);
        var cell = Cells.Find(x => x.Position == hexPos);
        if (cell != null) {
            return cell.Occupied;
        }

        return true; // outside of the map!!!
    }

    public bool IsOutOfMap(Vector3 pos)
    {
        Vector3 hexPos = RetrieveHexPos(pos);
        var cell = Cells.Find(x => x.Position == hexPos);

        return cell == null;
    }

    public void SetCellAsOccupiedByPosition(Vector3 pos)
    {
        Vector3 hexPos = RetrieveHexPos(pos);
        var cell = Cells.Find(x => x.Position == hexPos);
        if (cell != null) {
            cell.Occupied = true;
        }
    }

    public Vector3 ClosestBorderPoint(Vector3 pos)
    {
        Vector3 result = Vector3.negativeInfinity;
        float distance = float.PositiveInfinity;
        foreach (var cell in Cells) {
            float d = Vector3.Distance(pos, cell.Position);
            if (d < distance) {
                distance = d;
                result = cell.Position;
            }
        }

        return result;
    }

    private Vector3 RetrieveHexPos(Vector3 pos)
    {
        var hex = HexUtils.FromWorld(pos);
        return hex.ToWorld(0f);
    }
    #endregion
}

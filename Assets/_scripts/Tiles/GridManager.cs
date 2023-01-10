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
    public HexUtils hex => HexUtils.FromWorld(transform.position);
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
            var hex = HexUtils.FromWorld(obj.transform.position);
            Vector3 hexPos = hex.ToWorld(0f);
            CellInfo info = new CellInfo(i == 0 ? true : false, obj, hexPos);
            Cells.Add(info);
        }
    }

    public bool IsCellOccupiedByPos(Vector3 pos)
    {
        var hex = HexUtils.FromWorld(pos);
        Vector3 hexPos = hex.ToWorld(0f);
        var cell = Cells.Find(x => x.Position == hexPos);
        if (cell != null) {
            return cell.Occupied;
        }

        return false;
    }

    public void SetCellAsOccupiedByPosition(Vector3 pos)
    {
        var hex = HexUtils.FromWorld(pos);
        Vector3 hexPos = hex.ToWorld(0f);
        var cell = Cells.Find(x => x.Position == hexPos);
        if (cell != null) {
            cell.Occupied = true;
        }
    }
    #endregion
}

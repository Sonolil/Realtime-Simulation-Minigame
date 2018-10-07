using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Manager : MonoBehaviour {
    public Infra infra;
    public Unit unit;

    private List<Infra> infras;
    private List<Unit> units;

    private Infra _selectInfra;
    private HexTile _selectTile;


    void Update () {
        if (_selectInfra)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                HexTile tile = hit.collider.GetComponentInParent<HexTile>();

                if (tile)
                {
                    if (_selectTile == tile)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                           if (_selectTile.colony[1])
                                BuildInfra(_selectInfra, _selectTile, 1);
                            else if (_selectTile.colony[2])
                                BuildInfra(_selectInfra, _selectTile, 2);
                            else if (_selectTile.colony[0])
                                BuildInfra(_selectInfra, _selectTile, 0);
                        }
                        return;
                    }

                    else if (_selectTile)
                    {
                        _selectTile.RemoveColor(Color.red);
                        _selectTile = null;
                    }

                    if (tile.infra)
                        return;
                    for (int i = 0; i < tile.nears.Count; i++)
                    {
                        if (tile.nears[i].infra)
                        {
                            if (tile.nears[i].infra.team == _selectInfra.team)
                                return;
                        }
                    }
                    _selectTile = tile;
                    _selectTile.AddColor(Color.red);
                }
            }
        }
    }

    public void Select(GameObject infra)
    {
        _selectInfra = infra.GetComponent<Infra>();
    }
    
    public void BuildInfra(Infra infra, HexTile curr, int team)
    {
        infra.curr = curr;
        infra.team = team;
        //infra.gameObject.SetActive(false);
        Instantiate(infra);
    }

    public void SetColor(Infra infra)
    {
        if (infra.team == 1)
            infra.SetColor(Color.yellow);

        else if (infra.team == 2)
            infra.SetColor(Color.cyan);
    }

    public void MakeRed(Infra infra)
    {
        infra.SetColor(Color.red);
    }

}

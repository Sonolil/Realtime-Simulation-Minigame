using System.Collections.Generic;

[System.Serializable]
public class HexFinder {
    
    public bool found = false;
    public List<HexTile> path = new List<HexTile>();

    private List<HexTile> _obstacles = new List<HexTile>(); 
    private HexTile _next;
    private HexTile _dest;
    private Unit _unit;


    private List<HexTile> visited = new List<HexTile>();
    private Dictionary<HexTile, HexTile> links = new Dictionary<HexTile, HexTile>();

    public void Init(Unit unit)
    {
        _unit = unit;
    }

    public void Reset()
    {
        _dest = null;
        _next = null;

        found = false;
        visited.Clear();
        links.Clear();
        path.Clear();
    }

    public bool CheckTarget()
    {
        if (!_dest)
            return false; 

        if (_dest.infra == _unit.target)
        {
            return true;
        } 
        
        return false;
    }

    public void SetTemporaryObstacle(HexTile obstacle)
    {
        _obstacles.Add(obstacle);
        if (_obstacles.Count > 20)
            _obstacles.RemoveAt(0);
    }

    public void ClearObstacles()
    {
        _obstacles.Clear();
    }

    public HexTile Search()
    {
        int step = 0;
        visited.Add(_unit.curr);
        while (!found)
        {
            if (visited.Count <= step)
            {
                return null;
            }

            else
            {
                CheckNears(visited[step]);
                step++;
            }
        }

        Pave();
        path.Reverse();
        return _dest;
    }

    private void CheckNears(HexTile step)
    {
        step.Shuffle();
        for (int i = 0; i < step.nears.Count; i++)
        {
            HexTile near = step.nears[i];
            Infra target = near.infra;

            if (!near.pass)
                continue;
            
            if (target)
            {
                if (target.team != _unit.team
                    && target.alive)
                {
                    _dest = near;
                    _next = step;
                    found = true;
                    return;
                }

                else if (target is Unit)
                {
                    if (_obstacles.Contains(near))
                        continue;
                }

                else continue;
            }

            if (!visited.Contains(near))
                visited.Add(near);

            if (!links.ContainsKey(near))
                links.Add(near, step);
        }
    }
    
    private void Pave()
    {
        path.Add(_next);

        foreach (HexTile near in _next.nears)
        {
            if (links[_next] == near)
            {
                if (near != _unit.curr)
                {
                    _next = near;
                    Pave();
                }

                else break;
            }
        }
    }
}

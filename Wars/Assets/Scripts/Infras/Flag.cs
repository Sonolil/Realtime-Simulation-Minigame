using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flag : Infra {
    protected List<HexTile> _grounds = new List<HexTile>();

    protected override IEnumerator Active()
    {
        yield return null;
    }

    protected override void Init()
    {
        base.Init();
        Colonize();
    }

    protected void Colonize()
    {
        curr.Colonize(team);
        for (int i = 0; i < curr.nears.Count; i++)
        {
            curr.nears[i].Colonize(team);
            _grounds.Add(curr.nears[i]);
        }

        for (int j = 0; j < _grounds.Count; j++)
        {
            for (int k = 0; k < _grounds[j].nears.Count; k++)
            {
                _grounds[j].nears[k].Colonize(team);
            }
            
        }
    }

    protected override IEnumerator Die()
    {
        //Dying Animation
        _mat.SetColor("_Color", Color.black);
        yield return new WaitForSeconds(0.5f);

        //Destroy Object
        curr.Decolonize(team);
        for (int i = 0; i < curr.nears.Count; i++)
        {
            curr.nears[i].Decolonize(team);
            _grounds.Clear();
        }

        curr.infra = null;
        curr = null;
        _tr = null;
        _mat = null;

        Destroy(this.gameObject);
    }

}

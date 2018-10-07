using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : Infra {
    public int ATT = 15;
    public Infra target;
    public HexTile next;
    public HexFinder finder;

    new public void Start()
    {
        Init();
        StartCoroutine("Incarnate");
    }
    

    protected override void Init()
    {
        base.Init();
        finder.Init(this);
    }

    override protected IEnumerator Idle()
    {
        finder.Reset();
        yield return null;
        StartCoroutine("Trace");
    }
    
    protected IEnumerator Trace()
    {
        for (int i = 0; i < curr.nears.Count; i++)
        {
            Infra temp = curr.nears[i].infra;
            if (temp && temp.team != team)
            {
                target = temp;
                StartCoroutine("Work");
                yield break;
            }
        }

        if (!target || !finder.CheckTarget())
        {
            finder.Reset();
            HexTile tile = finder.Search();
            if (!tile) {
                StartCoroutine("Idle");
                yield break;
            }  else target = tile.infra;
        }

        StartCoroutine("Move");
    }
    
    protected IEnumerator Move()
    {
        next = finder.path[0];
        if (next.infra)
        {
            finder.SetTemporaryObstacle(next);
            StartCoroutine("Idle");
            yield break;
        }

        curr.infra = null;
        next.infra = this;
        Transform tempTr = next.transform;
        while (_tr.position != tempTr.position)
        {
            yield return null;
            _tr.position = Vector3.MoveTowards(_tr.position, tempTr.position, Time.deltaTime * 2.0f);
        }

        curr = next;
        next = null;
        finder.path.RemoveAt(0);
        StartCoroutine("Trace");
    }

    protected virtual IEnumerator Work()
    {
        while (target.alive)
        {
            yield return new WaitForSeconds(0.3f);
            target.Hit(ATT);
        }
        
        target = null;
        StartCoroutine("Idle");
    }
}

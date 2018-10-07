using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Infra : MonoBehaviour
{
    public bool alive = false;
    public HexTile curr;
    public int team;
    public float MaxHP = 100;
    public float HP = 100;
    public int DEF;

    protected Transform _tr;
    protected Material _mat;
    protected Manager _manager;
    protected HPBar _bar;

    public void Start()
    {
        Init();
        StartCoroutine("Incarnate");
    }
    
    protected virtual void Init()
    {
        _bar = GetComponentInChildren<HPBar>();
        _mat = GetComponentInChildren<Renderer>().material;
        _manager = GameObject.FindWithTag("Manager").GetComponent<Manager>();
        _tr = transform;
        _tr.position = curr.transform.position;
        curr.infra = this;
        SetColor(Color.white);
    }

    public void SetColor(Color newColor)
    {
        _mat.color = newColor;
    }

    protected IEnumerator Incarnate()
    {
        //Incarnate Animation
        yield return new WaitForSeconds(0.3f);
        _manager.SetColor(this);
        alive = true;
        StartCoroutine("Idle");
    }

    protected virtual IEnumerator Idle()
    {
        yield return new WaitForSeconds(0.3f);
        StartCoroutine("Active");
    }

    protected virtual IEnumerator Active()
    {
        while(alive)
        {
            yield return new WaitForSeconds(2.0f);
            curr.Shuffle();
            for (int i=0; i < curr.nears.Count; i++)
            {
                if (!curr.nears[i].infra)
                {
                    _manager.BuildInfra(_manager.unit, curr.nears[i], team);
                    break;
                }
            }
        }
    }

    protected virtual IEnumerator Die()
    {
        //Dying Animation
        _mat.SetColor("_Color", Color.black);
        yield return new WaitForSeconds(0.5f);

        //Destroy Object
        curr.infra = null;
        curr = null;
        _tr = null;
        _mat = null;
        
        Destroy(this.gameObject);
    }

    //use to damage infra
    public void Hit(int ATT)
    {
        HP -= ATT;
        _bar.UpdateBar(HP / MaxHP);
        //gameObject.GetComponentInChildren<>;
        if (HP <= 0)
        {
            alive = false;
            StopAllCoroutines();
            StartCoroutine("Die");
            return;
        }
        else StartCoroutine("HitMotion");
    }

    private IEnumerator HitMotion()
    {
        _mat.SetColor("_Color", Color.gray);
        yield return new WaitForSeconds(0.1f);
        _manager.SetColor(this);
        StopCoroutine("Hit");
    }

}

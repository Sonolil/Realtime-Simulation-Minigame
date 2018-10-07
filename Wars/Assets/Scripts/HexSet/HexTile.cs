using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class HexTile : MonoBehaviour {
	public int cost = 1;
	public bool pass = true;
    public bool build = true;
    public Infra infra;
    public Vector3 location;
    public List<HexTile> nears = new List<HexTile>();
    public bool[] colony;
    
    //private Renderer _rend;
    private Renderer _rend;

	void Awake () {
        _rend = gameObject.GetComponentInChildren<Renderer>();
        colony = new bool[3];
    }

    void Start ()
    {
        if (!Application.isPlaying)
        {
            _rend.material.color = Color.black;
        }
    }

#if UNITY_EDITOR
    void Update()
    {
        if (!Application.isPlaying)
        {
            Reposition(location);
        }
    }
#endif

    private void Reposition(Vector3 axial) {
		float row = axial.z + (axial.x - (axial.x % 1)) / 2;		
		float col = axial.x;
        float hgt = axial.y * 0.4f; //+ Random.Range(0, 0.1f);
        row *= Mathf.Sqrt(3) / 2;
		col *= 0.75f;
		transform.position = new Vector3 (row, hgt, col);
    }

    public void AddColor(Color newColor)
    {
        _rend.material.color += newColor;
    }

    public void RemoveColor(Color newColor)
    {
        _rend.material.color -= newColor;
    }

    public void Colonize(int team)
    {
        colony[team] = true;

        if (team == 1)
            AddColor(Color.yellow/5);

        else if (team == 2)
            AddColor(Color.cyan/5);
    }

    public void Decolonize(int team)
    {
        colony[team] = false;

        if (team == 1)
            RemoveColor(Color.yellow/5);

        else if (team == 2)
            RemoveColor(Color.cyan/5);
    }

    public void Shuffle()
    {
        for (int i = 0; i < nears.Count; i++)
        {
            HexTile temp = nears[i];
            int r = Random.Range(i, nears.Count);
            nears[i] = nears[r];
            nears[r] = temp;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalStorm : MonoBehaviour {

    List<FractalStormBranch> branches = new List<FractalStormBranch>();

    [SerializeField] float scale;
    [SerializeField] int depth;
    // [SerializeField] AnimationCurve fadeOffCurve;
    [SerializeField, Range(0,1)] float diminishFactorOverDepth;
    //[SerializeField] int maxForkPerBranch;
    [SerializeField] float totalAngle = 45;
    [SerializeField] int[] forkPerDepth;
    [SerializeField] FractalStormBranch fractalStormBranchPrefab;

	// Use this for initialization
	void Start () {
        GenerateLightning();

        foreach(FractalStormBranch branch in branches)
        {
            branch.SetLineRendererPositions();
        }
    }
	
    void GenerateLightning()
    {
        FractalStormBranch branch = InstantiateFractalStorm(transform);
        branches.Add(branch);
        branch.length = scale;
        branch.vertices.Add(Vector3.zero);
        branch.vertices.Add(Vector3.down * scale);
        RecursiveGenerate(branch, 0);
    }

    FractalStormBranch InstantiateFractalStorm(Transform parent)
    {
        FractalStormBranch branch =  Instantiate(fractalStormBranchPrefab, parent);
        //Reset stuff
        return branch;
    }

    void RecursiveGenerate(FractalStormBranch branch, int currentDepth)
    {
        if (currentDepth >= depth)
            return;

        //float ratioDepth = GetCurrentDepthRatio(currentDepth);

        //int numberFork = Random.Range(0, maxForkPerBranch + 1);
        int numberFork = forkPerDepth[currentDepth];

        branch.childs = new FractalStormBranch[numberFork];
        for (int i = 0; i < numberFork; i++)
        {
            branch.childs[i] = InstantiateFractalStorm(branch.transform);
            branches.Add(branch.childs[i]);
        }

        //branch.vertices.Add(GameMath.RandomRotateVectorZ(Vector3.down * branch.length, -angles, angles));

        for (int i = 0; i < branch.childs.Length; i++)
        {
            FractalStormBranch child = branch.childs[i];
            child.vertices.AddRange(branch.vertices);

            float ratio = (float)i / branch.childs.Length;
            float angle = Mathf.Lerp(-totalAngle, totalAngle, ratio);
            child.length = branch.length * diminishFactorOverDepth;

            child.vertices.Add(GameMath.RotateVectorZ(angle, Vector3.down * child.length));
            //child.vertices.Add(GameMath.RandomRotateVectorZ(Vector3.down * branch.length, -angles, angles));

            RecursiveGenerate(child, currentDepth + 1);
        }
    }
  
    //float GetCurrentDepthRatio(int currentDepth)
    //{
    //    float ratioDepth = (float)currentDepth / depth;
    //    return fadeOffCurve.Evaluate(ratioDepth);
    //}

    //[System.Serializable]
    //public class LightningBranch
    //{
    //    public float length;
    //    public List<Vector3> vertices;

    //    public LightningBranch[] childs;
    //    public LineRenderer lineRenderer;
    //}
}

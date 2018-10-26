using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractalTree : MonoBehaviour {

    FractalTreeBranch mainBranch;
    protected List<FractalTreeBranch> branches = new List<FractalTreeBranch>();

    [Header("Scale and angles")]
    [SerializeField] protected float scale;
    [SerializeField] protected float totalAngle = 45;
    [SerializeField] protected float offsetAngle;

    [Header("Settings over depth")]
    [SerializeField, Range(0,2)] protected float lengthFactorOverDepth;
    [SerializeField, Range(0,2)] protected float angleFactorOverDepth;

    [Header("Direction")]
    [SerializeField] Vector3 startDirection;
    [SerializeField] Vector3 stormDirection;
    [SerializeField, Range(0, 1)] float directionWeigth;

    [Header("Color")]
    [SerializeField] Gradient gradientOverFork;

    [Header("Initial Settings")]
    //[SerializeField] int[] forkPerDepth;
    [SerializeField] int depth;
    [SerializeField] FractalTreeReader reader;
    [SerializeField] FractalTreeBranch fractalStormBranchPrefab;

    public void Start () {
        GenerateLightning();
        UpdateStorm();
    }

    private void OnValidate()
    {
        if(mainBranch != null)
            UpdateStorm();
    }

    public void UpdateStorm()
    {
        InitMainBranch();
        RecursiveUpdate(mainBranch, 0);
        foreach (FractalTreeBranch branch in branches)
        {
            branch.SetLineRendererPositions();
        }
    }

    void GenerateLightning()
    {
        mainBranch = InstantiateFractalStorm(transform);
        branches.Add(mainBranch);

        InitMainBranch();

        RecursiveGenerate(mainBranch, 0);
    }

    void InitMainBranch()
    {
        mainBranch.length = scale;
        mainBranch.direction = startDirection.normalized;
        mainBranch.totalAngle = totalAngle;
        mainBranch.vertices.Clear();
        mainBranch.vertices.Add(Vector3.zero);
        mainBranch.vertices.Add(stormDirection * scale);
    }

    FractalTreeBranch InstantiateFractalStorm(Transform parent)
    {
        FractalTreeBranch branch =  Instantiate(fractalStormBranchPrefab, parent);
        //Reset stuff
        return branch;
    }

    void RecursiveGenerate(FractalTreeBranch branch, int currentDepth)
    {
        if (currentDepth >= depth)
            return;

        int numberFork = reader.GetFork(currentDepth);// forkPerDepth[currentDepth];

        branch.childs = new FractalTreeBranch[numberFork];
        for (int i = 0; i < numberFork; i++)
        {
            branch.childs[i] = InstantiateFractalStorm(branch.transform);
            branches.Add(branch.childs[i]);
            RecursiveGenerate(branch.childs[i], currentDepth + 1);
        }
    }
  
    public void RecursiveUpdate(FractalTreeBranch branch, int currentDepth)
    {
        if (currentDepth >= depth)
            return;

        float ratioDepth = (float)currentDepth / (depth - 1);

        for (int i = 0; i < branch.childs.Length; i++)
        {
            FractalTreeBranch child = branch.childs[i];

            child.vertices.Clear();
            child.vertices.AddRange(branch.vertices);

            child.length = branch.length * lengthFactorOverDepth;
            child.totalAngle = branch.totalAngle * angleFactorOverDepth;

            //float ratio = (float)i / (branch.childs.Length-1); 
            //float angle = Mathf.Lerp((-halfAngle + offsetAngle), (halfAngle + offsetAngle), ratio);

            float halfAngle = child.totalAngle * 0.5f;
            float angle = reader.GetAngle
                (i, branch.childs.Length, currentDepth, -halfAngle + offsetAngle, halfAngle + offsetAngle);

            float length = reader.GetLenght(child.length);
            Vector3 direction;
            if (branch.childs.Length == 1)
            {
                direction = stormDirection * length;
            }
            else
            {
                Vector3 currentDirection = directionWeigth * branch.direction + (1 - directionWeigth) * stormDirection;
                direction = GameMath.RotateVectorZ(angle, currentDirection.normalized * length);
            }
            child.direction = direction;
            child.SetColor(gradientOverFork.Evaluate(ratioDepth));

            child.vertices.AddRange(reader.GetVertices(branch.position, direction));
            //child.vertices.Add(branch.position + direction);
            RecursiveUpdate(child, currentDepth + 1);
        }
    }
}

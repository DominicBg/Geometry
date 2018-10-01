using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour {

    [SerializeField] int branchCount;
    [SerializeField] Gradient gradient;
    [SerializeField] Snakes branchPrefab;

    Snakes[] branches;

    void Start()
    {
        SpawnBranches();
    }

    void Update()
    {
        UpdateBranchColor();
    }

    void SpawnBranches()
    {
        branches = new Snakes[branchCount];

        for (int i = 0; i < branchCount; i++)
        {
            branches[i] = Instantiate(branchPrefab, transform);
        }
    }

    void UpdateBranchColor()
    {
        for (int i = 0; i < branchCount; i++)
        {
            float t = (float)i / (branchCount - 1);
            Color color = gradient.Evaluate(t);
            branches[i].trailRenderer.startColor = color;
            branches[i].trailRenderer.endColor = Color.white - color + new Color(0,0,0,1);

        }
    }
}

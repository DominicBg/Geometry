using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUPlane : MonoBehaviour {
    public const string KERNELINDEX = "CSMAIN";

    public float[] array = new float[24];
    [SerializeField] ComputeShader shader;
	// Use this for initialization

    [ContextMenu("Generate")]
	void Start () {
        RunShader();
	}
	void RunShader()
    {
        int kernelIndex = shader.FindKernel(KERNELINDEX);

        ComputeBuffer buffer = new ComputeBuffer(array.Length, 12);
        buffer.SetData(array);
        shader.SetBuffer(kernelIndex, "output", buffer);
        shader.Dispatch(kernelIndex, array.Length, 1,1);

        float[] data = new float[15];

        buffer.GetData(data);
        buffer.Dispose();
    }
}

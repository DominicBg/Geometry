using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerNeuron : MonoBehaviour {
    public Neuron[] neurons { get; private set; }

    public void Initiate()
    {
        neurons = GetComponentsInChildren<Neuron>();
    }
}

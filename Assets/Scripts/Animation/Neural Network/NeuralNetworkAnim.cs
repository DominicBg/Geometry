using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetworkAnim : MonoBehaviour {

    [Header("Prefabs")]
    [SerializeField] Connection prefabConnection;
    [SerializeField] Neuron prefabNeuron;

    [Header("Settings")]
    [SerializeField] bool cylinderMode;
    [SerializeField] float angle;
    [SerializeField] int[] layerDimension;
    [SerializeField] float timeBetweenNeuronConnection;
    [SerializeField] Vector2 scaling;

    //[SerializeField] float growth = 2;

    [Header("Connections")]
    [SerializeField, Range(0, 1)] float weightColorLayer = 0.8f;
    [SerializeField] Gradient gradientLayer;
    [SerializeField] Gradient gradientNeuron;
    [SerializeField] Color endColor;

    LayerNeuron[] layers;

    int layer;
    int neuronPerLayer;

	// Use this for initialization
	void Start ()
    {
        GenerateNeuralNetwork();
        CreateConnections();
        StartCoroutine(AnimationRoutine());
    }
	
    void GenerateNeuralNetwork()
    {
        for (int i = 0; i < layerDimension.Length; i++)
        {
            GameObject layer = new GameObject("Layer" + i);
            LayerNeuron layerNeuron = layer.AddComponent<LayerNeuron>();
            layer.transform.SetParent(transform);
            layer.transform.localScale = Vector3.one;

            for (int j = 0; j < layerDimension[i]; j++)
            {
                Neuron newNeuron = Instantiate(prefabNeuron, layer.transform);
                newNeuron.transform.localScale = Vector3.one;
                newNeuron.transform.localPosition = CalculateNeuronPosition(i, j, layerDimension[i]);
            }
            layerNeuron.Initiate();
        }
        layers = GetComponentsInChildren<LayerNeuron>();
    }

    Vector3 CalculateNeuronPosition(int x, int y, int numberPerLayer)
    {
        if (cylinderMode)
        {
            float t = (float)x / (layerDimension.Length-1);
            float lerped = Mathf.Lerp(0, angle * Mathf.Deg2Rad, t);
            return GameMath.SphericalRotation(scaling.x, lerped, 0) + Vector3.up * y * scaling.y;
        }
        else
        {
            return new Vector3(x * scaling.x, -y * scaling.y, 0);
        }
    }

    void CreateConnections()
    {
        //On commence a la première hidden layer, donc i = 1
        for (int i = 1; i < layers.Length; i++)
        {
            LayerNeuron previousLayer = layers[i - 1];
            LayerNeuron currentLayer = layers[i];
            for (int j = 0; j < currentLayer.neurons.Length; j++)
            {
                //Create connections
                Neuron currentNeuron = currentLayer.neurons[j];
                currentNeuron.incomingConnections = new Connection[previousLayer.neurons.Length];
                for (int k = 0; k < currentNeuron.incomingConnections.Length; k++)
                {
                    CreateSingleConnection(i, previousLayer, currentLayer, currentNeuron, k);
                }
            }
        }
    }

    private void CreateSingleConnection(int i, LayerNeuron previousLayer, LayerNeuron currentLayer, Neuron currentNeuron, int k)
    {
        //On spawn autant de connection quia de neuron au previous layer
        Connection newConnection = Instantiate(prefabConnection, currentNeuron.transform);
        currentNeuron.incomingConnections[k] = newConnection;
        newConnection.transform.localScale = Vector3.one;
        //On connect tous les neurons du previous layer au current neuron
        newConnection.from = previousLayer.neurons[k];
        newConnection.to = currentNeuron;

        float layerT = (float)i / layers.Length;
        float neuronT = (float)k / currentLayer.neurons.Length;

        Color layerColor = gradientLayer.Evaluate(layerT);
        Color neuronColor = gradientNeuron.Evaluate(neuronT);
        Color startColor = (layerColor * weightColorLayer + neuronColor * (1 - weightColorLayer));

        newConnection.enableColor = startColor;
        newConnection.disableColor = endColor;

        newConnection.ConnectionFromTo();
    }

    IEnumerator AnimationRoutine()
    {
        LayerNeuron inputLayer = layers[0];
        LayerNeuron firstHiddenLayer = layers[1];

        for (int j = 0; j < firstHiddenLayer.neurons.Length; j++)
        {
            for (int i = 0; i < inputLayer.neurons.Length; i++)
            {
                //On grossie le neuron assez de fois pour qu'il se fasse absorbé par les nexts layers
                Neuron currentNeuron = inputLayer.neurons[i];
                currentNeuron.Growth(); 
            }
            yield return new WaitForSeconds(timeBetweenNeuronConnection);
        }

        for (int i = 0; i < layers.Length; i++)
        {
            LayerNeuron currentLayer = layers[i];
            for (int j = 0; j < currentLayer.neurons.Length; j++)
            {
                Neuron currentNeuron = currentLayer.neurons[j];
                int connectionPerNeuron = currentNeuron.incomingConnections.Length;

                for (int k = 0; k < connectionPerNeuron; k++)
                {
                    currentNeuron.PlayAnimationWithConnection(k);
                    yield return new WaitForSeconds(timeBetweenNeuronConnection);
                }

                yield return new WaitForSeconds(timeBetweenNeuronConnection);
            }
            yield return new WaitForSeconds(timeBetweenNeuronConnection);
        }


        LayerNeuron outputLayer = layers[layers.Length - 1];
        LayerNeuron lastHiddenLayer = layers[layers.Length - 2];

        for (int j = 0; j < lastHiddenLayer.neurons.Length; j++)
        {
            for (int i = 0; i < outputLayer.neurons.Length; i++)
            {
                //On grossie le neuron assez de fois pour qu'il se fasse absorbé par les nexts layers
                Neuron currentNeuron = outputLayer.neurons[i];
                currentNeuron.Shrink();
            }
            yield return new WaitForSeconds(timeBetweenNeuronConnection);

        }
        StartCoroutine(AnimationRoutine());

    }
}

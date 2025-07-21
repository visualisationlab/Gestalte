using UnityEngine;

public class SimpleNeuralNetwork
{
    float[,] weightsInputHidden = new float[2, 2];
    float[] biasesHidden = new float[2];
    
    float[] weightsHiddenOutput = new float[2];
    float biasOutput = 0f;
    
    System.Random rnd = new();
    
    float Sigmoid(float x) => 1f / (1f + Mathf.Exp(-x));

    public void Initialize()
    {
        for (int i = 0; i < 2; i++)
        {
            biasesHidden[i] = (float)rnd.NextDouble() * 2 - 1; // Random bias between -1 and 1
            weightsHiddenOutput[i] = (float)rnd.NextDouble() * 2 - 1; // Random weight between -1 and 1
            for (int j = 0; j < 2; j++)
            {
                weightsInputHidden[i, j] = (float)rnd.NextDouble() * 2 - 1; // Random weight between -1 and 1
            }
            biasOutput = (float)rnd.NextDouble() * 2 - 1; // Random bias between -1 and 1
        }
    }
    
    public float Forward(float x1, float x2, out float[] hiddenActivations)
    {
        hiddenActivations = new float[2];
        float[] hiddenRaw = new float[2];

        // Hidden layer computation
        for (int i = 0; i < 2; i++)
        {
            hiddenRaw[i] = x1 * weightsInputHidden[0, i] + x2 * weightsInputHidden[1, i] + biasesHidden[i];
            hiddenActivations[i] = Sigmoid(hiddenRaw[i]);
        }

        // Output layer
        float output = hiddenActivations[0] * weightsHiddenOutput[0] +
                       hiddenActivations[1] * weightsHiddenOutput[1] +
                       biasOutput;
        return Sigmoid(output);
    }
    
    public void Train(float x1, float x2, float target, float learningRate)
    {
        float[] hiddenActivations;
        float output = Forward(x1, x2, out hiddenActivations);

        float outputError = target - output;
        float outputDelta = outputError * output * (1 - output); // sigmoid derivative

        // Backpropagate to hidden layer
        float[] hiddenDeltas = new float[2];
        for (int i = 0; i < 2; i++)
        {
            float hiddenOutput = hiddenActivations[i];
            hiddenDeltas[i] = outputDelta * weightsHiddenOutput[i] * hiddenOutput * (1 - hiddenOutput);
        }

        // Update weights and biases
        for (int i = 0; i < 2; i++)
        {
            weightsHiddenOutput[i] += learningRate * outputDelta * hiddenActivations[i];
            for (int j = 0; j < 2; j++)
            {
                float input = j == 0 ? x1 : x2;
                weightsInputHidden[j, i] += learningRate * hiddenDeltas[i] * input;
            }
            biasesHidden[i] += learningRate * hiddenDeltas[i];
        }

        biasOutput += learningRate * outputDelta;
    }
    
    public void TrainXOR()
    {
        Initialize();
        float[][] data = new[]
        {
            new float[] { 0, 0, 0 },
            new float[] { 0, 1, 1 },
            new float[] { 1, 0, 1 },
            new float[] { 1, 1, 0 }
        };

        for (int epoch = 0; epoch < 10000; epoch++)
        {
            foreach (var sample in data)
            {
                Train(sample[0], sample[1], sample[2], 0.1f); //input A, input B, target result, learning rate
            }

            if (epoch % 1000 == 0)
            {
                Debug.Log($"Epoch {epoch}");
                foreach (var sample in data)
                {
                    float output = Forward(sample[0], sample[1], out _);
                    Debug.Log($"Input: {sample[0]}, {sample[1]} => Predicted: {output:F3}");
                }
            }
        }
    }
}

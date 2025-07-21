using UnityEngine;

namespace DefaultNamespace
{
    public class XORTrainer : MonoBehaviour
    {
        void Start()
        {
            SimpleNeuralNetwork nn = new SimpleNeuralNetwork();
            nn.TrainXOR();
        }
    }
}
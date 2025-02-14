using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra.Double;
using NeuralNetworkLibrary;

namespace NeuralNetworkLibrary
{
    using Vector = MathNet.Numerics.LinearAlgebra.Vector<double>;
    public class NeuralNet : ICloneable
    {
        
        public List<NNLayer> Layers = new List<NNLayer>();

        public int NumberOfInputs;
        public int NumberOfOutputs;

        private Vector[] Inputs;

        private bool RandomizeNewLayers = true;

        public NeuralNet(int inputs)
        {
            NumberOfInputs = inputs;
            NumberOfOutputs = inputs;
            Inputs = Array.Empty<Vector>();
            RandomizeNewLayers = true;
        }

        public NeuralNet(int inputs, bool randomize) : this(inputs)
        {
            RandomizeNewLayers = randomize;
        }

        public NeuralNet AddLayer(Activation activation, int outputs)
        {
            if (RandomizeNewLayers)
            {
                Layers.Add(new NNLayer(NumberOfOutputs, outputs, Random.Shared, activation));
            }
            else
            {
                Layers.Add(new NNLayer(NumberOfOutputs, outputs, () => 0.0, activation));
            }
            NumberOfOutputs = outputs;
            return this;
        }

        private NeuralNet(NeuralNet toCopy)
        {
            Layers = new List<NNLayer>(toCopy.Layers.Count);
            for (int i = 0; i < toCopy.Layers.Count; i++)
            {
                Layers.Add(toCopy.Layers[i].Clone());
            }
            NumberOfOutputs = toCopy.NumberOfOutputs;
            NumberOfInputs = toCopy.NumberOfInputs;
            Inputs = toCopy.Inputs.Select(a => a.Clone()).ToArray();
        }

        public NeuralNet Clone()
        {
            return new NeuralNet(this);
        }

        public Vector Compute(Vector inputs)
        {
            Inputs = new Vector[Layers.Count];

            for (int i = 0; i < Layers.Count; i++)
            {
                Inputs[i] = inputs;

                inputs = Layers[i].ComputeOutputs(inputs);
            }

            return inputs;
        }

        public double[] Compute(double[] inputs)
        {
            return Compute(new DenseVector(inputs)).ToArray();
        }

        public double Compute(double input)
        {
            return Compute(new DenseVector([input]))[0];
        }

        public void ClearUpdates()
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].ClearUpdates();
            }
        }

        public void AccumulateUpdates(Vector partialErrorPartialOutputs)
        {
            for (int i = Layers.Count - 1; i >= 0; i--)
            {
                partialErrorPartialOutputs = Layers[i].AccumulateDerivatives(Inputs[i], partialErrorPartialOutputs);
            }
        }

        public void ApplyUpdates(double learningRate, double momentum)
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                Layers[i].ApplyUpdates(learningRate, momentum);
            }
        }

        object ICloneable.Clone() => Clone();
    }
}
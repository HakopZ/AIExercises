
using MathNet.Numerics.LinearAlgebra.Double;

namespace NeuralNetworkLibrary;


using Vector = MathNet.Numerics.LinearAlgebra.Vector<double>;
using Matrix = MathNet.Numerics.LinearAlgebra.Matrix<double>;
public class NNLayer : ICloneable
{

    public Matrix Weights;
    public Vector Biases;

    public Vector PreActivationSum;
    public Vector Outputs;

    public Vector EOutputGrad;

    public Matrix WeightUpdates;

    public Func<double, double> ActivationFunction;
    public Func<double, double, double> ActivationDerivative;

    public Vector BiasUpdates;

    public Matrix PreviousWeightUpdates;
    public Vector PreviousBiasUpdates;

    public NNLayer(int inputs, int outputs, Random random, Activation activation) : this(inputs, outputs, () => random.NextDouble() * 2 - 1, activation.ActivationFunction, activation.ActivationDerivativeFunction)
    { }

    public NNLayer(int inputs, int outputs, Func<double> generateWeights, Activation activation) : this(inputs, outputs, generateWeights, activation.ActivationFunction, activation.ActivationDerivativeFunction)
    { }

    public NNLayer(int inputs, int outputs, Func<double> generateWeight, Func<double, double> activationFunction, Func<double, double, double> activationDerivative)
    {
        Weights = new DenseMatrix(outputs, inputs);
        Biases = new DenseVector(outputs);

        PreActivationSum = new DenseVector(outputs);
        Outputs = new DenseVector(outputs);
        EOutputGrad = new DenseVector(outputs);
        WeightUpdates = new DenseMatrix(outputs, inputs);
        PreviousWeightUpdates = WeightUpdates.Clone();

        ActivationFunction = activationFunction;
        ActivationDerivative = activationDerivative;

        BiasUpdates = new DenseVector(outputs);
        PreviousBiasUpdates = BiasUpdates.Clone();

        for (int j = 0; j < outputs; j++)
        {
            for (int i = 0; i < inputs; i++)
            {
                Weights[j, i] = generateWeight();
            }

            Biases[j] = generateWeight();
        }
    }

    private NNLayer(NNLayer input)
    {
        Weights = input.Weights.Clone();
        Biases = input.Biases.Clone();
        PreActivationSum = input.PreActivationSum.Clone();
        PreviousBiasUpdates = input.PreviousBiasUpdates.Clone();
        ActivationDerivative = input.ActivationDerivative;
        ActivationFunction = input.ActivationFunction;
        Outputs = input.Outputs.Clone();
        EOutputGrad = input.EOutputGrad.Clone();
        WeightUpdates = input.WeightUpdates.Clone();
        BiasUpdates = input.BiasUpdates.Clone();
        PreviousWeightUpdates = input.PreviousWeightUpdates.Clone();
    }

    private Vector ComponentwiseActivate(Vector inputs)
    {
        Vector outputs = new DenseVector(inputs.Count());
        for (int i = 0; i < outputs.Count; i++)
        {
            outputs[i] = ActivationFunction(inputs[i]);
        }
        return outputs;
    }

    public Vector ComputeOutputs(Vector inputs)
    {
        PreActivationSum = Weights * inputs + Biases;
        return Outputs = ComponentwiseActivate(PreActivationSum);
    }

    public void ClearUpdates()
    {
        BiasUpdates.Clear();
        WeightUpdates.Clear();
    }

    public Vector AccumulateDerivatives(Vector inputs, Vector partialEpartialOutputs)
    {
        EOutputGrad = partialEpartialOutputs;

        Vector eBiasGrad = EOutputGrad.Clone();

        for (int i = 0; i < BiasUpdates.Count; i++)
        {
            eBiasGrad[i] *= ActivationDerivative(PreActivationSum![i], Outputs![i]);
        }

        var partialEPartialWeights = eBiasGrad.ToColumnMatrix() * inputs.ToRowMatrix();
        WeightUpdates += partialEPartialWeights;

        BiasUpdates += eBiasGrad;

        return Weights!.TransposeThisAndMultiply(eBiasGrad!);
    }

    public void ApplyUpdates(double learningRate, double momentum)
    {
        Biases += -learningRate * BiasUpdates - momentum * PreviousBiasUpdates;
        Weights += -learningRate * WeightUpdates - momentum * PreviousWeightUpdates;
        PreviousWeightUpdates = WeightUpdates.Clone();
        PreviousBiasUpdates = BiasUpdates.Clone();
    }

    public NNLayer Clone()
    {
        return new NNLayer(this);
    }

    object ICloneable.Clone() => Clone();
}

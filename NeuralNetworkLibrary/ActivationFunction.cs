namespace NeuralNetworkLibrary;

public class Activation

{
    public Func<double, double> ActivationFunction { get; }
    public Func<double, double, double> ActivationDerivativeFunction { get; }

    public Activation(Func<double, double> activationFunction, Func<double, double, double> activationDerivativeFunction)
    {
        ActivationFunction = activationFunction;
        ActivationDerivativeFunction = activationDerivativeFunction;
    }
}

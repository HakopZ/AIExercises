namespace NeuralNetworkLibrary;
public static class Activations
{
    public static readonly Activation Identity = new Activation(x => x, (_, _) => 1);
    public static readonly Activation ReLU = new Activation(x => (x > 0) ? x : 0, (x, _) => (x > 0) ? 1 : 0);
    public static readonly Activation LeakyReLU = new Activation(x => (x > 0) ? x : (0.01 * x), (x, _) => (x > 0) ? 1 : 0.01);
    public static readonly Activation Tanh = new Activation(Math.Tanh, (_, x) => 1 - x * x);

}
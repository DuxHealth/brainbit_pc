using System;

using NeuroTech.Spectrum;

namespace CallibriDemo.NeuroImpl;

public class SpectrumController
{
    private const double AlphaCoef = 1.0;
    private const double BetaCoef  = 1.0;
    private const double DeltaCoef = 0.0;
    private const double GammaCoef = 0.0;
    private const double ThetaCoef = 1.0;

    private const int ProcessWinRate = 20; // Hz
    private const int BordFrequency  = 50;
    private const int FftWindow      = 4000;

    private readonly SpectrumMath _math;

    public Action<RawSpectrumData[], WavesSpectrumData[]> ProcessedData;

    public SpectrumController(int samplingRate)
    {
        _math = new SpectrumMath(samplingRate, FftWindow, ProcessWinRate);
        _math.InitParams(BordFrequency, true);
        _math.SetWavesCoeffs(DeltaCoef, ThetaCoef, AlphaCoef, BetaCoef, GammaCoef);
    }

    public double FftBinsFor1Hz { get => _math.GetFFTBinsFor1Hz(); }

    public void Dispose() { _math.Dispose(); }

    public void ProcessSamples(double[] samples)
    {
        try
        {
            _math.PushData(samples);

            var rawSpectrumData   = _math.ReadRawSpectrumInfoArr();
            var wavesSpectrumData = _math.ReadWavesSpectrumInfoArr();

            _math.SetNewSampleSize();

            ProcessedData?.Invoke(rawSpectrumData, wavesSpectrumData);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}

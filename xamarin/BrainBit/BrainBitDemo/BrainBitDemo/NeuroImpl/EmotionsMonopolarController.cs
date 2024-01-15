using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroSDK;

using SignalMath;

namespace BrainBitDemo.NeuroImpl;

public class EmotionsMonopolarController
{
    private readonly Dictionary<string,EegEmotionalMath> _math = new Dictionary<string, EegEmotionalMath>();

    public Action<int, string> progressCalibrationCallback = null;
    public Action<bool, string> isArtefactedSequenceCallback = null;
    public Action<bool, string> isBothSidesArtifactedCallback = null;
    public Action<SpectralDataPercents, string> lastSpectralDataCallback = null;
    public Action<RawSpectVals, string> rawSpectralDataCallback = null;
    public Action<MindData, string> lastMindDataCallback = null;

    private bool isCalibrated = false;

    private object _locker = new object();


    public EmotionsMonopolarController() 
    {
        var config = EmotionalMathConfig.GetDefault(false);

        
        _math["O1"] = new EegEmotionalMath(
            config.MathLib,
            config.ArtifactDetect,
            config.ShortArtifactDetect,
            config.MentalAndSpectral);
        _math["O2"] = new EegEmotionalMath(
            config.MathLib,
            config.ArtifactDetect,
            config.ShortArtifactDetect,
            config.MentalAndSpectral);
        _math["T3"] = new EegEmotionalMath(
            config.MathLib,
            config.ArtifactDetect,
            config.ShortArtifactDetect,
            config.MentalAndSpectral);
        _math["T4"] = new EegEmotionalMath(
            config.MathLib,
            config.ArtifactDetect,
            config.ShortArtifactDetect,
            config.MentalAndSpectral);

        foreach (var math in _math.Values)
        {
            math?.SetZeroSpectWaves(config.Active, config.delta, config.theta, config.alpha, config.beta, config.gamma);
            math?.SetWeightsForSpectra(config.delta_c, config.theta_c, config.alpha_c, config.beta_c, config.gamma_c);
            math?.SetCallibrationLength(config.CallibrationLength);
            math?.SetMentalEstimationMode(config.MentalEstimation);
            math?.SetPrioritySide(config.PrioritySide);
            math?.SetSkipWinsAfterArtifact(config.SkipWinsAfterArtifact);
            math?.SetSpectNormalizationByBandsWidth(config.SpectNormalizationByBandsWidth);
            math?.SetSpectNormalizationByCoeffs(config.SpectNormalizationByCoeffs);
        }
        
    }

    public void Dispose() 
    {
        lock (_locker)
        {
            foreach (var math in _math.Values)
            {
                math?.Dispose();
            }
            _math.Clear();
        }
        
    }

    public void StartCalibration() 
    {
        lock ( _locker ) {
            foreach (var math in _math.Values)
            {
                math?.StartCalibration();
            }
        }
        
    }

    public void ProcessData(BrainBitSignalData[] samples)
    {
        Task.Factory.StartNew(() => {
            lock (_locker)
            {
                var o1Values = new RawChannelsArray[samples.Length];
                var o2Values = new RawChannelsArray[samples.Length];
                var t3Values = new RawChannelsArray[samples.Length];
                var t4Values = new RawChannelsArray[samples.Length];

                for (int i = 0; i < samples.Length; i++)
                {
                    o1Values[i].channels = [samples[i].O1];
                    o2Values[i].channels = [samples[i].O2];
                    t3Values[i].channels = [samples[i].T3];
                    t4Values[i].channels = [samples[i].T4];
                }

                _math["O1"]?.PushDataArr(o1Values);
                _math["O2"]?.PushDataArr(o2Values);
                _math["T3"]?.PushDataArr(t3Values);
                _math["T4"]?.PushDataArr(t4Values);

                try
                {
                    foreach (var math in _math.Values)
                    {
                        math?.ProcessDataArr();
                    }
                }
                catch { }

                resolveArtefacted();

                if (!isCalibrated)
                {
                    processCalibration();
                }
                else
                {
                    resolveSpectralData();
                    resolveRawSpectralData();
                    resolveMindData();
                }
            }
            
        });
    }

    private void resolveArtefacted()
    {
        foreach (var math in _math)
        {
            // sequence artifacts
            bool isArtifactedSequence = math.Value.IsArtifactedSequence();
            isArtefactedSequenceCallback?.Invoke(isArtifactedSequence, math.Key);

            // both sides artifacts
            bool isBothSideArtifacted = math.Value.IsBothSidesArtifacted();
            isBothSidesArtifactedCallback?.Invoke(isBothSideArtifacted, math.Key);
        }
    }

    private void processCalibration()
    {
        foreach (var math in _math)
        {
            isCalibrated = math.Value.CalibrationFinished();
            if (!isCalibrated)
            {
                int progress = math.Value.GetCallibrationPercents();
                progressCalibrationCallback?.Invoke(progress, math.Key);
            }
        }
    }

    private void resolveSpectralData() 
    {
        foreach (var math in _math)
        {
            var spectralValues = math.Value?.ReadSpectralDataPercentsArr();
            lastSpectralDataCallback?.Invoke(spectralValues.Last(), math.Key);
        }
    }
    private void resolveRawSpectralData() 
    {
        foreach (var math in _math)
        {
            var rawSpectralValues = math.Value.ReadRawSpectralVals();
            rawSpectralDataCallback?.Invoke(rawSpectralValues, math.Key);
        }
    }
    private void resolveMindData() 
    {
        foreach (var math in _math)
        {
            var mentalValues = math.Value.ReadMentalDataArr();
            lastMindDataCallback?.Invoke(mentalValues.Last(), math.Key);
        }
    }
}

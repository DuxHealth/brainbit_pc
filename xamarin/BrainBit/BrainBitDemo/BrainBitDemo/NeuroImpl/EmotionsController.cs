using System;
using System.Text;

using NeuroSDK;

using SignalMath;

namespace BrainBitDemo.NeuroImpl;

public class EmotionsController
{
    private readonly EegEmotionalMath _math;

    public EmotionsController() { _math = CreateEmotionLib(); }

    private static EegEmotionalMath CreateEmotionLib()
    {
        const int calibrationLength      = 8;
        const int nwinsSkipAfterArtifact = 10;

        var mls = new MathLibSetting
        {
            sampling_rate        = (uint)BrainBitController.Instance.SamplingFrequency,
            process_win_freq     = 25,
            fft_window = (uint)BrainBitController.Instance.SamplingFrequency * 2,
            n_first_sec_skipped  = 6,
            bipolar_mode         = true,
            squared_spectrum = false,
            channels_number      = 4,
            channel_for_analysis = 0
        };

        var ads = new ArtifactDetectSetting
        {
            art_bord                  = 110,
            allowed_percent_artpoints = 70,
            raw_betap_limit           = 800_000,
            total_pow_border          = 30_000_000,
            global_artwin_sec         = 4,
            spect_art_by_totalp       = false,
            num_wins_for_quality_avg  = 100,
            hanning_win_spectrum      = false,
            hamming_win_spectrum      = true
        };

        var sads = new ShortArtifactDetectSetting 
        { 
            ampl_art_detect_win_size = 200, 
            ampl_art_zerod_area = 200, 
            ampl_art_extremum_border = 25 
        };

        var mss = new MentalAndSpectralSetting 
        { 
            n_sec_for_averaging = 2, 
            n_sec_for_instant_estimation = 2 
        };

        var math = new EegEmotionalMath(mls, ads, sads, mss);

        bool independentMentalLevels = false;
        math.SetMentalEstimationMode(independentMentalLevels);
        math.SetCallibrationLength(calibrationLength);
        math.SetSkipWinsAfterArtifact(nwinsSkipAfterArtifact);
        math.SetZeroSpectWaves(true, 0, 1, 1, 1, 0);

        return math;
    }

    public void Dispose() { _math.Dispose(); }

    public void StartCalibration() { _math.StartCalibration(); }

    public string PushData(BrainBitSignalData[] samples)
    {
        var emotionResult = "";

        var bipolarSamples = new RawChannels[samples.Length];

        for (var i = 0; i < samples.Length; i++)
        {
            bipolarSamples[i].LeftBipolar  = samples[i].T3 - samples[i].O1;
            bipolarSamples[i].RightBipolar = samples[i].T4 - samples[i].O2;
        }

        try
        {
            _math.PushData(bipolarSamples);
            _math.ProcessDataArr();

            bool calibrationFinished = _math.CalibrationFinished();
            if (calibrationFinished)
                emotionResult = GetLibResults();
            else
            {
                var sb = new StringBuilder();

                sb.AppendLine($"Calibration in progress: {_math.GetCallibrationPercents()} %");
                sb.AppendLine($"Artifacts: {_math.IsBothSidesArtifacted()}");

                emotionResult = sb.ToString();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return emotionResult;
    }

    private string GetLibResults()
    {
        var sb        = new StringBuilder();
        var mentalArr = _math.ReadMentalDataArr();

        sb.AppendLine($"MindData size {mentalArr.Length}");

        foreach (MindData item in mentalArr) sb.AppendLine($"Rel_Att: {item.RelAttention} \n" + $"Rel_Relax {item.RelRelaxation}\n" + $"Inst_Att {item.InstAttention}\n" + $"Inst_Relax {item.InstRelaxation} \n");

        var spectralPercentsArr = _math.ReadSpectralDataPercentsArr();
        sb.AppendLine($"SpectralPercentsArr size {spectralPercentsArr.Length}");

        foreach (SpectralDataPercents item in spectralPercentsArr) sb.AppendLine($"Delta: {item.Delta * 100} \n" + $"Theta: {item.Theta * 100} \n" + $"Alpha: {item.Alpha * 100} \n" + $"Beta: {item.Beta * 100} \n" + $"Gamma: {item.Gamma * 100} \n");
        sb.AppendLine($"Artifacts: {_math.IsArtifactedSequence()}");

        return sb.ToString();
    }
}

using System;
using System.Text;

using SignalMath;

namespace CallibriDemo.NeuroImpl;

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
            sampling_rate        = (uint)CallibriController.Instance.SamplingFrequency,
            process_win_freq     = 25,
            n_first_sec_skipped  = 6,
            fft_window           = 250,
            bipolar_mode         = false,
            channels_number      = 1,
            channel_for_analysis = 0
        };

        var ads = new ArtifactDetectSetting
        {
            art_bord                  = 70,
            allowed_percent_artpoints = 50,
            raw_betap_limit           = 800000,
            global_artwin_sec         = 4,
            num_wins_for_quality_avg  = 125,
            hanning_win_spectrum      = true
        };

        var sads = new ShortArtifactDetectSetting { ampl_art_detect_win_size = 200, ampl_art_zerod_area = 200, ampl_art_extremum_border = 25 };

        var mss = new MentalAndSpectralSetting { n_sec_for_averaging = 2, n_sec_for_instant_estimation = 2 };

        var math = new EegEmotionalMath(mls, ads, sads, mss);

        math.SetCallibrationLength(calibrationLength);
        math.SetSkipWinsAfterArtifact(nwinsSkipAfterArtifact);
        math.SetZeroSpectWaves(true, 0, 1, 1, 1, 0);

        return math;
    }

    public void Dispose() { _math.Dispose(); }

    public void StartCalibration() { _math.StartCalibration(); }

    public string PushData(double[] samples)
    {
        var emotionResult = "";

        try
        {
            var channelData = new RawChannelsArray[1];
            channelData[0].channels = samples;

            _math.PushDataArr(channelData);
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

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Neurotech.CallibriUtils;

using Xamarin.Forms.Internals;

namespace CallibriDemo.NeuroImpl;

public struct ECGData
{
    public double RR;
    public double HR;
    public double PressureIndex;
    public double Moda;
    public double AmplModa;
    public double VariationDist;
}

public class ECGController
{
    private readonly CallibriMath _math;

    public Action<ECGData> ProcessedResult;

    private readonly ConcurrentQueue<double> _samplesBuf = new();
    private readonly int                     _packSize;

    public ECGController(int samplingRate)
    {
        int dataWindow            = samplingRate / 2;
        var nwinsForPressureIndex = 30;

        _math     = new CallibriMath(samplingRate, dataWindow, nwinsForPressureIndex);
        _packSize = samplingRate / 10;
    }

    public void Dispose() { _math.Dispose(); }

    public void ProcessSamples(IEnumerable<double> samples)
    {
        samples.ForEach(x => { _samplesBuf.Enqueue(x * 1e6); });

        var pack = new double[_packSize];

        if (_samplesBuf.Count >= _packSize)
            for (var i = 0; i < _packSize; i++)
                _samplesBuf.TryDequeue(out pack[i]);
        else return;

        _math.PushData(pack);

        bool rrDetected = _math.RRdetected;
        if (!rrDetected) return;

        ProcessedResult?.Invoke(
            new ECGData
            {
                RR            = _math.RR,
                HR            = _math.HR,
                AmplModa      = _math.AmplModa,
                Moda          = _math.Moda,
                PressureIndex = _math.PressureIndex,
                VariationDist = _math.VariationDist
            }
        );
        _math.SetRRchecked();
    }
}

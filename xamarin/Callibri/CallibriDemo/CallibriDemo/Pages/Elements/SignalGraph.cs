using System;
using System.Diagnostics;
using System.Threading.Tasks;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using Xamarin.Forms;

namespace CallibriDemo.Pages.Elements;

public class SignalGraph : SKCanvasView
{
    public SignalGraph()
    {
        _axisPaint  = new SKPaint { Style = SKPaintStyle.Stroke, Color       = GraphColor.ToSKColor(), IsAntialias = true };
        _graphPaint = new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 3, Color                            = GraphColor.ToSKColor(), IsAntialias = true };
        _textPaint  = new SKPaint { Style = SKPaintStyle.Fill, Color         = GraphColor.ToSKColor(), TextSize    = 32.0f, IsAntialias                  = true };
    }

    public void Init(int maxWindow, int samplingFrequency)
    {
        if (_samples != null) return;

        Window             = maxWindow;
        _samplingFrequency = samplingFrequency;
        _maxSamples        = maxWindow * samplingFrequency;

        _samples = new double[_maxSamples];
    }

    public void AddSamples(double[] samples)
    {
        if (samples is not { Length: > 0 }) return;

        _allSamplesCount += samples.Length;
        Array.Copy(_samples, samples.Length, _samples, 0,                                _samples.Length - samples.Length);
        Array.Copy(samples,  0,              _samples, _samples.Length - samples.Length, samples.Length);
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        SKSurface surface = e.Surface;
        SKCanvas  canvas  = surface.Canvas;

        _abscissa     = _padding.Top + (CanvasSize.Height - (_padding.Bottom + _padding.Top)) * 0.5f;
        _spaceForText = CanvasSize.Width * 0.10f;

        canvas.Clear(BackColor.ToSKColor());

        DrawAxis(canvas);
        DrawTime(canvas);
        DrawGraph(canvas);
    }

    private void DrawAxis(SKCanvas canvas)
    {
        _textPaint.TextSize = CanvasSize.Width * 0.025f;
        _textPaint.MeasureText("-" + _ampText, ref _ampTextRect);
        float x = _padding.Left + _spaceForText;

        canvas.DrawText(_ampText,       (x - _ampTextRect.Width) / 2, _padding.Top + _textPaint.TextSize / 2,                        _textPaint);
        canvas.DrawText("-" + _ampText, (x - _ampTextRect.Width) / 2, CanvasSize.Height - _padding.Bottom + _textPaint.TextSize / 2, _textPaint);

        canvas.DrawLine(x, _padding.Top,                        x,        CanvasSize.Height - _padding.Bottom, _axisPaint);
        canvas.DrawLine(x, _padding.Top,                        x * 1.2f, _padding.Top,                        _axisPaint);
        canvas.DrawLine(x, CanvasSize.Height - _padding.Bottom, x * 1.2f, CanvasSize.Height - _padding.Bottom, _axisPaint);

        canvas.DrawLine(x, _abscissa, CanvasSize.Width - _padding.Right, _abscissa, _axisPaint);
    }

    private void DrawTime(SKCanvas canvas)
    {
        double mViewTime = _allSamplesCount / (double)_samplingFrequency;

        double startDashTime = Math.Floor(mViewTime - Window);
        double move          = (CanvasSize.Width - (_padding.Left + _padding.Right) - _spaceForText) / (double)Window;

        for (int i = Window; i > 0; i--)
        {
            double dashTime     = startDashTime + i;
            double dashX        = move * (mViewTime - Math.Floor(mViewTime));
            string dashTimeText = dashTime > 0 ? TimeSpan.FromSeconds(dashTime).ToString(@"mm\:ss") : "";
            _textPaint.TextSize = CanvasSize.Width * 0.025f;
            canvas.DrawText(
                dashTimeText,
                _padding.Right + _spaceForText + (float)move * i - (float)dashX,
                CanvasSize.Height - _padding.Left,
                _textPaint
            );
        }
    }

    private void DrawGraph(SKCanvas canvas)
    {
        if (_samples == null) return;
        var path = new SKPath();

        int   samplesCount = _samplingFrequency * Window;
        float xAxisStep    = (CanvasSize.Width - _spaceForText - (_padding.Left + _padding.Right)) / samplesCount;

        float leftPadding = _padding.Left + _spaceForText;

        float yAxisStep = (CanvasSize.Height - (_padding.Bottom + _padding.Top)) / (Amplitude * 2.0f);
        float yMin      = _padding.Top;
        float yMax      = CanvasSize.Height - _padding.Bottom;

        float x = leftPadding;
        float y = _padding.Top + yAxisStep * ((float)-_samples[_maxSamples - samplesCount] + Amplitude);

        if (y < yMin) y      = yMin;
        else if (y > yMax) y = yMax;

        path.MoveTo(x, y);

        for (int i = _maxSamples - samplesCount + 1; i < _maxSamples; i++)
        {
            y = _padding.Top + yAxisStep * ((float)-_samples[i] + Amplitude);

            if (y < yMin) y      = yMin;
            else if (y > yMax) y = yMax;

            path.LineTo(x, y);
            x += xAxisStep;
        }
        canvas.DrawPath(path, _graphPaint);
    }

#region Const
    private int _maxSamples        = 2500;
    private int _samplingFrequency = 250;
#endregion

#region Parameters
    private double[] _samples;

    public int Window { get; set; } = 10;

    private float _amplitude = 1000000;

    public float Amplitude
    {
        get => _amplitude;

        set
        {
            if (_amplitude == value) return;

            _amplitude = value;
            _ampText   = _amplitude >= 10f ? _amplitude + "mV" : _amplitude * 100 + "uV";
        }
    }
#endregion

#region Properties
    private Color _backColor = Color.Black;

    public Color BackColor
    {
        get => _backColor;

        set
        {
            if (_backColor == value) return;

            _backColor = value;
            if (!_stopwatch.IsRunning) OnPropertyChanged();
        }
    }

    private Color _graphColor = Color.White;

    public Color GraphColor
    {
        get => _graphColor;

        set
        {
            if (_graphColor == value) return;

            _graphColor = value;
            OnPropertyChanged();
        }
    }

    public static BindableProperty FPSProperty = BindableProperty.Create(
        nameof(FPS),
        typeof(int),
        typeof(SignalGraph),
        30
    );

    public int FPS { get => (int)GetValue(FPSProperty); set => SetValue(FPSProperty, value); }
#endregion

#region Variables
    private float _abscissa;
    private int   _allSamplesCount;
    private float _spaceForText;

    private readonly SKPaint _axisPaint;
    private readonly SKPaint _graphPaint;
    private readonly SKPaint _textPaint;

    private readonly Point _padding = new(40.0f, 40.0f);

    private SKRect _ampTextRect;
    private string _ampText;
#endregion

#region Redraw
    private readonly Stopwatch _stopwatch = new();

    private async Task AnimationLoop()
    {
        var delay = (int)(1.0 / FPS * 1000);
        while (_stopwatch.IsRunning)
        {
            InvalidateSurface();
            await Task.Delay(delay);
        }
    }

    public async void StartAnimation()
    {
        if (_stopwatch.IsRunning) return;
        _stopwatch.Start();
        await AnimationLoop();
    }

    public void StopAnimation() { _stopwatch.Stop(); }
#endregion
}

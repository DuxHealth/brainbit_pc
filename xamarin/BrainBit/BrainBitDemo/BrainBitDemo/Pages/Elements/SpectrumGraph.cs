using System;

using SkiaSharp;
using SkiaSharp.Views.Forms;

using Xamarin.Forms;

namespace BrainBitDemo.Pages.Elements;

internal class SpectrumGraph : SKCanvasView
{
#region Fields
    private SKPaint _axisPaint;
    private SKPaint _textPaint;

    private SKPaint _deltaChartPaint;
    private SKPaint _thetaChartPaint;
    private SKPaint _alphaChartPaint;
    private SKPaint _betaChartPaint;
    private SKPaint _otherChartPaint;

    private float _abscissa;

    private readonly SKColor _deltaColor = SKColor.Parse("D9BBA7");
    private readonly SKColor _thetaColor = SKColor.Parse("F3D1BA");
    private readonly SKColor _alphaColor = SKColor.Parse("F2F2BB");
    private readonly SKColor _betaColor  = SKColor.Parse("C8E6C9");
    private readonly SKColor _otherColor = SKColor.Parse("EBD6AD");

    private const int DeltaMaxBorder = 4;
    private const int ThetaMaxBorder = 8;
    private const int AlphaMaxBorder = 14;
    private const int BetaMaxBorder  = 34;

    private float _spaceForText;

    private readonly Point _padding = new(40.0f, 40.0f);
#endregion

#region Properties
    public int SampleCount { get; set; }

    private int _amplitude;

    public int Amplitude
    {
        get => _amplitude;

        set
        {
            if (_amplitude == value) return;

            _amplitude = value;

            OnPropertyChanged();
            InvalidateSurface();
        }
    }

    private double[] _entries;

    public double[] Entries
    {
        get => _entries;

        set
        {
            if (_entries == value) return;

            _entries = value;

            OnPropertyChanged();
            InvalidateSurface();
        }
    }

    private Color _backColor = Color.Black;

    public Color BackColor
    {
        get => _backColor;

        set
        {
            if (_backColor == value) return;

            _backColor = value;
            OnPropertyChanged();
        }
    }

    private Color _axisColor = Color.White;

    public Color AxisColor
    {
        get => _axisColor;

        set
        {
            if (_axisColor == value) return;

            _axisColor = value;
            OnPropertyChanged();
            InitAxisPaint();
        }
    }

    private Color _textColor = Color.White;

    public Color TextColor
    {
        get => _textColor;

        set
        {
            if (_textColor == value) return;

            _textColor = value;

            OnPropertyChanged();
            InitTextPaint();
        }
    }
#endregion

#region Init
    public SpectrumGraph()
    {
        Amplitude = 1000;

        InitAxisPaint();
        InitChartPaint();
        InitTextPaint();
        InvalidateSurface();
    }

    public void Init(int sampleCount, int ampWatt)
    {
        if (sampleCount <= 0) return;

        SampleCount = sampleCount;
        Amplitude   = ampWatt;

        _spaceForText = CanvasSize.Width * 0.1f + 40f;
    }
#endregion

#region Paint Initializers
    private void InitAxisPaint() { _axisPaint = new SKPaint { Style = SKPaintStyle.Stroke, Color = AxisColor.ToSKColor(), IsAntialias = true, StrokeWidth = 2 }; }

    private void InitChartPaint()
    {
        _deltaChartPaint = new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = _deltaColor, IsAntialias = true, StrokeWidth = 1 };

        _thetaChartPaint = new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = _thetaColor, IsAntialias = true, StrokeWidth = 1 };

        _alphaChartPaint = new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = _alphaColor, IsAntialias = true, StrokeWidth = 1 };

        _betaChartPaint = new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = _betaColor, IsAntialias = true, StrokeWidth = 1 };

        _otherChartPaint = new SKPaint { Style = SKPaintStyle.StrokeAndFill, Color = _otherColor, IsAntialias = true, StrokeWidth = 1 };
    }

    private void InitTextPaint()
    {
        _textPaint = new SKPaint
        {
            Style       = SKPaintStyle.Fill,
            Color       = TextColor.ToSKColor(),
            TextSize    = 28.0f,
            IsAntialias = true,
            TextAlign   = SKTextAlign.Center
        };
    }
#endregion

#region Drawing
    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        SKSurface surface = e.Surface;
        SKCanvas  canvas  = surface.Canvas;

        _abscissa = CanvasSize.Height - _padding.Bottom;

        canvas.Clear(BackColor.ToSKColor());

        DrawGraph(canvas);
        DrawAxis(canvas);
        DrawLabel(canvas);
    }

    private void DrawGraph(SKCanvas canvas)
    {
        if (Entries == null || Entries.Length == 0) return;

        var path = new SKPath();

        float leftPaddingWithText = _padding.Left + _spaceForText;

        // Step to move along the X axis
        float xAxisStep = (CanvasSize.Width - (leftPaddingWithText + _padding.Right)) / SampleCount;

        // Step to move along the Y axis
        float yAxisStep = (CanvasSize.Height - (_padding.Bottom + _padding.Top)) / (Amplitude * 2.0f);

        // Start chart in (0, Entries[0])
        float yStart = _abscissa - yAxisStep * (float)Entries[0];

        if (yStart < _padding.Top) yStart = _padding.Top;

        path.MoveTo(leftPaddingWithText, _abscissa);
        path.LineTo(leftPaddingWithText, yStart);

        // Move along the X axis to the right and create points for values of Entries
        for (var i = 0; i < SampleCount; i++)
        {
            float x = leftPaddingWithText + xAxisStep * i;
            float y = _abscissa - yAxisStep * (float)Entries[i];

            if (y > CanvasSize.Height - _padding.Bottom) y = CanvasSize.Height - _padding.Bottom;

            if (y < _padding.Top) y = _padding.Top;

            switch (i)
            {
            case DeltaMaxBorder:
                // Draw last line from [x, y] to [x, 0] and close path
                path.LineTo(x, y);
                path.LineTo(x, _abscissa);
                path.Close();

                canvas.DrawPath(path, _deltaChartPaint);

                // Start new path from [x, 0]
                path.Rewind();
                path.MoveTo(x, _abscissa);
                break;
            case ThetaMaxBorder:
                // Draw last line from [x, y] to [x, 0] and close path
                path.LineTo(x, y);
                path.LineTo(x, _abscissa);
                path.Close();

                canvas.DrawPath(path, _thetaChartPaint);

                // Start new path from [x, 0]
                path.Rewind();
                path.MoveTo(x, _abscissa);
                break;
            case AlphaMaxBorder:
                // Draw last line from [x, y] to [x, 0] and close path
                path.LineTo(x, y);
                path.LineTo(x, _abscissa);
                path.Close();

                canvas.DrawPath(path, _alphaChartPaint);

                // Start new path from [x, 0]
                path.Rewind();
                path.MoveTo(x, _abscissa);
                break;
            case BetaMaxBorder:
                // Draw last line from [x, y] to [x, 0] and close path
                path.LineTo(x, y);
                path.LineTo(x, _abscissa);
                path.Close();

                canvas.DrawPath(path, _betaChartPaint);

                // Start new path from [x, 0]
                path.Rewind();
                path.MoveTo(x, _abscissa);
                break;
            }

            path.LineTo(x, y);
        }

        // To end fill we need to end line on (x, 0) where x is the leftmost point on chart
        path.LineTo(leftPaddingWithText + xAxisStep * (SampleCount - 1), _abscissa);
        path.Close();

        canvas.DrawPath(path, _otherChartPaint);
    }

    private void DrawAxis(SKCanvas canvas)
    {
        float x         = _padding.Left + _spaceForText;
        float xAxisStep = (CanvasSize.Width - (x + _padding.Right)) / SampleCount;

        // Draw Y axis
        canvas.DrawLine(x, _padding.Top, x, CanvasSize.Height - _padding.Bottom, _axisPaint);

        // Draw small line at the top of Y axis to look like "Г"
        canvas.DrawLine(x, _padding.Top, x * 1.2f, _padding.Top, _axisPaint);

        // Draw top part of X axis. But why?
        canvas.DrawLine(x, CanvasSize.Height - _padding.Bottom, x * 1.2f, CanvasSize.Height - _padding.Bottom, _axisPaint);

        // Draw bottom part of X axis. But why?
        canvas.DrawLine(x, _abscissa, x + xAxisStep * (SampleCount - 1), _abscissa, _axisPaint);
    }

    private void DrawLabel(SKCanvas canvas)
    {
        float labelYDelta = (CanvasSize.Height - (_padding.Top + _padding.Bottom)) / 2;

        _textPaint.TextSize = CanvasSize.Width * .025f;

        var ampTextRect = new SKRect();

        float x = _padding.Left + _spaceForText;

        const string mW                = "mW";
        var          amplitudeText     = $"{Amplitude} {mW}";
        string       halfAmplitudeText = Amplitude / 2.0f != .0f ? $"{Math.Round(Amplitude / 2.0f, 1)} {mW}" : $"{Amplitude / 2} {mW}";
        const string zeroMw            = $"0 {mW}";

        _textPaint.MeasureText(amplitudeText, ref ampTextRect);

        canvas.DrawText(amplitudeText,     x - ampTextRect.Width, _padding.Top + _textPaint.TextSize / 2,                        _textPaint);
        canvas.DrawText(halfAmplitudeText, x - ampTextRect.Width, _padding.Top + labelYDelta + _textPaint.TextSize / 2,          _textPaint);
        canvas.DrawText(zeroMw,            x - ampTextRect.Width, CanvasSize.Height - _padding.Bottom + _textPaint.TextSize / 2, _textPaint);

        float leftPaddingWithText = _padding.Left + _spaceForText;
        float xAxisStep           = (CanvasSize.Width - (leftPaddingWithText + _padding.Right)) / SampleCount;

        canvas.DrawText(DeltaMaxBorder.ToString(), leftPaddingWithText + xAxisStep * DeltaMaxBorder, _abscissa + _spaceForText / 2, _textPaint);
        canvas.DrawText(ThetaMaxBorder.ToString(), leftPaddingWithText + xAxisStep * ThetaMaxBorder, _abscissa + _spaceForText / 2, _textPaint);
        canvas.DrawText(AlphaMaxBorder.ToString(), leftPaddingWithText + xAxisStep * AlphaMaxBorder, _abscissa + _spaceForText / 2, _textPaint);
        canvas.DrawText(BetaMaxBorder.ToString(),  leftPaddingWithText + xAxisStep * BetaMaxBorder,  _abscissa + _spaceForText / 2, _textPaint);

        canvas.DrawText(SampleCount.ToString(), leftPaddingWithText + xAxisStep * SampleCount, _abscissa + _spaceForText / 2, _textPaint);
    }
#endregion
}

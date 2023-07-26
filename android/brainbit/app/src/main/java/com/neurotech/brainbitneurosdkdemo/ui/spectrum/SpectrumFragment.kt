package com.neurotech.brainbitneurosdkdemo.ui.spectrum

import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.graphics.Color
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import com.androidplot.xy.LineAndPointFormatter
import com.androidplot.xy.SimpleXYSeries
import com.androidplot.xy.XYPlot
import com.neurotech.brainbitneurosdkdemo.R
import com.neurotech.brainbitneurosdkdemo.databinding.FragmentSpectrumBinding
import com.neurotech.brainbitneurosdkdemo.spectrum.SignalChannels

class SpectrumFragment : Fragment() {
    private lateinit var viewModel: SpectrumViewModel
    private lateinit var binding: FragmentSpectrumBinding

    private lateinit var plotO1: XYPlot
    private lateinit var plotO2: XYPlot
    private lateinit var plotT3: XYPlot
    private lateinit var plotT4: XYPlot

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?
    ): View {
        binding = FragmentSpectrumBinding.inflate(inflater, container, false)

        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        viewModel = ViewModelProvider(this)[SpectrumViewModel::class.java]

        binding.signalButton.setOnClickListener {
            viewModel.onSignalClicked()
        }

        binding.viewModel = viewModel

        plotO1 = binding.spectrumPlotO1
        plotO2 = binding.spectrumPlotO2
        plotT3 = binding.spectrumPlotT3
        plotT4 = binding.spectrumPlotT4

        plotO1.graph.gridBackgroundPaint.color = Color.TRANSPARENT
        plotO1.graph.backgroundPaint.color = Color.TRANSPARENT
        plotO1.backgroundPaint.color = Color.TRANSPARENT
        plotO1.borderPaint.color = Color.TRANSPARENT

        plotO2.graph.gridBackgroundPaint.color = Color.TRANSPARENT
        plotO2.graph.backgroundPaint.color = Color.TRANSPARENT
        plotO2.backgroundPaint.color = Color.TRANSPARENT
        plotO2.borderPaint.color = Color.TRANSPARENT

        plotT3.graph.gridBackgroundPaint.color = Color.TRANSPARENT
        plotT3.graph.backgroundPaint.color = Color.TRANSPARENT
        plotT3.backgroundPaint.color = Color.TRANSPARENT
        plotT3.borderPaint.color = Color.TRANSPARENT

        plotT4.graph.gridBackgroundPaint.color = Color.TRANSPARENT
        plotT4.graph.backgroundPaint.color = Color.TRANSPARENT
        plotT4.backgroundPaint.color = Color.TRANSPARENT
        plotT4.borderPaint.color = Color.TRANSPARENT


        val formatter = LineAndPointFormatter(context, R.xml.line_point_formatter)
        formatter.isLegendIconEnabled = false

        val spectrumObserver = Observer<Map<SignalChannels, List<Double>>> {
            plotO1.clear()
            plotO2.clear()
            plotT3.clear()
            plotT4.clear()

            plotO1.addSeries(
                SimpleXYSeries(
                    it.getValue(SignalChannels.O1), SimpleXYSeries.ArrayFormat.Y_VALS_ONLY, "O1"
                ), formatter
            )

            plotO2.addSeries(
                SimpleXYSeries(
                    it.getValue(SignalChannels.O2), SimpleXYSeries.ArrayFormat.Y_VALS_ONLY, "O2"
                ), formatter
            )

            plotT3.addSeries(
                SimpleXYSeries(
                    it.getValue(SignalChannels.T3), SimpleXYSeries.ArrayFormat.Y_VALS_ONLY, "T3"
                ), formatter
            )

            plotT4.addSeries(
                SimpleXYSeries(
                    it.getValue(SignalChannels.T4), SimpleXYSeries.ArrayFormat.Y_VALS_ONLY, "T4"
                ), formatter
            )

            plotO1.redraw()
            plotO2.redraw()
            plotT3.redraw()
            plotT4.redraw()
        }
        viewModel.spectrumBins.observe(requireActivity(), spectrumObserver)

        val alphaObserver = Observer<Int> {
            Log.i(::SpectrumFragment.name, "alpha: $it")
        }

        val betaObserver = Observer<Int> {
            Log.i(::SpectrumFragment.name, "beta: $it")
        }

        val deltaObserver = Observer<Int> {
            Log.i(::SpectrumFragment.name, "delta: $it")
        }

        val thetaObserver = Observer<Int> {
            Log.i(::SpectrumFragment.name, "theta: $it")
        }

        val gammaObserver = Observer<Int> {
            Log.i(::SpectrumFragment.name, "gamma: $it")
        }

        viewModel.alpha.observe(requireActivity(), alphaObserver)
        viewModel.beta.observe(requireActivity(), betaObserver)
        viewModel.gamma.observe(requireActivity(), gammaObserver)
        viewModel.delta.observe(requireActivity(), deltaObserver)
        viewModel.theta.observe(requireActivity(), thetaObserver)
    }

    override fun onDestroyView() {
        super.onDestroyView()

        viewModel.close()
    }
}
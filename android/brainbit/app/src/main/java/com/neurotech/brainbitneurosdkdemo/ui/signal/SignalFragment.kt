package com.neurotech.brainbitneurosdkdemo.ui.signal

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import androidx.databinding.DataBindingUtil
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import com.neurotech.brainbitneurosdkdemo.R
import com.neurotech.brainbitneurosdkdemo.databinding.SignalFragmentBinding
import com.neurotech.brainbitneurosdkdemo.ui.helpers.PlotHolder

class SignalFragment : Fragment() {

    companion object {
        fun newInstance() = SignalFragment()
    }

    private lateinit var binding: SignalFragmentBinding
    private lateinit var viewModel: SignalViewModel

    private var plotO1: PlotHolder? = null
    private var plotO2: PlotHolder? = null
    private var plotT3: PlotHolder? = null
    private var plotT4: PlotHolder? = null

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?
    ): View {
        binding = DataBindingUtil.inflate(inflater, R.layout.signal_fragment, container, false)
        viewModel = ViewModelProvider(this)[SignalViewModel::class.java]
        binding.viewModel = viewModel
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)
        binding.signalButton.setOnClickListener { viewModel.onSignalClicked() }

        initPlots()
    }

    fun initPlots() {

        plotO1 = PlotHolder(binding.signalPlotO1)
        plotO1?.startRender(250.0f, PlotHolder.ZoomVal.V_AUTO_M_S2, 5.0f)
        val samplesO1Observer = Observer<List<Double>> { newSamples ->
            plotO1?.addData(newSamples)
        }
        viewModel.samplesO1.observe(requireActivity(), samplesO1Observer)

        plotO2 = PlotHolder(binding.signalPlotO2)
        plotO2?.startRender(250.0f, PlotHolder.ZoomVal.V_AUTO_M_S2, 5.0f)
        val samplesO2Observer = Observer<List<Double>> { newSamples ->
            plotO2?.addData(newSamples)
        }
        viewModel.samplesO2.observe(requireActivity(), samplesO2Observer)

        plotT3 = PlotHolder(binding.signalPlotT3)
        plotT3?.startRender(250.0f, PlotHolder.ZoomVal.V_AUTO_M_S2, 5.0f)
        val samplesT3Observer = Observer<List<Double>> { newSamples ->
            plotT3?.addData(newSamples)
        }
        viewModel.samplesT3.observe(requireActivity(), samplesT3Observer)

        plotT4 = PlotHolder(binding.signalPlotT4)
        plotT4?.startRender(250.0f, PlotHolder.ZoomVal.V_AUTO_M_S2, 5.0f)
        val samplesT4Observer = Observer<List<Double>> { newSamples ->
            plotT4?.addData(newSamples)
        }
        viewModel.samplesT4.observe(requireActivity(), samplesT4Observer)
    }

    override fun onDestroyView() {
        super.onDestroyView()

        viewModel.close()
    }
}
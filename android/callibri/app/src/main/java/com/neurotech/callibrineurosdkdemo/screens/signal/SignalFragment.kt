package com.neurotech.callibrineurosdkdemo.screens.signal

import androidx.lifecycle.ViewModelProvider
import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import androidx.lifecycle.Observer
import com.neurosdk2.neuro.types.CallibriSignalType
import com.neurotech.callibrineurosdkdemo.callibri.CallibriController
import com.neurotech.callibrineurosdkdemo.databinding.FragmentSignalBinding
import com.neurotech.callibrineurosdkdemo.utils.PlotHolder

class SignalFragment : Fragment() {

    companion object {
        fun newInstance() = SignalFragment()
    }

    private var _binding: FragmentSignalBinding? = null
    private var _viewModel: SignalViewModel? = null

    private val binding get() = _binding!!
    private val viewModel get() = _viewModel!!

    private var plot: PlotHolder? = null

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentSignalBinding.inflate(inflater, container, false)
        _viewModel = ViewModelProvider(this)[SignalViewModel::class.java]

        binding.viewModel = viewModel

        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        binding.signalButton.setOnClickListener { viewModel.onSignalClicked() }

        plot = PlotHolder(binding.plotSignal)
        CallibriController.samplingFrequency?.let { plot?.startRender(it, PlotHolder.ZoomVal.V_AUTO_M_S2, 5.0f) }

        val samplesObserver = Observer<List<Double>> { newSamples ->
            plot?.addData(newSamples)
        }
        viewModel.samples.observe(requireActivity(), samplesObserver)

        signalTypeSpinnerImpl()
    }

    private fun signalTypeSpinnerImpl(){

        ArrayAdapter(
            requireContext(),
            android.R.layout.simple_spinner_item,
            CallibriSignalType.values()
        ).also {
            it.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item)
            binding.spinner.adapter = it
            CallibriController.signalType?.index()?.let { it1 -> binding.spinner.setSelection(it1) }
            binding.spinner.onItemSelectedListener = object : AdapterView.OnItemSelectedListener{
                override fun onNothingSelected(parent: AdapterView<*>?) {
                }

                override fun onItemSelected(parent: AdapterView<*>?, view: View?, position: Int, id: Long) {
                    if (CallibriController.signalType != CallibriSignalType.indexOf(position)) {
                        CallibriController.signalType = CallibriSignalType.indexOf(position)
                    }
                }

            }
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null

        viewModel.close()
    }

}
package com.neurotech.brainbitneurosdkdemo.ui.spectrum

import androidx.databinding.ObservableBoolean
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController
import com.neurotech.brainbitneurosdkdemo.spectrum.SignalChannels
import com.neurotech.brainbitneurosdkdemo.spectrum.SpectrumController
import kotlin.math.roundToInt

class SpectrumViewModel : ViewModel() {
    var started = ObservableBoolean(false)

    private val spectrumController = SpectrumController()

    private val _spectrumBins: MutableLiveData<Map<SignalChannels, List<Double>>> by lazy {
        MutableLiveData<Map<SignalChannels, List<Double>>>()
    }
    val spectrumBins: LiveData<Map<SignalChannels, List<Double>>> = _spectrumBins

    val sf = 250.div(spectrumController.fftBinsFor1Hz).toFloat()

    private val _alpha: MutableLiveData<Int> by lazy {
        MutableLiveData<Int>()
    }
    var alpha: LiveData<Int> = _alpha

    private val _beta: MutableLiveData<Int> by lazy {
        MutableLiveData<Int>()
    }
    var beta: LiveData<Int> = _beta

    private val _delta: MutableLiveData<Int> by lazy {
        MutableLiveData<Int>()
    }
    var delta: LiveData<Int> = _delta

    private val _theta: MutableLiveData<Int> by lazy {
        MutableLiveData<Int>()
    }
    var theta: LiveData<Int> = _theta

    private val _gamma: MutableLiveData<Int> by lazy {
        MutableLiveData<Int>()
    }
    var gamma: LiveData<Int> = _gamma

    fun onSignalClicked() {
        if (started.get()) {
            BrainBitController.stopSignal()
        } else {
            startSignal()
        }

        started.set(!started.get())
    }

    private fun startSignal() {
        spectrumController.processedSpectrum = {
            val res = mutableMapOf<SignalChannels, List<Double>>()
            var hasSpectrum = true
            for (channel in it.keys) {
                val temp = mutableListOf<Double>()

                for (sample in it.getValue(channel)) {
                    temp.addAll(sample.allBinsValues.toList())
                }
                if (temp.isEmpty()) {
                    hasSpectrum = false
                    break
                }
                res[channel] = temp
            }

            if (hasSpectrum) {
                _spectrumBins.postValue(res.toMap())

            }
        }

        spectrumController.processedWaves = {
            for (channel in SignalChannels.values()) {
                for (sample in it[channel]!!) {
                    _alpha.postValue((sample.alpha_rel * 100).roundToInt())
                    _beta.postValue((sample.beta_rel * 100).roundToInt())
                    _delta.postValue((sample.delta_rel * 100).roundToInt())
                    _theta.postValue((sample.theta_rel * 100).roundToInt())
                    _gamma.postValue((sample.gamma_rel * 100).roundToInt())
                }
            }
        }

        BrainBitController.startSignal {
            val res = mutableMapOf<SignalChannels, DoubleArray>()

            val o1 = mutableListOf<Double>()
            val o2 = mutableListOf<Double>()
            val t3 = mutableListOf<Double>()
            val t4 = mutableListOf<Double>()

            for (sample in it) {
                o1.add(sample.o1)
                o2.add(sample.o2)
                t3.add(sample.t3)
                t4.add(sample.t4)
            }

            res[SignalChannels.O1] = o1.toDoubleArray()
            res[SignalChannels.O2] = o2.toDoubleArray()
            res[SignalChannels.T3] = t3.toDoubleArray()
            res[SignalChannels.T4] = t4.toDoubleArray()

            spectrumController.processSamples(res.toMap())
        }
    }

    fun close() {
        BrainBitController.stopSignal()

        spectrumController.processedSpectrum = {}
        spectrumController.processedWaves = {}
    }
}
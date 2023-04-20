package com.neurotech.callibrineurosdkdemo.screens.signal

import androidx.databinding.ObservableBoolean
import androidx.databinding.ObservableField
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.neurosdk2.neuro.types.CallibriElectrodeState
import com.neurotech.callibrineurosdkdemo.callibri.CallibriController

class SignalViewModel : ViewModel() {
    var started = ObservableBoolean(false)

    val samples: MutableLiveData<List<Double>> by lazy {
        MutableLiveData<List<Double>>()
    }

    var electrodesState = ObservableField<String>()
    fun onSignalClicked(){
        if(started.get()){
            CallibriController.stopSignal()
            CallibriController.stopElectrodes()
        }
        else{
            CallibriController.startSignal(signalReceived = {
                val res = mutableListOf<Double>()
                for (sample in it) {
                    res.addAll(sample.samples.toList())
                }
                samples.postValue(res)
            })
            CallibriController.startElectrodes ( electrodesStateChanged = {
                when (it) {
                    CallibriElectrodeState.Normal-> electrodesState.set("Normal")
                    CallibriElectrodeState.Detached -> electrodesState.set("Detached")
                    CallibriElectrodeState.HighResistance -> electrodesState.set("HighResistance")
                }
                }
            )
        }
        started.set(!started.get())
    }

    fun close(){
        CallibriController.stopSignal()
        CallibriController.stopElectrodes()
    }
}
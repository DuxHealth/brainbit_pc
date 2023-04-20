package com.neurotech.brainbitneurosdkdemo.ui.signal

import androidx.databinding.ObservableBoolean
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController

class SignalViewModel : ViewModel() {
    var started = ObservableBoolean(false)

    val samplesO1: MutableLiveData<List<Double>> by lazy {
        MutableLiveData<List<Double>>()
    }
    val samplesO2: MutableLiveData<List<Double>> by lazy {
        MutableLiveData<List<Double>>()
    }
    val samplesT3: MutableLiveData<List<Double>> by lazy {
        MutableLiveData<List<Double>>()
    }
    val samplesT4: MutableLiveData<List<Double>> by lazy {
        MutableLiveData<List<Double>>()
    }

    fun onSignalClicked(){
        if(started.get()){
            BrainBitController.stopSignal()
        }
        else{
            BrainBitController.startSignal(signalReceived = {
                val dataO1 = mutableListOf<Double>()
                val dataO2 = mutableListOf<Double>()
                val dataT3 = mutableListOf<Double>()
                val dataT4 = mutableListOf<Double>()
                for(data in it){
                    dataO1.add(data.o1)
                    dataO2.add(data.o2)
                    dataT3.add(data.t3)
                    dataT4.add(data.t4)
                }
                samplesO1.postValue(dataO1)
                samplesO2.postValue(dataO2)
                samplesT3.postValue(dataT3)
                samplesT4.postValue(dataT4)
            })
        }
        started.set(!started.get())
    }

    fun close(){
        BrainBitController.stopSignal()
    }
}
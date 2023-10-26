package com.neurotech.brainbitneurosdkdemo.ui.signal

import androidx.databinding.ObservableBoolean
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.neurotech.brainbitneurosdkdemo.filters.FiltersMath
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController
import com.neurotech.filters.FilterParam

class SignalViewModel : ViewModel() {
    private var filtersMath = FiltersMath()

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
                    dataO1.add(filtersMath.filterO1(data.o1))
                    dataO2.add(filtersMath.filterO2(data.o2))
                    dataT3.add(filtersMath.filterT3(data.t3))
                    dataT4.add(filtersMath.filterT4(data.t4))
                }
                samplesO1.postValue(dataO1)
                samplesO2.postValue(dataO2)
                samplesT3.postValue(dataT3)
                samplesT4.postValue(dataT4)
            })
        }
        started.set(!started.get())
    }

    fun addFilter(fParam: FilterParam) {
        filtersMath.addFilter(fParam)
    }

    fun removeFilter(fParam: FilterParam) {
        filtersMath.removeFilter(fParam)
    }
    fun close(){
        BrainBitController.stopSignal()
    }
}
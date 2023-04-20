package com.neurotech.brainbitneurosdkdemo.ui.search

import androidx.databinding.*
import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController
import com.neurosdk2.neuro.types.SensorInfo

class SearchViewModel : ViewModel(){

    var started = ObservableBoolean(false)

    private val _sensors = MutableLiveData<List<SensorInfo>>()
    val sensors: LiveData<List<SensorInfo>> get() = _sensors

    fun onSearchClicked(){
        if(started.get()){
            BrainBitController.stopSearch()
        }
        else{
            BrainBitController.startSearch(sensorsChanged = { scanner, infos ->
                run {
                    _sensors.postValue(infos)
                }
            })
        }
        started.set(!started.get())
    }

    fun close(){
        BrainBitController.stopSearch()
    }

}


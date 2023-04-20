package com.neurotech.brainbitneurosdkdemo.ui.resist

import androidx.databinding.ObservableBoolean
import androidx.databinding.ObservableField
import androidx.lifecycle.ViewModel
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController

class ResistViewModel : ViewModel() {

    var started = ObservableBoolean(false)

    val sampleO1 = ObservableField<String>()
    val sampleO2 = ObservableField<String>()
    val sampleT3 = ObservableField<String>()
    val sampleT4 = ObservableField<String>()


    fun onResistClicked(){
        if(started.get()){
            BrainBitController.stopResist()
        }
        else{
            BrainBitController.startResist {
                sampleO1.set(it.o1.toString())
                sampleO2.set(it.o2.toString())
                sampleT3.set(it.t3.toString())
                sampleT4.set(it.t4.toString())
            }
        }
        started.set(!started.get())
    }

    fun close(){
        BrainBitController.stopResist()
    }
}
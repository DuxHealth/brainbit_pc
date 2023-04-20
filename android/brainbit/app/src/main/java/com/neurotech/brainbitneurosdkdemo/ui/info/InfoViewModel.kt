package com.neurotech.brainbitneurosdkdemo.ui.info

import androidx.databinding.ObservableField
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController

class InfoViewModel : ViewModel() {
    var infoText = ObservableField(BrainBitController.fullInfo())
}
package com.neurotech.brainbitneurosdkdemo.ui.menu

import androidx.databinding.ObservableBoolean
import androidx.lifecycle.ViewModel
import com.neurosdk2.neuro.types.SensorState
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController


class MenuViewModel : ViewModel() {

    var connected = ObservableBoolean(true)
    var hasDevice = ObservableBoolean(BrainBitController.hasDevice)

    fun reconnect() {
        if (BrainBitController.connectionState == SensorState.StateInRange) {
            BrainBitController.disconnectCurrent()
            connected.set(false)
        } else {
            BrainBitController.connectCurrent(onConnectionResult = {
                connected.set(it == SensorState.StateInRange)
            })
        }
    }

    fun updateDeviceInfo() {
        connected.set(BrainBitController.connectionState == SensorState.StateInRange)
        hasDevice.set(BrainBitController.hasDevice)
    }

}
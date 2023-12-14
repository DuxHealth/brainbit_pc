package com.neurotech.brainbitneurosdkdemo.filters

import com.neurotech.filters.*
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController
import java.lang.Exception

class FiltersMath {
    val filtersLP = mutableMapOf<String, FilterParam>()
    val filtersHP = mutableMapOf<String, FilterParam>()
    val filtersBS = mutableMapOf<String, FilterParam>()

    private val preinstalledFilterList = PreinstalledFilterList.get()
    private val filterListO1 = mutableMapOf<FilterParam, Filter>()
    private val filterListO2 = mutableMapOf<FilterParam, Filter>()
    private val filterListT3 = mutableMapOf<FilterParam, Filter>()
    private val filterListT4 = mutableMapOf<FilterParam, Filter>()

    init {
        val brainBitSF = 250

        for (pf in preinstalledFilterList) {
            if (pf.samplingFreq != brainBitSF) continue

            if (pf.type == FilterType.FtBandPass || pf.type == FilterType.FtBandStop) {
                filtersBS["${pf.cutoffFreq} Hz"] = pf
            }

            if (pf.type == FilterType.FtHP) {
                filtersHP["${pf.cutoffFreq} Hz"] = pf
            }

            if (pf.type == FilterType.FtLP) {
                filtersLP["${pf.cutoffFreq} Hz"] = pf
            }
        }
    }

    fun addFilter(fParam: FilterParam) {
        try {
            filterListO1[fParam] = Filter(fParam)
            filterListO2[fParam] = Filter(fParam)
            filterListT3[fParam] = Filter(fParam)
            filterListT4[fParam] = Filter(fParam)
        } catch (ex: Exception){

        }

    }

    fun removeFilter(fParam: FilterParam) {
        filterListO1.remove(fParam)
        filterListO2.remove(fParam)
        filterListT3.remove(fParam)
        filterListT4.remove(fParam)
    }

    fun filterO1(sample: Double): Double {
        var result = sample
        filterListO1.forEach { entry ->
            result = entry.value.filter(result)
        }
        return result
    }

    fun filterO2(sample: Double): Double {
        var result = sample
        filterListO2.forEach { entry ->
            result = entry.value.filter(result)
        }
        return result
    }

    fun filterT3(sample: Double): Double {
        var result = sample
        filterListT3.forEach { entry ->
            result = entry.value.filter(result)
        }
        return result
    }

    fun filterT4(sample: Double): Double {
        var result = sample
        filterListT4.forEach { entry ->
            result = entry.value.filter(result)
        }
        return result
    }
}
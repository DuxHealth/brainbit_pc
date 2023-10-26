package com.neurotech.brainbitneurosdkdemo.filters

import com.neurotech.filters.*
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController
import java.lang.Exception

class FiltersMath {
    val filtersLP = mutableMapOf<String, FilterParam>()
    val filtersHP = mutableMapOf<String, FilterParam>()
    val filtersBS = mutableMapOf<String, FilterParam>()

    private val preinstalledFilterList = PreinstalledFilterList.get()
    private val tmpFilterList = mutableMapOf<FilterParam, Int>()
    private val filterListO1: FilterList = FilterList()
    private val filterListO2: FilterList = FilterList()
    private val filterListT3: FilterList = FilterList()
    private val filterListT4: FilterList = FilterList()

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
            val filter = Filter(fParam)
            tmpFilterList[fParam] = filter.id
            filterListO1.addFilter(Filter(fParam))
            filterListO2.addFilter(Filter(fParam))
            filterListT3.addFilter(Filter(fParam))
            filterListT4.addFilter(Filter(fParam))
        } catch (ex: Exception){

        }

    }

    fun removeFilter(fParam: FilterParam) {
        tmpFilterList[fParam]?.let {
            filterListO1.deleteFilter(it)
            filterListO2.deleteFilter(it)
            filterListT3.deleteFilter(it)
            filterListT4.deleteFilter(it)
        }
    }

    fun filterO1(sample: Double): Double {
        return filterListO1.filter(sample)
    }

    fun filterO2(sample: Double): Double {
        return filterListO2.filter(sample)
    }

    fun filterT3(sample: Double): Double {
        return filterListT3.filter(sample)
    }

    fun filterT4(sample: Double): Double {
        return filterListT4.filter(sample)
    }
}
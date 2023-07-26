package com.neurotech.brainbitneurosdkdemo.filters

import com.neurotech.filters.*
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController

class FiltersMath {
    val filtersLP = mutableMapOf<String, FilterParam>()
    val filtersHP = mutableMapOf<String, FilterParam>()
    val filtersBS = mutableMapOf<String, FilterParam>()

    private val preinstalledFilterList = PreinstalledFilterList.get()
    private val tmpFilterList = mutableMapOf<FilterParam, Int>()
    private val filterList: FilterList = FilterList()

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
        val filter = Filter(fParam)
        tmpFilterList[fParam] = filter.id
        filterList.addFilter(Filter(fParam))
    }

    fun removeFilter(fParam: FilterParam) {
        tmpFilterList[fParam]?.let { filterList.deleteFilter(it) }
    }

    fun filter(sample: Double): Double {
        return filterList.filter(sample)
    }
}
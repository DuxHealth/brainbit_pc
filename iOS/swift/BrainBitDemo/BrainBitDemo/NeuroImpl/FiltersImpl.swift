//
//  FiltersImpl.swift
//  BrainBitDemo
//
//  Created by Aseatari on 01.08.2023.
//

import Foundation
import filters

class DefaultFilters{
    private var preinstalledFilterList = FTPreinstalledFilters.getList()
    
    var filtersLP: [String: FTFilterParam] = [:]
    var filtersHP: [String: FTFilterParam] = [:]
    var filtersBS: [String: FTFilterParam] = [:]

        init () {
            let brainBitSF = 250
            for pf in preinstalledFilterList! {
                
                if(pf.samplingFreq != brainBitSF) { continue }

                if(pf.type == FTFilterType.FTBandPass || pf.type == FTFilterType.FTBandStop){
                    filtersBS["\(pf.cutoffFreq) Hz, \(pf.samplingFreq) Hz"] = pf
                }
                if(pf.type == FTFilterType.FTHP){
                    filtersHP["\(pf.cutoffFreq) Hz, \(pf.samplingFreq) Hz"] = pf
                }
                if(pf.type == FTFilterType.FTLP){
                    filtersLP["\(pf.cutoffFreq) Hz, \(pf.samplingFreq) Hz"] = pf
                }
            }
        }
}

class FiltersImpl{
    
    private var tmpFilterList: [FTFilterParam: TFilterID] = [:]
    private var filterList: FTFilterList = FTFilterList.init()

    func addFilter(fParam: FTFilterParam){
        var filter = FTFilter(param: fParam)
        tmpFilterList[fParam] = filter.getID()
        filterList.add(filter)
    }

    func removeFilter(fParam: FTFilterParam){
        guard let fId = tmpFilterList[fParam] else { return }
        filterList.deleteFilter(fId)
    }

    func filter(samples: [NSNumber]) -> [NSNumber]{
        return filterList.filterArray(samples)
    }

}

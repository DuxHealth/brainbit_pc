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
            preinstalledFilterList?.sort(by: { first, second in
                return first.cutoffFreq < second.cutoffFreq
            })
            for pf in preinstalledFilterList! {
                
                if(pf.samplingFreq != brainBitSF) { continue }

                if(pf.type == FTFilterType.FTBandPass || pf.type == FTFilterType.FTBandStop){
                    filtersBS["\(pf.cutoffFreq) Hz"] = pf
                }
                if(pf.type == FTFilterType.FTHP){
                    filtersHP["\(pf.cutoffFreq) Hz"] = pf
                }
                if(pf.type == FTFilterType.FTLP){
                    filtersLP["\(pf.cutoffFreq) Hz"] = pf
                }
            }
        }
}

class FiltersImpl{
    
    private var tmpFilterList: [FTFilterParam: TFilterID] = [:]
    private var filterList: [FTFilter] = []

    func addFilter(fParam: FTFilterParam){
        let filter = FTFilter(param: fParam)
        tmpFilterList[fParam] = filter.getID()
        filterList.append(filter)
    }

    func removeFilter(fParam: FTFilterParam){
        guard let fId = tmpFilterList[fParam] else { return }
        filterList.removeAll { filter in
            return filter.getID() == fId
        }
    }

    func filter(samples: [NSNumber]) -> [NSNumber]{
        var result = samples
        filterList.forEach { filter in
            result = filter.filterArray(result)
        }
        return result
    }

}

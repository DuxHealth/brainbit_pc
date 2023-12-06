//
//  EmotionsMonopolar.swift
//  BrainBitDemo
//
//  Created by Aseatari on 30.11.2023.
//

import Foundation
import EmStArtifacts
import neurosdk2

class EmotionsMonopolar {
    
    typealias RawSpectralChannelDataCallback = (_ spectralDataPercents: EMRawSpectVals, _ channel: String) -> Void
    typealias IsArtifactedChannelCallback = (_ artefacted: Bool, _ channel: String) -> Void
    typealias ProgressChannelCallback = (_ progress: UInt32, _ channel: String) -> Void
    
    var emotions: [String: EMEmotionalMath] = [:]
    var isEmotionalMathInited = false
    
    private let queue = DispatchQueue(label: "thread-safe-samples", attributes: .concurrent)
    
    private let mathLibSetting = EMMathLibSetting.init(samplingRate: 250,
                                                  andProcessWinFreq: 25,
                                                  andFftWindow: 1000,
                                                  andNFirstSecSkipped: 4,
                                                  andBipolarMode: false,
                                                  andSquaredSpectrum: true,
                                                  andChannelsNumber: 1,
                                                  andChannelForAnalysis: 0)
    private let artifactDetectSetting = EMArtifactDetectSetting.init(artBord: 110,
                                                                andAllowedPercentArtpoints: 70,
                                                                andRawBetapLimit: 800000,
                                                                andTotalPowBorder: 80000000,
                                                                andGlobalArtwinSec: 4,
                                                                andSpectArtByTotalp: true,
                                                                andHanningWinSpectrum: false,
                                                                andHammingWinSpectrum: true,
                                                                andNumWinsForQualityAvg: 125)

    private let shortArtifactDetectSetting = ShortArtifactDetectSetting(ampl_art_detect_win_size: 200,
                                                                        ampl_art_zerod_area: 200,
                                                                        ampl_art_extremum_border: 25)
    private let mentalAndSpectralSetting = MentalAndSpectralSetting(n_sec_for_instant_estimation: 2, n_sec_for_averaging: 1)
    
    var isCalibrated = false
    
    var calibrationProgressCallback: ProgressChannelCallback?
    var showIsArtifactedCallback: IsArtifactedChannelCallback?
    var showRawSpectralDataCallback: RawSpectralChannelDataCallback?
    
    public func initEmotionMath() {
        if(isEmotionalMathInited){
            emotions["O1"] = nil;
            emotions["O2"] = nil;
            emotions["T3"] = nil;
            emotions["T4"] = nil;
            isEmotionalMathInited = false
            isCalibrated = false
        }
        if ( !isEmotionalMathInited ) {
            emotions["O1"] = EMEmotionalMath(libSettings: mathLibSetting,
                                             andArtifactDetetectSettings: artifactDetectSetting,
                                             andShortArtifactDetectSettigns: shortArtifactDetectSetting,
                                             andMentalAndSpectralSettings: mentalAndSpectralSetting)
            emotions["O2"] = EMEmotionalMath(libSettings: mathLibSetting,
                                             andArtifactDetetectSettings: artifactDetectSetting,
                                             andShortArtifactDetectSettigns: shortArtifactDetectSetting,
                                             andMentalAndSpectralSettings: mentalAndSpectralSetting)
            emotions["T3"] = EMEmotionalMath(libSettings: mathLibSetting,
                                             andArtifactDetetectSettings: artifactDetectSetting,
                                             andShortArtifactDetectSettigns: shortArtifactDetectSetting,
                                             andMentalAndSpectralSettings: mentalAndSpectralSetting)
            emotions["T4"] = EMEmotionalMath(libSettings: mathLibSetting,
                                             andArtifactDetetectSettings: artifactDetectSetting,
                                             andShortArtifactDetectSettigns: shortArtifactDetectSetting,
                                             andMentalAndSpectralSettings: mentalAndSpectralSetting)
            
            for (_, value) in emotions {
                value.setSkipWinsAfterArtifact(10)
                value.setZeroSpectWavesWithActive(true, andDelta: 0, andTheta: 1, andAlpha: 1, andBeta: 1, andGamma: 0)
                value.setSpectNormalizationByBandsWidth(true)
                value.startCalibration()
            }
            isEmotionalMathInited = true
            
        }
    }
    
    public func processData(bbData: [NTBrainBitSignalData]){
        queue.async(flags: .barrier) { [self] in
            
            var o1Values: [[NSNumber]] = [[]]
            var o2Values: [[NSNumber]] = [[]]
            var t3Values: [[NSNumber]] = [[]]
            var t4Values: [[NSNumber]] = [[]]
            o1Values.removeAll()
            o2Values.removeAll()
            t3Values.removeAll()
            t4Values.removeAll()
            for sample in bbData{
                o1Values.append([sample.o1])
                o2Values.append([sample.o2])
                t3Values.append([sample.t3])
                t4Values.append([sample.t4])
            }
            
            do{
                try ObjcEx.catchException {
                    self.emotions["O1"]?.pushDataArr(o1Values)
                    self.emotions["O2"]?.pushDataArr(o2Values)
                    self.emotions["T3"]?.pushDataArr(t3Values)
                    self.emotions["T4"]?.pushDataArr(t4Values)
                }
                
                
                for (key, value) in emotions {
                    try ObjcEx.catchException {
                        value.processDataArr()
                    }
                    getIsAtifacted(math: value, channel: key)
                    
                    if (!isCalibrated) {
                        processCalibration(math: value, channel: key)
                    }
                    else {
                        getRawSpectralData(math: value, channel: key)
                    }
                }
            }catch let error {
                print("Error: \(error.localizedDescription)")
            }
            
            
        }
    }
    
    private func processCalibration(math: EMEmotionalMath, channel: String) {
        if (math.calibrationFinished()) {
            isCalibrated = true
        }
        let progress = math.getCallibrationPercents()
        calibrationProgressCallback?(progress, channel)
    }
      
    private func getRawSpectralData(math: EMEmotionalMath, channel: String) {
        let spectralData = math.readRawSpectralVals()
        if((spectralData?.alpha)! > 0 ||
           (spectralData?.beta)! > 0){
            showRawSpectralDataCallback?(spectralData!, channel)
        }
    }
    
    private func getIsAtifacted(math: EMEmotionalMath, channel: String) {
        let tmp = isCalibrated ? math.isArtifactedSequence() : math.isBothSidesArtifacted()
        showIsArtifactedCallback?(tmp, channel)
    }
}

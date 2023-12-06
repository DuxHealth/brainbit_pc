//
//  EmotionsImpl.swift
//  BrainBitDemo
//
//  Created by Aseatari on 01.08.2023.
//

import Foundation
import EmStArtifacts

typealias LastSpectralDataCallback = (_ spectralDataPercents: EMSpectralDataPercents) -> Void
typealias LastMindDataCallback = (_ mindData: EMMindData) -> Void
typealias IsArtifactedCallback = (_ artefacted: Bool) -> Void
typealias ProgressCallback = (_ progress: UInt32) -> Void

class EmotionsImpl {
    
    var emotionalMath : EMEmotionalMath?
    var isEmotionalMathInited = false
    
    private let mathLibSetting = EMMathLibSetting(samplingRate: 250,
                                                  andProcessWinFreq: 25,
                                                  andFftWindow: 1000,
                                                  andNFirstSecSkipped: 4,
                                                  andBipolarMode: true,
                                                  andSquaredSpectrum: true,
                                                  andChannelsNumber: 1,//4
                                                  andChannelForAnalysis: 0)
    
    private let artifactDetectSetting = EMArtifactDetectSetting(artBord: 110,
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
    private let mentalAndSpectralSetting = MentalAndSpectralSetting(n_sec_for_instant_estimation: 2,
                                                                    n_sec_for_averaging: 1)
    
    private let queue = DispatchQueue(label: "thread-safe-samples", attributes: .concurrent)
    
    var isCalibrated = false
    
    var calibrationProgressCallback: ProgressCallback?
    var showIsArtifactedCallback: IsArtifactedCallback?
    var showLastMindDataCallback: LastMindDataCallback?
    var showLastSpectralDataCallback: LastSpectralDataCallback?
    
    public func initEmotionMath() {
        if(isEmotionalMathInited){
            emotionalMath = nil;
            isEmotionalMathInited = false
            isCalibrated = false
        }
        if ( !isEmotionalMathInited ) {
            emotionalMath = EMEmotionalMath(libSettings: mathLibSetting, andArtifactDetetectSettings: artifactDetectSetting, andShortArtifactDetectSettigns: shortArtifactDetectSetting, andMentalAndSpectralSettings: mentalAndSpectralSetting)
            isEmotionalMathInited = true
        }
    }
    
    public func start() {
        if (!isCalibrated) {
            emotionalMath?.startCalibration()
        }
        startSignal()
    }
    
    public func stop() {
        BrainbitController.shared.stopSignal()
    }
    
    private func startSignal() {
        BrainbitController.shared.startSignal { [self] data in
            queue.async(flags: .barrier) { [self] in
                var bipolarArray : [EMRawChannels] = []
                for sample in data{
                    let bipolarElement = EMRawChannels(leftBipolar: sample.t3.doubleValue - sample.o1.doubleValue, andRightBipolar: sample.t4.doubleValue - sample.o2.doubleValue)
                    bipolarArray.append(bipolarElement!)
                }
                emotionalMath?.pushData(bipolarArray)
                emotionalMath?.processDataArr()
                
                getIsAtifacted()
                
                if (!isCalibrated) {
                    processCalibration()
                }
                else {
                    calcData()
                    
                    getSpectralData()
                }
            }
        }
    }
    
    private func processCalibration() {
        if (emotionalMath?.calibrationFinished() ?? false) {
            isCalibrated = true
        }
        let progress = emotionalMath?.getCallibrationPercents()
        calibrationProgressCallback?(progress ?? 0)
    }
    
    private func calcData() {
        let mindData = emotionalMath?.readMentalDataArr()
        if (mindData != nil && !mindData!.isEmpty) {
            showLastMindDataCallback?(mindData?.last ?? EMMindData(relAttention: 0.0, andRelRelax: 0.0, andInstAttention: 0.0, andInstRelax: 0.0))
        }
    }
    
    private func getSpectralData() {
        let spectralData = emotionalMath?.mathLibReadSpectralDataPercentsArr()
        let lastSpectralData = spectralData?.last ?? EMSpectralDataPercents()
        if(lastSpectralData.alpha > 0 &&
           lastSpectralData.beta > 0 &&
           lastSpectralData.theta > 0 &&
           lastSpectralData.delta > 0 &&
           lastSpectralData.gamma > 0){
            showLastSpectralDataCallback?(lastSpectralData)
        }
        
    }
    
    private func getIsAtifacted() {
        showIsArtifactedCallback?(isCalibrated ? (emotionalMath?.isArtifactedSequence())! : (emotionalMath?.isBothSidesArtifacted())!)
    }
}

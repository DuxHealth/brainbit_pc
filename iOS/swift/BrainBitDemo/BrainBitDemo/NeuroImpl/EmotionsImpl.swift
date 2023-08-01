//
//  EmotionsImpl.swift
//  BrainBitDemo
//
//  Created by Aseatari on 01.08.2023.
//

import Foundation
import EmStArtifacts

typealias Callback = () -> Void
typealias ProgressCallback = (_ progress: Int32) -> Void

class EmotionsImpl {
    
    var emotionalMath : EMEmotionalMath?
    var isEmotionalMathInited = false
    
    private let mathLibSetting = MathLibSetting(sampling_rate: 250, process_win_freq: 25, fft_window: 500, n_first_sec_skipped: 6, bipolar_mode: true, squared_spectrum: false, channels_number: 4, channel_for_analysis: 0)
    private let artifactDetectSetting = ArtifactDetectSetting(art_bord: 110, allowed_percent_artpoints: 70, raw_betap_limit: 800000, total_pow_border: 30000000, global_artwin_sec: 4, spect_art_by_totalp: false, hanning_win_spectrum: false, hamming_win_spectrum: true, num_wins_for_quality_avg: 100)
    private let shortArtifactDetectSetting = ShortArtifactDetectSetting(ampl_art_detect_win_size: 200, ampl_art_zerod_area: 200, ampl_art_extremum_border: 25)
    private let mentalAndSpectralSetting = MentalAndSpectralSetting(n_sec_for_instant_estimation: 2, n_sec_for_averaging: 2)
    
    //
    private let lastWinsToAvg = 3
    private let queue = DispatchQueue(label: "thread-safe-samples", attributes: .concurrent)
    
    var lastMindData : EMMindData = EMMindData()
    var lastSpectralData : EMSpectralDataPercents = EMSpectralDataPercents()
    var isCalibrated = false
    var isArtifacted = false
    
    var calibrationProgressCallback: ProgressCallback?
    var showIsArtifactedCallback: Callback?
    var showLastMindDataCallback: Callback?
    var showLastSpectralDataCallback: Callback?
    
    public func initEmotionMath() {
        if ( !isEmotionalMathInited ) {
            emotionalMath = EMEmotionalMath(libSettings: mathLibSetting, andArtifactDetetectSettings: artifactDetectSetting, andShortArtifactDetectSettigns: shortArtifactDetectSetting, andMentalAndSpectralSettings: mentalAndSpectralSetting)
            //emotionalMath?.setPrioritySide(.Left)
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
                emotionalMath?.pushData(bipolarArray) // отправляем массив "биполяров"
                emotionalMath?.processDataArr()
                
                getIsAtifacted()
                showIsArtifactedCallback?()
                
                if (!isCalibrated) {
                    processCalibration()
                }
                else {
                    calcData()
                    showLastMindDataCallback?()
                    
                    getSpectralData()
                    showLastSpectralDataCallback?()
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
        if (lastWinsToAvg > 0) {
            lastMindData = (emotionalMath?.readAverageMentalData(Int32(lastWinsToAvg)))!
        }
        let mindData = emotionalMath?.readMentalDataArr()
        if (mindData != nil && !mindData!.isEmpty) {
            lastMindData = getMindAvg(inputMindData: mindData!)
        }
    }
    
    private func getMindAvg(inputMindData : [EMMindData]) -> EMMindData {
        if (inputMindData.isEmpty) {
            return EMMindData(relAttention: 0.0, andRelRelax: 0.0, andInstAttention: 0.0, andInstRelax: 0.0)
        }
        if (inputMindData.count == 1) {
            return inputMindData[0]
        }
        var resultMindData = EMMindData(relAttention: 0.0, andRelRelax: 0.0, andInstAttention: 0.0, andInstRelax: 0.0)
        for mindData in inputMindData {
            resultMindData?.relAttention += mindData.relAttention
            resultMindData?.relRelaxation += mindData.relRelaxation
            resultMindData?.instAttention += mindData.instAttention
            resultMindData?.instRelaxation += mindData.instRelaxation
        }
        resultMindData?.relAttention /= Double(inputMindData.count)
        resultMindData?.relRelaxation /= Double(inputMindData.count)
        resultMindData?.instAttention /= Double(inputMindData.count)
        resultMindData?.instRelaxation /= Double(inputMindData.count)
                
        return resultMindData!
    }
    
    private func getSpectralData() {
        let spectralData = emotionalMath?.mathLibReadSpectralDataPercentsArr()
        lastSpectralData = spectralData?.last ?? EMSpectralDataPercents()
    }
    
    private func getIsAtifacted() {
        if (!isCalibrated) {
            isArtifacted = ((emotionalMath?.isBothSidesArtifacted()) != nil)
        }
        isArtifacted = ((emotionalMath?.isArtifactedSequence()) != nil)
    }
}

//
//  SpectrumImpl.swift
//  BrainBitDemo
//
//  Created by Aseatari on 06.12.2023.
//

import Foundation
import spectrumlib

class SpectrumImpl{
    
    typealias ProcessedSpectrumCallback = (_ spectrum: [SMRawSpectrumData], _ channel: String) -> Void
    typealias ProcessedWavesCallback = (_ waves: SMWavesSpectrumData, _ channel: String) -> Void
    
    var spectrums: [String: SMSpectrumMath] = [:]
    var processedSpectrumCallback: ProcessedSpectrumCallback?
    var processedWavesCallback: ProcessedWavesCallback?
    
    public func initSpectrumMath(){
        spectrums.removeAll()
        
        spectrums["O1"] = SMSpectrumMath(sampleRate: Parameters.samplingRate, andFftWindow: Parameters.fftWindow, andProcessWinFreq: Parameters.processWinRate)
        spectrums["O2"] = SMSpectrumMath(sampleRate: Parameters.samplingRate, andFftWindow: Parameters.fftWindow, andProcessWinFreq: Parameters.processWinRate)
        spectrums["T3"] = SMSpectrumMath(sampleRate: Parameters.samplingRate, andFftWindow: Parameters.fftWindow, andProcessWinFreq: Parameters.processWinRate)
        spectrums["T4"] = SMSpectrumMath(sampleRate: Parameters.samplingRate, andFftWindow: Parameters.fftWindow, andProcessWinFreq: Parameters.processWinRate)
        
        for (_, value) in spectrums {
            value.initParams(withUpBorderFreq: Parameters.bordFrequency, andNormilize: Parameters.normalze)
            value.setSquaredSpect(Parameters.squaredSpect)
            value.setWavesCoeffsWithDelta(Parameters.deltaCoeff, andTheta: Parameters.thetaCoeff, andAlpha: Parameters.alphaCoeff, andBeta: Parameters.betaCoeff, andGamma: Parameters.gammaCoeff)
        }
    }
    
    public func processSamples(channel: String, samples: [NSNumber]) {
        spectrums[channel]?.pushAndProcessData(samples)
        guard let rawInfo = spectrums[channel]?.readRawSpectrumInfoArr() else {
            spectrums[channel]?.setNewSampleSize()
            return
        }
        processedSpectrumCallback?(rawInfo, channel)
        
        guard let wavesInfo = spectrums[channel]?.readWavesSpectrumInfoArr().last else {
            spectrums[channel]?.setNewSampleSize()
            return
        }
        processedWavesCallback?(wavesInfo, channel)
        spectrums[channel]?.setNewSampleSize()
            
    }
    
}

extension SpectrumImpl {
    enum Parameters{
        static let samplingRate: Int32 = 250
        static let bordFrequency = samplingRate / 2
        static let fftWindow = samplingRate * 1
        static let processWinRate: Int32 = 10
        static let squaredSpect = false
        static let normalze = false
        static let alphaCoeff = 1.0
        static let betaCoeff = 1.0
        static let deltaCoeff = 1.0
        static let thetaCoeff = 1.0
        static let gammaCoeff = 1.0
    }
}

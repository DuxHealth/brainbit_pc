//
//  SpectrumViewController.swift
//  BrainBitDemo
//
//  Created by Aseatari on 01.12.2023.
//

import Foundation
import UIKit
import spectrumlib

class SpectrumViewController: UIViewController {
    
    private let queue = DispatchQueue(label: "thread-safe-samples", attributes: .concurrent)
    
    var spectrumO1 = SMSpectrumMath(sampleRate: 250, andFftWindow: 250, andProcessWinFreq: 10)
    
    private var isStarted = false

    override func viewDidLoad() {
        super.viewDidLoad()
        
        spectrumO1?.initParams(withUpBorderFreq: 250 / 2, andNormilize: false)
        spectrumO1?.setWavesCoeffsWithDelta(0, andTheta: 0, andAlpha: 1, andBeta: 1, andGamma: 0)
        spectrumO1?.setSquaredSpect(false)
        
        
    }
    
    @IBAction func onStartTapped(_ sender: UIButton) {
        if(isStarted){
            sender.setTitle("Start", for: .normal)
            BrainbitController.shared.stopSignal()
        }else{
            sender.setTitle("Stop", for: .normal)
            
            BrainbitController.shared.startSignal { [self] data in
                queue.async(flags: .barrier) { [self] in
                    var samples: [NSNumber] = []
                    for sample in data{
                        samples.append(sample.o1)
                    }
                    spectrumO1?.pushAndProcessData(samples)
                    var rawInfo = spectrumO1?.readRawSpectrumInfoArr()
                    guard let lastRaw = rawInfo?.last else {
                        return
                    }
                    print("length: \(rawInfo?.count) totalRawPow: \(lastRaw.totalRawPow)")
                    var wavesInfo = spectrumO1?.readWavesSpectrumInfoArr()
                    guard let lastWaves = wavesInfo?.last else {
                        return
                    }
                    print("alpha raw: \(lastWaves.alpha_raw) rel: \(lastWaves.alpha_rel)")
                    print("beta raw: \(lastWaves.beta_raw) rel: \(lastWaves.beta_rel)")
                    spectrumO1?.setNewSampleSize()
                }
            }
        }
        isStarted = !isStarted
    }
    
    /*
    // MARK: - Navigation

    // In a storyboard-based application, you will often want to do a little preparation before navigation
    override func prepare(for segue: UIStoryboardSegue, sender: Any?) {
        // Get the new view controller using segue.destination.
        // Pass the selected object to the new view controller.
    }
    */

}

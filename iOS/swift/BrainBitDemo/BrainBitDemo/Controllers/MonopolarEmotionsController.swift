//
//  MonopolarEmotionsController.swift
//  BrainBitDemo
//
//  Created by Aseatari on 04.12.2023.
//

import Foundation
import UIKit
import EmStArtifacts

class MonopolarEmotionsViewController : UIViewController {
    
    @IBOutlet weak var O1calibrationLabel: UILabel!
    @IBOutlet weak var O1alphaLabel: UILabel!
    @IBOutlet weak var O1betaLabel: UILabel!
    @IBOutlet weak var O1isArtifactedLabel: UILabel!
    
    @IBOutlet weak var T3calibrationLabel: UILabel!
    @IBOutlet weak var T3artifactedLabel: UILabel!
    @IBOutlet weak var T3AlphaLabel: UILabel!
    @IBOutlet weak var T3BetaLabel: UILabel!
    
    private var emotionsImpl : EmotionsMonopolar = EmotionsMonopolar()
    private var isStarted = false
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        emotionsImpl.calibrationProgressCallback = showCalibrationProgress
        emotionsImpl.showIsArtifactedCallback = showIsArtifacted
        emotionsImpl.showRawSpectralDataCallback = showLastSpectralData
    }

    @IBAction func onStartTapped(_ sender: UIButton) {
        
        emotionsImpl.initEmotionMath()
        
        if (isStarted) {
            BrainbitController.shared.stopSignal()
            sender.setTitle("Start", for: .normal)
        }
        else {
            BrainbitController.shared.startSignal { [self] data in
                emotionsImpl.processData(bbData: data)
            }
            
            sender.setTitle("Stop", for: .normal)
            O1calibrationLabel.text = "Calibration: 0%"
            T3calibrationLabel.text = "Calibration: 0%"
        }
        isStarted = !isStarted;
    }
    
    private func showCalibrationProgress(progress: UInt32, channel: String) {
        DispatchQueue.main.async { [self] in
            switch (channel){
            case "O1":
                O1calibrationLabel.text = String(format: "Calibration: %d%", progress)
                break;
            case "T3":
                T3calibrationLabel.text = String(format: "Calibration: %d%", progress)
                break;
            default:
                print("\(channel) calibration is \(progress)%")
            }
            
        }
    }
    
    private func showLastSpectralData(spectralData: EMRawSpectVals, channel: String) {
        DispatchQueue.main.async { [self] in
            switch (channel){
            case "O1":
                O1alphaLabel.text = "Alpha: \(spectralData.alpha)"
                O1betaLabel.text = "Beta: \(spectralData.beta)"
                break;
            case "T3":
                T3AlphaLabel.text = "Alpha: \(spectralData.alpha)"
                T3BetaLabel.text = "Beta: \(spectralData.beta)"
                break;
            default:
                print("\(channel) alpha = \(spectralData.alpha), beta = \(spectralData.beta)")
            }
        }
    }
    
    private func showIsArtifacted(artifacted: Bool, channel: String) {
        DispatchQueue.main.async { [self] in
            switch (channel){
            case "O1":
                O1isArtifactedLabel.text = artifacted ? "Is Artifacted" : "Not Artifacted"
                break;
            case "T3":
                T3artifactedLabel.text = artifacted ? "Is Artifacted" : "Not Artifacted"
                break;
            default:
                print("\(channel) \(artifacted ? "Is Artifacted" : "Not Artifacted")")
            }
        }
    }
}

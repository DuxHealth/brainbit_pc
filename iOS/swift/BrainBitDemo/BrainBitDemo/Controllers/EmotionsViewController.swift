//
//  EmotionsViewController.swift
//  BrainBitDemo
//
//  Created by Aseatari on 01.08.2023.
//

import UIKit
import EmStArtifacts

class EmotionsViewController : UIViewController {
    
    @IBOutlet weak var calibrationLabel: UILabel!
    @IBOutlet weak var relaxationLabel: UILabel!
    @IBOutlet weak var attentionLabel: UILabel!
    @IBOutlet weak var relRelaxationLabel: UILabel!
    @IBOutlet weak var relAttentionLabel: UILabel!
    @IBOutlet weak var alphaLabel: UILabel!
    @IBOutlet weak var betaLabel: UILabel!
    @IBOutlet weak var gammaLabel: UILabel!
    @IBOutlet weak var deltaLabel: UILabel!
    @IBOutlet weak var thetaLabel: UILabel!
    @IBOutlet weak var isArtifactedLabel: UILabel!
    
    private var emotionsImpl : EmotionsImpl = EmotionsImpl()
    private var isStarted = false
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        emotionsImpl.calibrationProgressCallback = showCalibrationProgress
        emotionsImpl.showIsArtifactedCallback = showIsArtifacted
        emotionsImpl.showLastMindDataCallback = showLastMindData
        emotionsImpl.showLastSpectralDataCallback = showLastSpectralData
    }

    @IBAction func onStartTapped(_ sender: UIButton) {
        
        emotionsImpl.initEmotionMath()
        
        if (isStarted) {
            emotionsImpl.stop()
            sender.setTitle("Start", for: .normal)
        }
        else {
            emotionsImpl.start()
            
            sender.setTitle("Stop", for: .normal)
            calibrationLabel.text = ""
        }
        isStarted = !isStarted;
    }
    
    private func showCalibrationProgress(_ progress: UInt32) {
        DispatchQueue.main.async { [self] in
            calibrationLabel.text = String(format: "%d", progress)
        }
    }
    
    private func showLastMindData(mindData: EMMindData) {
        DispatchQueue.main.async { [self] in
            relaxationLabel.text = "Relaxation: \(mindData.instRelaxation) %"
            attentionLabel.text = "Attention: \(mindData.instAttention) %"
            relRelaxationLabel.text = "Rel Relaxation: \(mindData.relRelaxation)"
            relAttentionLabel.text = "Rel Attention: \(mindData.relAttention)"
        }
    }
    
    private func showLastSpectralData(spectralData: EMSpectralDataPercents) {
        DispatchQueue.main.async { [self] in
            print("Alpha: \(spectralData.alpha) Beta: \(spectralData.beta) Gamma: \(spectralData.gamma) Delta: \(spectralData.delta) Theta: \(spectralData.theta)")
            alphaLabel.text = "Alpha: \(spectralData.alpha)"
            betaLabel.text = "Beta: \(spectralData.beta)"
            gammaLabel.text = "Gamma: \(spectralData.gamma)"
            deltaLabel.text = "Delta: \(spectralData.delta)"
            thetaLabel.text = "Theta: \(spectralData.theta)"
        }
    }
    
    private func showIsArtifacted(artifacted: Bool) {
        DispatchQueue.main.async { [self] in
            isArtifactedLabel.text = artifacted ? "Is Artifacted" : "Not Artifacted"
        }
    }
}


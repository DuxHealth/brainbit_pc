//
//  EmotionsViewController.swift
//  BrainBitDemo
//
//  Created by Aseatari on 01.08.2023.
//

import UIKit

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
    
    private func showCalibrationProgress(_ progress: Int32) {
        DispatchQueue.main.async { [self] in
            calibrationLabel.text = String(format: "%d", progress)
        }
    }
    
    private func showLastMindData() {
        DispatchQueue.main.async { [self] in
            relaxationLabel.text = "Relaxation: \(emotionsImpl.lastMindData.instRelaxation) %"
            attentionLabel.text = "Attention: \(emotionsImpl.lastMindData.instAttention) %"
            relRelaxationLabel.text = "Rel Relaxation: \(emotionsImpl.lastMindData.relRelaxation)"
            relAttentionLabel.text = "Rel Attention: \(emotionsImpl.lastMindData.relAttention)"
        }
    }
    
    private func showLastSpectralData() {
        DispatchQueue.main.async { [self] in
            alphaLabel.text = "Alpha: \(emotionsImpl.lastSpectralData.alpha)"
            betaLabel.text = "Beta: \(emotionsImpl.lastSpectralData.beta)"
            gammaLabel.text = "Gamma: \(emotionsImpl.lastSpectralData.gamma)"
            deltaLabel.text = "Delta: \(emotionsImpl.lastSpectralData.delta)"
            thetaLabel.text = "Theta: \(emotionsImpl.lastSpectralData.theta)"
        }
    }
    
    private func showIsArtifacted() {
        DispatchQueue.main.async { [self] in
            if (emotionsImpl.isArtifacted) {
                isArtifactedLabel.text = "Is Artifacted"
            }
            else {
                isArtifactedLabel.text = "Not Artifacted"
            }
        }
    }
}


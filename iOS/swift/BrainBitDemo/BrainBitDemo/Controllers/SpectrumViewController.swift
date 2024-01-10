//
//  SpectrumViewController.swift
//  BrainBitDemo
//
//  Created by Aseatari on 01.12.2023.
//

import Foundation
import UIKit
import spectrumlib
import DropDown

class SpectrumViewController: UIViewController {
    
    private let queue = DispatchQueue(label: "thread-safe-samples", attributes: .concurrent)
    
    var spectrumMath = SpectrumImpl()
    
    @IBOutlet weak var AlphaPLabel: UILabel!
    @IBOutlet weak var BetaPLabel: UILabel!
    @IBOutlet weak var ThetaPLabel: UILabel!
    @IBOutlet weak var DeltaPLabel: UILabel!
    @IBOutlet weak var GammaPLabel: UILabel!
    
    @IBOutlet weak var AlphaRawLabel: UILabel!
    @IBOutlet weak var BetaRawLabel: UILabel!
    @IBOutlet weak var ThetaRawLabel: UILabel!
    @IBOutlet weak var DeltaRawLabel: UILabel!
    @IBOutlet weak var GammaRawLabel: UILabel!
    
    @IBOutlet weak var SpectrumGraph: SpectrumGraphView!
    
    private var isStarted = false
    
    var channelsArr = ["O1", "O2", "T3", "T4"]
    var currentChannel = "O1"
    let ChannelsDropDown = DropDown()

    override func viewDidLoad() {
        super.viewDidLoad()
        
        spectrumMath.initSpectrumMath()
        spectrumMath.processedWavesCallback = processedWavesData
        spectrumMath.processedSpectrumCallback = processedSpectrum
        
        SpectrumGraph.initGraph(samplingFrequency: 250)
        
        ChannelsDropDown.dataSource = channelsArr
        ChannelsDropDown.selectRow(at: 0)
        currentChannel = "O1"
    }
    
    @IBAction func OnChannelChoose(_ sender: UIButton) {
        ChannelsDropDown.anchorView = sender
        ChannelsDropDown.bottomOffset = CGPoint(x: 0, y: sender.frame.size.height)
        ChannelsDropDown.show()
        ChannelsDropDown.selectionAction = { [weak self] (index: Int, item: String) in
              guard let self = self else { return }
              sender.setTitle(item, for: .normal)
            currentChannel = item
        }
    }
    
    @IBAction func onStartTapped(_ sender: UIButton) {
        if(isStarted){
            sender.setTitle("Start", for: .normal)
            BrainbitController.shared.stopSignal()
        }else{
            sender.setTitle("Stop", for: .normal)
            
            BrainbitController.shared.startSignal { [self] data in
                queue.async(flags: .barrier) { [self] in
                    var o1Values: [NSNumber] = []
                    var o2Values: [NSNumber] = []
                    var t3Values: [NSNumber] = []
                    var t4Values: [NSNumber] = []
                    o1Values.removeAll()
                    o2Values.removeAll()
                    t3Values.removeAll()
                    t4Values.removeAll()
                    for sample in data{
                        o1Values.append(sample.o1)
                        o2Values.append(sample.o2)
                        t3Values.append(sample.t3)
                        t4Values.append(sample.t4)
                    }
                    spectrumMath.processSamples(channel: "O1", samples: o1Values)
                    spectrumMath.processSamples(channel: "O2", samples: o2Values)
                    spectrumMath.processSamples(channel: "T3", samples: t3Values)
                    spectrumMath.processSamples(channel: "T4", samples: t4Values)
                }
            }
        }
        isStarted = !isStarted
    }
    
    private func processedWavesData(_ waves: SMWavesSpectrumData, _ channel: String) {
        DispatchQueue.main.async { [self] in
            AlphaPLabel.text = String(format: "%.1f%%", waves.alpha_rel * 100)
            BetaPLabel.text  = String(format: "%.1f%%", waves.beta_rel * 100)
            ThetaPLabel.text = String(format: "%.1f%%", waves.theta_rel * 100)
            DeltaPLabel.text = String(format: "%.1f%%", waves.delta_rel * 100)
            GammaPLabel.text = String(format: "%.1f%%", waves.gamma_rel * 100)
            
            AlphaRawLabel.text = String(format: "%.4f", waves.alpha_raw)
            BetaRawLabel.text  = String(format: "%.4f", waves.beta_raw)
            ThetaRawLabel.text = String(format: "%.4f", waves.theta_raw)
            DeltaRawLabel.text = String(format: "%.4f", waves.delta_raw)
            GammaRawLabel.text = String(format: "%.4f", waves.gamma_raw)
        }
    }
    
    private func processedSpectrum(_ spectrum: [SMRawSpectrumData], _ channel: String) {
        DispatchQueue.main.async { [self] in
            var data = [NSNumber]()
            if(channel == currentChannel){
                for d in spectrum{
                    data.append(contentsOf: d.allBinsValues)
                }
                if(!data.isEmpty){
                    SpectrumGraph.dataChanged(newValues: data)
                }
                
            }
        }
    }

}

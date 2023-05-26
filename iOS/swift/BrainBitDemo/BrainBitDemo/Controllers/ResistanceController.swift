//
//  ResistanceController.swift
//  BrainBitDemo
//
//  Created by Aseatari on 21.02.2023.
//

import UIKit

class ResistanceViewController: UIViewController {
    
    
    @IBOutlet weak var O1Value: UILabel!
    @IBOutlet weak var O2Value: UILabel!
    @IBOutlet weak var T3Value: UILabel!
    @IBOutlet weak var T4Value: UILabel!
    
    override func viewWillDisappear(_ animated: Bool) {
        super.viewWillDisappear(animated)
        BrainbitController.shared	.stopResist()
    }
    
    @IBAction func onStartTapped(_ sender: UIButton) {
        BrainbitController.shared.startResist { data in
            DispatchQueue.main.async { [self] in
                O1Value.text = String(format: "%.2f Ohm", data.o1.floatValue)
                O2Value.text = String(format: "%.2f Ohm", data.o2.floatValue)
                T3Value.text = String(format: "%.2f Ohm", data.t3.floatValue)
                T4Value.text = String(format: "%.2f Ohm", data.t4.floatValue)
            }
        }
    }
}

import UIKit

class SignalViewController: UIViewController {

    @IBOutlet weak var O1SignalView: SignalGraphView!
    @IBOutlet weak var O2SignalView: SignalGraphView!
    @IBOutlet weak var T3SignalView: SignalGraphView!
    @IBOutlet weak var T4SignalView: SignalGraphView!
    
    var isSignal = false;
    
    var timer: Timer?
    let queue = DispatchQueue(label: "thread-safe-samples", attributes: .concurrent)
    var o1Samples = [NSNumber]()
    var o2Samples = [NSNumber]()
    var t3Samples = [NSNumber]()
    var t4Samples = [NSNumber]()
    
    override func viewDidLoad() {
        super.viewDidLoad()

        O1SignalView.initGraph(samplingFrequency: 250, channelName: "O1")
        O2SignalView.initGraph(samplingFrequency: 250, channelName: "O2")
        T3SignalView.initGraph(samplingFrequency: 250, channelName: "T3")
        T4SignalView.initGraph(samplingFrequency: 250, channelName: "T4")
        
        timer = Timer.scheduledTimer(timeInterval: 1.0/30.0, target: self, selector: #selector(updateGraph), userInfo: nil, repeats: true)
    }
    
    override func viewWillDisappear(_ animated: Bool) {
        super.viewWillDisappear(animated)
        BrainbitController.shared.stopSignal()
        timer?.invalidate()
        timer = nil
    }
    
    @IBAction func onStartButtonTapped(_ sender: UIButton) {
        if(!isSignal){
            BrainbitController.shared.startSignal { [self] data in
                queue.async(flags: .barrier) { [self] in
                    for sample in data{
                        o1Samples.append(sample.o1.floatValue * 1e6 as NSNumber)
                        o2Samples.append(sample.o2.floatValue * 1e6 as NSNumber)
                        t3Samples.append(sample.t3.floatValue * 1e6 as NSNumber)
                        t4Samples.append(sample.t4.floatValue * 1e6 as NSNumber)
                    }
                }
            }
            sender.setTitle("Stop", for: .normal)
        }
        else
        {
            BrainbitController.shared.stopSignal()
            sender.setTitle("Start", for: .normal)
        }
        isSignal = !isSignal;
    }
    
    @objc func updateGraph() {
        
        queue.sync {
            O1SignalView.dataChanged(newValues: o1Samples)
            O2SignalView.dataChanged(newValues: o2Samples)
            T3SignalView.dataChanged(newValues: t3Samples)
            T4SignalView.dataChanged(newValues: t4Samples)
            
            o1Samples.removeAll()
            o2Samples.removeAll()
            t3Samples.removeAll()
            t4Samples.removeAll()
        }
        
    }
    
}


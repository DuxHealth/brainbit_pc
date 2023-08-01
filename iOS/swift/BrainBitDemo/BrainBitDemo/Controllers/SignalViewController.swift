import UIKit
import filters
import DropDown

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
    
    var defaultFilters: DefaultFilters? = DefaultFilters()
    var lpArr = [String]()
    var prevLP: FTFilterParam = FTFilterParam()
    var hpArr = [String]()
    var prevHP: FTFilterParam = FTFilterParam()
    var bsArr = [String]()
    var prevBS: FTFilterParam = FTFilterParam()
    
    let LPdropDown = DropDown()
    let HPdropDown = DropDown()
    let BSdropDown = DropDown()
    
    var filtersO1Impl: FiltersImpl? = FiltersImpl()
    var filtersO2Impl: FiltersImpl? = FiltersImpl()
    var filtersT3Impl: FiltersImpl? = FiltersImpl()
    var filtersT4Impl: FiltersImpl? = FiltersImpl()
    
    override func viewDidLoad() {
        super.viewDidLoad()

        O1SignalView.initGraph(samplingFrequency: 250, channelName: "O1")
        O2SignalView.initGraph(samplingFrequency: 250, channelName: "O2")
        T3SignalView.initGraph(samplingFrequency: 250, channelName: "T3")
        T4SignalView.initGraph(samplingFrequency: 250, channelName: "T4")
        
        lpArr = defaultFilters?.filtersLP.map { $0.key } ?? []
        prevLP = defaultFilters?.filtersLP[lpArr.first ?? ""] ?? FTFilterParam()
        LPdropDown.dataSource = lpArr
        LPdropDown.selectRow(at: 0)
        
        hpArr = defaultFilters?.filtersHP.map { $0.key } ?? []
        prevHP = defaultFilters?.filtersHP[hpArr.first ?? ""] ?? FTFilterParam()
        HPdropDown.dataSource = hpArr
        LPdropDown.selectRow(at: 0)
        
        bsArr = defaultFilters?.filtersBS.map { $0.key } ?? []
        prevBS = defaultFilters?.filtersBS[bsArr.first ?? ""] ?? FTFilterParam()
        BSdropDown.dataSource = defaultFilters?.filtersBS.map { $0.key } ?? []
        BSdropDown.selectRow(at: 0)
        
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
                    o1Samples = filtersO1Impl?.filter(samples: o1Samples) ?? []
                    o2Samples = filtersO2Impl?.filter(samples: o2Samples) ?? []
                    t3Samples = filtersT3Impl?.filter(samples: t3Samples) ?? []
                    t4Samples = filtersT4Impl?.filter(samples: t4Samples) ?? []
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
    
    @IBAction func LPFiltersTap(_ sender: UIButton) {
        LPdropDown.anchorView = sender
        LPdropDown.bottomOffset = CGPoint(x: 0, y: sender.frame.size.height)
        LPdropDown.show()
        LPdropDown.selectionAction = { [weak self] (index: Int, item: String) in
              guard let self = self else { return }
              sender.setTitle(item, for: .normal)
            self.filtersO1Impl?.removeFilter(fParam: self.prevLP)
            self.filtersO2Impl?.removeFilter(fParam: self.prevLP)
            self.filtersT3Impl?.removeFilter(fParam: self.prevLP)
            self.filtersT4Impl?.removeFilter(fParam: self.prevLP)

            self.prevLP = self.defaultFilters?.filtersLP[self.lpArr[index]] ?? FTFilterParam()
            
            self.filtersO1Impl?.addFilter(fParam: self.prevLP)
            self.filtersO2Impl?.addFilter(fParam: self.prevLP)
            self.filtersT3Impl?.addFilter(fParam: self.prevLP)
            self.filtersT4Impl?.addFilter(fParam: self.prevLP)
        }
    }
    
    @IBAction func HPFiltersTap(_ sender: UIButton) {
        HPdropDown.anchorView = sender
        HPdropDown.bottomOffset = CGPoint(x: 0, y: sender.frame.size.height)
        HPdropDown.show()
        HPdropDown.selectionAction = { [weak self] (index: Int, item: String) in
            guard let self = self else { return }
            sender.setTitle(item, for: .normal)
            self.filtersO1Impl?.removeFilter(fParam: self.prevHP)
            self.filtersO2Impl?.removeFilter(fParam: self.prevHP)
            self.filtersT3Impl?.removeFilter(fParam: self.prevHP)
            self.filtersT4Impl?.removeFilter(fParam: self.prevHP)
            
            self.prevHP = self.defaultFilters?.filtersHP[self.hpArr[index]] ?? FTFilterParam()
            
            self.filtersO1Impl?.addFilter(fParam: self.prevHP)
            self.filtersO2Impl?.addFilter(fParam: self.prevHP)
            self.filtersT3Impl?.addFilter(fParam: self.prevHP)
            self.filtersT4Impl?.addFilter(fParam: self.prevHP)
        }
    }
    
    @IBAction func BSFiltersTap(_ sender: UIButton) {
        BSdropDown.anchorView = sender
        BSdropDown.bottomOffset = CGPoint(x: 0, y: sender.frame.size.height)
        BSdropDown.show()
        BSdropDown.selectionAction = { [weak self] (index: Int, item: String) in
              guard let self = self else { return }
              sender.setTitle(item, for: .normal)
            self.filtersO1Impl?.removeFilter(fParam: self.prevBS)
            self.filtersO2Impl?.removeFilter(fParam: self.prevBS)
            self.filtersT3Impl?.removeFilter(fParam: self.prevBS)
            self.filtersT4Impl?.removeFilter(fParam: self.prevBS)
            
            self.prevBS = self.defaultFilters?.filtersBS[self.hpArr[index]] ?? FTFilterParam()
            
            self.filtersO1Impl?.addFilter(fParam: self.prevBS)
            self.filtersO2Impl?.addFilter(fParam: self.prevBS)
            self.filtersT3Impl?.addFilter(fParam: self.prevBS)
            self.filtersT4Impl?.addFilter(fParam: self.prevBS)
        }
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


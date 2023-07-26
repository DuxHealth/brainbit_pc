package com.neurotech.brainbitneurosdkdemo.ui.emotions

import androidx.databinding.ObservableBoolean
import androidx.databinding.ObservableField
import androidx.lifecycle.ViewModel
import com.neurotech.brainbitneurosdkdemo.emotions.EmotionalController
import com.neurotech.brainbitneurosdkdemo.emotions.EmotionalMathConfig
import com.neurotech.brainbitneurosdkdemo.neuroimpl.BrainBitController
import com.neurotech.emstartifcats.ArtifactDetectSetting
import com.neurotech.emstartifcats.MathLibSetting
import com.neurotech.emstartifcats.MentalAndSpectralSetting
import com.neurotech.emstartifcats.ShortArtifactDetectSetting

class EmotionsViewModel : ViewModel() {
    //<editor-fold desc="Properties">
    private var emotionalController: EmotionalController

    var started = ObservableBoolean(false)
    private var calibrated = ObservableBoolean(false)
    var artifacted = ObservableBoolean(false)

    var calibrationProgress = ObservableField(0)

    var relaxationText = ObservableField("Relaxation: Waiting for calibration...")
    var attentionText = ObservableField("Attention: Waiting for calibration...")
    var relRelaxationText = ObservableField("Rel Relaxation: Waiting for calibration...")
    var relAttentionText = ObservableField("Rel Attention: Waiting for calibration...")

    var alphaText = ObservableField("Alpha: Waiting for calibration...")
    var betaText = ObservableField("Beta: Waiting for calibration...")
    var gammaText = ObservableField("Gamma: Waiting for calibration...")
    var thetaText = ObservableField("Theta: Waiting for calibration...")
    var deltaText = ObservableField("Delta: Waiting for calibration...")
    //</editor-fold>

    //<editor-fold desc="Init and Close">
    init {
        emotionalController = EmotionalController(getEmotionalConfig())
    }

    fun close() {
        BrainBitController.stopSignal()

        emotionalController.isArtifacted = {}
        emotionalController.lastMindData = {}
        emotionalController.lastSpectralData = {}
        emotionalController.isCalibrationSuccess = {}
        emotionalController.calibrationProgress = {}
    }
    //</editor-fold>

    //<editor-fold desc="Buttons">
    fun onSignalClicked() {
        if (started.get()) {
            BrainBitController.stopSignal()
        } else if (!calibrated.get()) {
            startSignal()
            emotionalController.startCalibration()
        } else {
            startSignal()
        }

        started.set(!started.get())
    }
    //</editor-fold>

    //<editor-fold desc="Signal">
    private fun startSignal() {
        emotionalController.isCalibrationSuccess = {
            calibrated.set(it)
        }

        emotionalController.isArtifacted = {
            artifacted.set(it)
        }

        emotionalController.lastMindData = {
            relaxationText.set("Relaxation: " + String.format("%.2f", it.instRelaxation) + "%")
            attentionText.set("Attention: " + String.format("%.2f", it.instAttention) + "%")
            relRelaxationText.set("Rel Relaxation: " + String.format("%.2f", it.relRelaxation))
            relAttentionText.set("Rel Attention: " + String.format("%.2f", it.relAttention))
        }

        emotionalController.calibrationProgress = {
            calibrationProgress.set(it)
        }

        emotionalController.lastSpectralData = {
            alphaText.set("Alpha: " + String.format("%.2f", it.alpha) + "%")
            betaText.set("Beta: " + String.format("%.2f", it.beta) + "%")
            gammaText.set("Gamma: " + String.format("%.2f", it.gamma) + "%")
            thetaText.set("Theta: " + String.format("%.2f", it.theta) + "%")
            deltaText.set("Delta: " + String.format("%.2f", it.delta) + "%")

//            alphaText.set("Alpha: ${it.alpha}%")
//            betaText.set("Beta: ${it.beta}%")
//            gammaText.set("Gamma: ${it.gamma}%")
//            thetaText.set("Theta: ${it.theta}%")
//            deltaText.set("Delta: ${it.delta}%")

        }

        BrainBitController.startSignal {
            emotionalController.pushData(it)
        }
    }
    //</editor-fold>

    //<editor-fold desc="Emotional Config">
    private fun getEmotionalConfig(): EmotionalMathConfig {
        val lastWinsToAvg = 3
        val poorSignalAvgSize = 5
        val poorSignalAvgTrigger = .8F

        val mathLibSetting = MathLibSetting(
            /* samplingRate = */        250,
            /* processWinFreq = */      25,
            /* fftWindow = */           500,
            /* nFirstSecSkipped = */    6,
            /* bipolarMode = */         true,
            /* channelsNumber = */      4,
            /* channelForAnalysis = */  0
        )

        val artifactDetectSetting = ArtifactDetectSetting(
            /* artBord = */                 110,
            /* allowedPercentArtpoints = */ 70,
            /* rawBetapLimit = */           800_000,
            /* totalPowBorder = */          30000000,
            /* globalArtwinSec = */         4,
            /* spectArtByTotalp = */        false,
            /* hanningWinSpectrum = */      false,
            /* hammingWinSpectrum = */      true,
            /* numWinsForQualityAvg = */    100
        )

        val shortArtifactDetectSetting = ShortArtifactDetectSetting(
            /* amplArtDetectWinSize = */    200,
            /* amplArtZerodArea = */        200,
            /* amplArtExtremumBorder = */   25
        )

        val mentalAndSpectralSetting = MentalAndSpectralSetting(
            /* nSecForInstantEstimation = */    2,
            /* nSecForAveraging = */            2
        )

        return EmotionalMathConfig(
            250,
            lastWinsToAvg,
            poorSignalAvgSize,
            poorSignalAvgTrigger,
            mathLibSetting,
            artifactDetectSetting,
            shortArtifactDetectSetting,
            mentalAndSpectralSetting
        )
    }
    //</editor-fold>
}
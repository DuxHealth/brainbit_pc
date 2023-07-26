package com.neurotech.brainbitneurosdkdemo.spectrum

import android.util.Log
import com.neurotech.spectrum.RawSpectrumData
import com.neurotech.spectrum.SpectrumMath
import com.neurotech.spectrum.WavesSpectrumData
import kotlinx.coroutines.DelicateCoroutinesApi
import kotlinx.coroutines.newSingleThreadContext
import kotlinx.coroutines.runBlocking

class SpectrumController {
    private val spectrumMathO1: SpectrumMath
    private val spectrumMathO2: SpectrumMath
    private val spectrumMathT3: SpectrumMath
    private val spectrumMathT4: SpectrumMath

    private val processWinRate = 20 // Hz
    private val fftWindow = 1000
    private var bordFrequency = 50
    private val samplingRate = 250 // Hz
    private var deltaCoef = 0.0
    private var thetaCoef = 1.0
    private var alphaCoef = 1.0
    private var betaCoef = 1.0
    private var gammaCoef = 0.0

    val fftBinsFor1Hz get() = spectrumMathO1.fftBinsFor1Hz

    var processedSpectrum: (Map<SignalChannels, Array<RawSpectrumData>>) -> Unit = { }
    var processedWaves: (Map<SignalChannels, Array<WavesSpectrumData>>) -> Unit = { }

    init {
        spectrumMathO1 = SpectrumMath(samplingRate, fftWindow, processWinRate)
        spectrumMathO1.initParams(bordFrequency, true)
        spectrumMathO1.setWavesCoeffs(deltaCoef, thetaCoef, alphaCoef, betaCoef, gammaCoef)

        spectrumMathO2 = SpectrumMath(samplingRate, fftWindow, processWinRate)
        spectrumMathO2.initParams(bordFrequency, true)
        spectrumMathO2.setWavesCoeffs(deltaCoef, thetaCoef, alphaCoef, betaCoef, gammaCoef)

        spectrumMathT3 = SpectrumMath(samplingRate, fftWindow, processWinRate)
        spectrumMathT3.initParams(bordFrequency, true)
        spectrumMathT3.setWavesCoeffs(deltaCoef, thetaCoef, alphaCoef, betaCoef, gammaCoef)

        spectrumMathT4 = SpectrumMath(samplingRate, fftWindow, processWinRate)
        spectrumMathT4.initParams(bordFrequency, true)
        spectrumMathT4.setWavesCoeffs(deltaCoef, thetaCoef, alphaCoef, betaCoef, gammaCoef)
    }

    fun processSamples(samples: Map<SignalChannels, DoubleArray>) {
            try {
                val rawSpectrumData = mutableMapOf<SignalChannels, Array<RawSpectrumData>>()
                val wavesSpectrumData = mutableMapOf<SignalChannels, Array<WavesSpectrumData>>()

                for (signalChannel in samples.keys) {
                    when (signalChannel) {
                        SignalChannels.O1 -> {
                            spectrumMathO1.pushData(samples[signalChannel])

                            rawSpectrumData[signalChannel] = spectrumMathO1.readRawSpectrumInfoArr()
                            wavesSpectrumData[signalChannel] =
                                spectrumMathO1.readWavesSpectrumInfoArr()

                            spectrumMathO1.setNewSampleSize()
                        }
                        SignalChannels.O2 -> {
                            spectrumMathO2.pushData(samples[signalChannel])

                            rawSpectrumData[signalChannel] = spectrumMathO2.readRawSpectrumInfoArr()
                            wavesSpectrumData[signalChannel] =
                                spectrumMathO2.readWavesSpectrumInfoArr()

                            spectrumMathO2.setNewSampleSize()
                        }
                        SignalChannels.T3 -> {
                            spectrumMathT3.pushData(samples[signalChannel])

                            rawSpectrumData[signalChannel] = spectrumMathT3.readRawSpectrumInfoArr()
                            wavesSpectrumData[signalChannel] =
                                spectrumMathT3.readWavesSpectrumInfoArr()

                            spectrumMathT3.setNewSampleSize()
                        }
                        SignalChannels.T4 -> {
                            spectrumMathT4.pushData(samples[signalChannel])

                            rawSpectrumData[signalChannel] = spectrumMathT4.readRawSpectrumInfoArr()
                            wavesSpectrumData[signalChannel] =
                                spectrumMathT4.readWavesSpectrumInfoArr()

                            spectrumMathT4.setNewSampleSize()
                        }
                    }
                }

                processedSpectrum(rawSpectrumData.toMap())
                processedWaves(wavesSpectrumData.toMap())
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }

    fun finish() {
        spectrumMathO1.clearData()
        spectrumMathO2.clearData()
        spectrumMathT3.clearData()
        spectrumMathT4.clearData()
    }
}
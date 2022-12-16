import {EventSubscription} from 'react-native';
import EventEmitter from 'react-native/Libraries/vendor/emitter/EventEmitter';
import {Scanner} from 'react-native-neurosdk2';
import {SensorFamily, SensorInfo} from 'react-native-neurosdk2/Types';

class ScannerImpl {
  private TAG: string = 'ScannerImpl';
  private _sensorFoundedEvent: string = 'sensorFounded';
  public scanner: Scanner;
  private _scannerEventEmitter: EventEmitter;

  private _searching: Boolean = false;
  constructor() {
    this._scannerEventEmitter = new EventEmitter();
  }
  CreateScanner() {
    if (this.scanner != null) {
      this.RemoveScanner();
    }

    if (this.scanner == null) {
      this.scanner = new Scanner([
        SensorFamily.LECallibri,
        SensorFamily.LEKolibri,
        SensorFamily.LEBrainBit,
        SensorFamily.LEBrainBitBlack,
        SensorFamily.LEHeadPhones,
        SensorFamily.LEHeadband,
      ]);
    }
    this.scanner.Start();
  }

  RemoveScanner() {
    this.scanner?.Close();
    this.scanner = null;
  }

  StartSearch() {
    if (this.scanner == null) {
      this.CreateScanner();
    }
    try {
      this.scanner.AddSensorListChanged(sensorFounded => {
        this._scannerEventEmitter.emit(this._sensorFoundedEvent, sensorFounded);
      });
      this.scanner.Start(); // crash from same thread as device
      this._searching = true;
    } catch (ex) {
      console.log(ex);
      this._searching = false;
    }
  }
  sensorFounded(
    callback: (sensorFounded: SensorInfo[]) => void,
  ): EventSubscription {
    return this._scannerEventEmitter.addListener(
      this._sensorFoundedEvent,
      callback,
    );
  }
  StopSearch() {
    try {
      this.scanner?.RemoveSensorListChanged();
      this.scanner?.Stop();
      this.RemoveScanner();
      this._searching = false;
    } catch (ex) {
      console.log(ex);
      this._searching = false;
    }
  }

  Close() {
    this._searching = false;
    this.scanner.RemoveSensorListChanged();
    this._scannerEventEmitter.removeAllListeners();
    this.RemoveScanner();
  }
}
export default ScannerImpl;

using NeuroDemo.Utils;
using NeuroSDK;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NeuroDemo.NeuroImpl
{
    public class DevScanner
    {
        public Scanner scanner { get; private set; }

        public Action<IReadOnlyList<SensorInfo>> sensorFounded;

        private void CreateScanner()
        {
            if (scanner != null)
            {
                RemoveScanner();
            }

            ISensorHelper sensorHelper = DependencyService.Get<ISensorHelper>();
            sensorHelper.EnableSensor((enabled) => {
                if (enabled)
                {
                    if (scanner == null)
                    {
                        scanner = new Scanner(SensorFamily.SensorLECallibri, SensorFamily.SensorLEKolibri,
                            SensorFamily.SensorLEBrainBit, SensorFamily.SensorLEBrainBitBlack,
                            SensorFamily.SensorLEHeadPhones, SensorFamily.SensorLEHeadband);
                    }
                    scanner.EventSensorsChanged += Scanner_Founded;
                    scanner?.Start();
                }
            });
        }

        private void RemoveScanner()
        {
            scanner?.Dispose();
            scanner = null;
        }

        public void StartSearch()
        {
            if (scanner == null)
            {
                CreateScanner();
            }
            else
            {
                try
                {
                    scanner.EventSensorsChanged += Scanner_Founded;
                    scanner?.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void StopSearch()
        {
            try
            {
                scanner.EventSensorsChanged -= Scanner_Founded;
                scanner?.Stop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Scanner_Founded(IScanner scanner, IReadOnlyList<SensorInfo> sensors)
        {
            foreach(SensorInfo sensorInfo in sensors){
                Console.WriteLine(sensorInfo.Name + ": " + sensorInfo.Address);
            }
            if (sensors?.Count > 0)
            {
                sensorFounded?.Invoke(sensors);
            }
        }
        ~DevScanner()
        {
            Close();
        }

        public void Close()
        {
            if (scanner != null)
            {
                scanner.EventSensorsChanged -= Scanner_Founded;
            }
            RemoveScanner();
        }
    }

}

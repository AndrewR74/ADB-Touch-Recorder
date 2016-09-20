using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AdbTouchGenerator
{
    class Program
    {
        const string EVENT_LOCATOR = "/dev/input/event2";
        const string POINT_LOCATOR = "ABS_MT_POSITION_";
        const string TOUCH_LOCATOR = "BTN_TOUCH";

        static void Main(string[] args)
        {
            string _filename = @"E:\GitHub\Projects\Device Loader\SwiftiumDeviceLoaders\bin\Debug\platform-tools\touches.txt", _deviceId = null, _outputFile = null;

            if (args.Length > 0)
            {
                _filename = args[0];

                if (args.Length >= 2)
                    _deviceId = args[1];

                if (args.Length >= 3)
                    _outputFile = args[2];
            }

            if (System.IO.File.Exists(_filename))
            {
                // Only Up, Down and positions of touches
                IEnumerable<string> _lines = System.IO.File.ReadAllLines(_filename, Encoding.UTF8).Where(l => l.IndexOf(EVENT_LOCATOR) > -1 && (l.IndexOf(TOUCH_LOCATOR) > -1 || l.IndexOf(POINT_LOCATOR) > -1));

                List<AdbEvent> _es = new List<AdbEvent>();
                AdbEvent _e = null;
                int _x = -1, _y = -1;

                foreach (string _l in _lines)
                {
                    string[] _p = null;

                    if (_l.IndexOf(TOUCH_LOCATOR) > -1)
                    {
                        // p[0] = /dev/input/event2: EV_KEY
                        // P[1] = BTN_TOUCH
                        // P[2] = DOWN

                        //if (_es.Count > 1)
                        //{
                        //    if (_x > -1 && _es[_es.Count - 1]._points.Count > 0)
                        //    {
                        //        _es[_es.Count - 1]._points.Add(new AdbEvent.AdbPointEvent() { _point = new Point(_x, _es[_es.Count - 1]._points[_es[_es.Count - 1]._points.Count - 1]._point.Y), _timestamp = -1 });
                        //        _x = -1;
                        //    }
                        //}

                        _p = _l.Split(new string[] { "       ", "            " }, StringSplitOptions.None);

                        if (_p[2].IndexOf("DOWN") > -1)
                            _e = new AdbEvent();
                        else if (_p[2].IndexOf("UP") > -1 && _e != null) {
                            _es.Add(_e);
                            _e = null;
                        }
                    }
                    else if (_l.IndexOf(POINT_LOCATOR) > -1)
                    {
                        // p[0] = /dev/input/event2: EV_ABS
                        // P[1] = ABS_MT_POSITION_X
                        // P[2] = 00000114

                        _p = _l.Split(new string[] { "       ", "    " }, StringSplitOptions.None);

                        if (_e != null)
                        {
                            if(_p[2].IndexOf("X") > -1)
                                _x =  Convert.ToInt32(_p[3].Replace(" ", ""), 16);
                            else if (_p[2].IndexOf("Y") > -1)
                                _y = Convert.ToInt32(_p[3].Replace(" ", ""), 16);

                            // Ignore this point because it is a single
                            if (_y > -1 && _x == -1)
                                _y = -1;

                            if (_x > -1 && _y > -1)
                            {
                                _e._points.Add(new AdbEvent.AdbPointEvent() { 
                                    _point = new Point(_x, _y), 
                                    _timestamp = decimal.Parse(_l.Substring(_l.IndexOf("[") + 1, (_l.IndexOf("]") - _l.IndexOf("[")) - 2).Replace(" ", ""))
                                });
                                _x = -1;
                                _y = -1;
                            }
                        }
                    }
                }

                StringBuilder _sb = new StringBuilder();

                foreach (AdbEvent _ae in _es)
                {
                    if (_ae._points.Count > 0)
                    {
                        _sb.AppendLine(_deviceId != null ? _ae.GetAdbSendCommand(_deviceId) : _ae.GetAdbSendCommand());
                        _sb.AppendLine("ping 192.0.2.1 -n 1 -w 250 >nul");
                    }
                }

                System.IO.File.WriteAllText(_outputFile != null ? _outputFile : "output.bat", _sb.ToString());
            }
            else
            {
                Console.WriteLine("File not found");
            }

            Console.ReadKey();
        }

        private class AdbEvent
        {
            public class AdbPointEvent
            {
                public Point _point { get; set; }
                public decimal _timestamp { get; set; }
            }

            public List<AdbPointEvent> _points { get; set; }

            public AdbEvent()
            {
                this._points = new List<AdbPointEvent>();
            }

            private bool _isSwipe()
            {
                if (this._points.Count > 1)
                {
                    int _maxDistanceX = Math.Abs(_points.Max(p => p._point.X) - _points.Min(p => p._point.X)),
                        _maxDistanceY = Math.Abs(_points.Max(p => p._point.Y) - _points.Min(p => p._point.Y));

                    if (_maxDistanceX > 75 || _maxDistanceY > 75)
                        return true;
                }
                return false;
            }

            public string GetAdbSendCommand()
            {
                if (!this._isSwipe())
                {
                    int _averageX = this._points[0]._point.X, //(this._points.Sum(p => p._point.X) / this._points.Count),
                        _averageY = this._points[0]._point.Y; //(this._points.Sum(p => p._point.Y) / this._points.Count);

                    return string.Format("adb shell input tap {0} {1}", _averageX, _averageY);
                }
                else
                {
                    return string.Format("adb shell input swipe {0} {1} {2} {3} {4}", this._points[0]._point.X, this._points[0]._point.Y, this._points[this._points.Count - 1]._point.X, this._points[this._points.Count - 1]._point.Y + 100, 1300);
                    // (int)((this._points[this._points.Count - 1]._timestamp - this._points[0]._timestamp) * 1000)
                }
            }

            public string GetAdbSendCommand(string deviceId)
            {
                if (!this._isSwipe())
                {
                    int _averageX = (this._points.Sum(p => p._point.X) / this._points.Count),
                        _averageY = (this._points.Sum(p => p._point.Y) / this._points.Count);

                    return string.Format("adb -s {0} shell input tap {1} {2}", deviceId, _averageX, _averageY);
                }
                else
                {
                    return string.Format("adb -s {0} shell input swipe {1} {2} {3} {4} {5}", deviceId, this._points[0]._point.X, this._points[0]._point.Y, this._points[this._points.Count - 1]._point.X, this._points[this._points.Count - 1]._point.Y + 100, 1300);
                    // (int)((this._points[this._points.Count - 1]._timestamp - this._points[0]._timestamp) * 1000)
                }
            }
        }
    }
}

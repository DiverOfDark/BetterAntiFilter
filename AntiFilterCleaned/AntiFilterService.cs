using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Timers;

namespace AntiFilterCleaned
{
    public class AntiFilterService
    {
        private readonly Timer _timer;
        private readonly HttpClient _hc;
        public List<IPNetwork> Networks = new();

        public AntiFilterService()
        {
            _hc = new HttpClient();
            _timer = new Timer();
            _timer.Interval = 1000 * 60 * 10;
            UpdateData(null, null);
            _timer.Elapsed += UpdateData;
            _timer.Start();
        }

        private async void UpdateData(object? sender, ElapsedEventArgs elapsedEventArgs)
        {
            var response = await _hc.GetStringAsync("https://antifilter.download/list/allyouneed.lst");
            var networks = response.Split("\n").Where(v=>!String.IsNullOrWhiteSpace(v)).Select(IPNetwork.Parse).ToList();
            networks.Sort();
            for (int i = 0; i < networks.Count - 1; i++)
            {
                var curr = networks[i];
                var curr2 = networks[i + 1];

                if (curr.Contains(curr2))
                {
                    networks.Remove(curr2);
                    i--;
                }
                else if (curr2.Contains(curr))
                {
                    networks.Remove(curr);
                    i--;
                }
            }

            Networks = networks;
        }
    }
}
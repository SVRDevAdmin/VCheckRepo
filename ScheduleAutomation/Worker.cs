using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ScheduleAutomation.Lib.Logic;
using ScheduleAutomation.Lib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VCheck.Interface.API;
using VCheck.Lib.Data.DBContext;
using VCheck.Lib.Data.Models;

namespace ScheduleAutomation
{
    public class Worker : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                var clinicID = ScheduleRepository.GetConfigurationInfoByKey("ClinicID");

                VCheckAPI vCheckAPI = new VCheckAPI();

                var schedulesString = await vCheckAPI.GetScheduleListNotSent(clinicID);
                var schedulesExtended = string.IsNullOrEmpty(schedulesString) ? new List<ScheduledTestModelExtended>() : JsonConvert.DeserializeObject<List<ScheduledTestModelExtended>>(schedulesString);
                DeviceModel device = ScheduleRepository.GetTargetAnalyzer();
                List<DeviceTypeModel> deviceTypes = ScheduleRepository.GetDeviceTypeList();

                if (device != null && schedulesExtended.Any())
                {
                    foreach (var schedule in schedulesExtended)
                    {
                        var selectedDevice = ScheduleRepository.GetAnalyzerByParameterAllowed(schedule.Parameters);

                        if (selectedDevice != null)
                        {
                            HL7.SendMessage(schedule.Schedule, device);
                        }
                    }
                }

                //Thread.Sleep(TimeSpan.FromMinutes(5));
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}

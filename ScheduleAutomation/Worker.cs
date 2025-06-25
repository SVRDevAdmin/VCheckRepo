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

                var twoWayCommDeviceType = ScheduleRepository.GetTwoWayCommunicationDeviceTypeList();
                var schedulesString = await vCheckAPI.GetScheduleListNotSent(clinicID);
                var schedulesExtended = string.IsNullOrEmpty(schedulesString) ? new List<ScheduledTestModelExtended>() : JsonConvert.DeserializeObject<List<ScheduledTestModelExtended>>(schedulesString);
                bool TwoWayDeviceExist = ScheduleRepository.TwoWayDeviceExist();
                //List<DeviceTypeModel> deviceTypes = ScheduleRepository.GetDeviceTypeList();

                if (TwoWayDeviceExist && schedulesExtended.Any())
                {
                    foreach (var schedule in schedulesExtended)
                    {
                        //var selectedDevice = ScheduleRepository.GetAnalyzerByParameterAllowed(schedule.Parameters);

                        string[] deviceList = schedule.IDAnalyzers.FirstOrDefault().Analyzers.Split(",");
                        DeviceModel targetDevice = null;
                        var targetDeviceType = twoWayCommDeviceType.AsEnumerable().Where(x => deviceList.Contains(x.TypeName));

                        if (targetDeviceType.Any())
                        {
                            targetDevice = ScheduleRepository.GetDeviceByAnalyzerList(targetDeviceType.Select(x => x.id).ToArray());

                            if (targetDevice != null)
                            {
                                HL7.SendMessage(schedule.IDAnalyzers, schedule.Schedule, targetDevice);
                            }
                        }
                        
                    }
                }

                //Thread.Sleep(TimeSpan.FromMinutes(5));
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
        }
    }
}

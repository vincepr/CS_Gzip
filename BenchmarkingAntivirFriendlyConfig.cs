using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Gzip
{
    internal class BenchmarkingAntivirFriendlyConfig : ManualConfig
    {
        public BenchmarkingAntivirFriendlyConfig()
        {
            AddJob(Job.MediumRun.WithToolchain(InProcessNoEmitToolchain.Instance));
        }
    }
}

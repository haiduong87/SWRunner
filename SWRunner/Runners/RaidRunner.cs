﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SWRunner.Runners
{
    class RaidRunner : AbstractRunner<RunnerConfig>
    {
        public RaidRunner(string logFile) : base(logFile, "",  null, null)
        {
        }

        public override void Collect()
        {
            throw new NotImplementedException();
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}

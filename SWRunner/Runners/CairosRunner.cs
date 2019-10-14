﻿using SWEmulator;
using SWRunner.Filters;
using SWRunner.Rewards;
using System;
using System.Diagnostics;
using System.Threading;

namespace SWRunner.Runners
{
    // TODO: Ensure to wait for each click.
    public class CairosRunner : AbstractRunner<CairosRunnerConfig>
    {
        CairosFilter Filter { get; set; }

        public CairosRunner(CairosFilter filter, string logFile, string fullLogFile, CairosRunnerConfig runnerConfig, 
            AbstractEmulator emulator, RunnerLogger logger) : base(logFile, fullLogFile, runnerConfig, emulator, logger)
        {
            Filter = filter;
            MinEnergyRequired = 8;
            MaxRunTime = new TimeSpan(0, 2, 0); //This would be overriden
        }
        public override void Run()
        {
            // 1. Check fail, do not revive. Jump to check refill
            // 2. Check run finish
            // 3. If not finish, wait 3-5s
            // 4. If finish, collect reward with filter
            // 5. Check for refill
            // 6. Start
            ModifiedTime = DateTime.Now;

            while (!Stop)
            {
                Debug.WriteLine("Checking run status ...");
                Thread.Sleep(3000);
                if (IsFailed())
                {
                    Debug.WriteLine("Run Failed");
                    SkipRevive();
                }
                else if (IsEnd())
                {
                    Debug.WriteLine("Collecting reward");
                    Collect();
                }
                else
                {
                    // Run is not completed yet
                    continue;
                }

                StartNewRun();
                break;
            }
        }

        public override void Collect()
        {
            // Random click twice to pop up the reward dialog
            Thread.Sleep(8000); // Wait for end animation
            Emulator.RandomClick();

            Thread.Sleep(2000); // wait for treasure box to pop up
            Emulator.RandomClick();

            Thread.Sleep(2000); // wait for reward to pop up


            RunResult runResult = Helper.GetRunResult(LogFile);
            Reward reward = Helper.GetReward(runResult);

            bool getReward = Filter.ShouldGet(reward);

            if (getReward)
            {
                switch (reward.Type)
                {
                    case RewardType.RUNE:
                        Emulator.Click(RunnerConfig.GetRunePoint);
                        break;
                    case RewardType.MYSTICAL_SCROLL:
                    case RewardType.SUMMON_STONE:
                        Emulator.PressEsc();
                        break;
                    default:
                        Emulator.Click(RunnerConfig.GetOtherPoint);
                        break;
                }
            }
            else
            {
                // Only Rune needs to be sold
                Emulator.Click(RunnerConfig.SellRunePoint);
                Thread.Sleep(1500); // Wait for confirmation dialog
                Emulator.Click(RunnerConfig.ConfirmSellRunePoint);
            }

            Logger.Log(runResult, reward, getReward);

            Thread.Sleep(3000); // Wait for server response
        }

    }
}

using System.Text;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

Console.OutputEncoding = Encoding.UTF8;

//BenchmarkSwitcher.FromAssembly(typeof(Program)assembly).Run(args, new DebugInProcessConfig());

BenchmarkRunner.Run<ReJsonBenchmark>();
//BenchmarkRunner.Run<DataStoreBenchmark>();
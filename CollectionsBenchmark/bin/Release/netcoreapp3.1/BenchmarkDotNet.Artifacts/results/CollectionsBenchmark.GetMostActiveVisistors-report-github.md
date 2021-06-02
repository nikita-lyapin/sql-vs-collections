``` ini

BenchmarkDotNet=v0.12.1, OS=ubuntu 20.04
Intel Core i5-6500 CPU 3.20GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.1.403
  [Host]     : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT
  Job-VRXCRD : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT

IterationCount=3  LaunchCount=1  WarmupCount=1  

```
|                Method |     Mean |    Error |   StdDev |
|---------------------- |---------:|---------:|---------:|
|              LinqJoin |       NA |       NA |       NA |
|   LinqJoin_LinkedList |       NA |       NA |       NA |
|             MergeJoin |       NA |       NA |       NA |
|            NestedLoop |       NA |       NA |       NA |
| NestedLoop_LinkedList |       NA |       NA |       NA |
|              DataLoad |       NA |       NA |       NA |
|              RunInSQL | 11.41 ms | 0.779 ms | 0.043 ms |

Benchmarks with issues:
  GetMostActiveVisistors.LinqJoin: Job-VRXCRD(IterationCount=3, LaunchCount=1, WarmupCount=1)
  GetMostActiveVisistors.LinqJoin_LinkedList: Job-VRXCRD(IterationCount=3, LaunchCount=1, WarmupCount=1)
  GetMostActiveVisistors.MergeJoin: Job-VRXCRD(IterationCount=3, LaunchCount=1, WarmupCount=1)
  GetMostActiveVisistors.NestedLoop: Job-VRXCRD(IterationCount=3, LaunchCount=1, WarmupCount=1)
  GetMostActiveVisistors.NestedLoop_LinkedList: Job-VRXCRD(IterationCount=3, LaunchCount=1, WarmupCount=1)
  GetMostActiveVisistors.DataLoad: Job-VRXCRD(IterationCount=3, LaunchCount=1, WarmupCount=1)

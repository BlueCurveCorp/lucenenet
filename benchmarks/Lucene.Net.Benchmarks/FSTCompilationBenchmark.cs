using BenchmarkDotNet.Attributes;
using Lucene.Net.Util;
using Lucene.Net.Util.Fst;
using System;
using System.Collections.Generic;
using Int64 = J2N.Numerics.Int64;

namespace Lucene.Net.Benchmarks;

[MemoryDiagnoser]
public class FSTCompilationBenchmark
{
    private string[] _sortedTerms;
    private FST<Int64> _fst;

    [Params(1000, 10000)]
    public int NumTerms { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(42);
        var terms = new HashSet<string>();
        while (terms.Count < NumTerms)
        {
            int len = rng.Next(4, 20);
            var span = new char[len];
            for (int j = 0; j < len; j++)
                span[j] = (char)('a' + rng.Next(26));
            terms.Add(new string(span));
        }
        _sortedTerms = new string[terms.Count];
        terms.CopyTo(_sortedTerms);
        Array.Sort(_sortedTerms, StringComparer.Ordinal);
    }

    [Benchmark]
    public void BuildFST()
    {
        var outputs = PositiveInt32Outputs.Singleton;
        var builder = new Builder<Int64>(FST.INPUT_TYPE.BYTE1, 0, 0, true, true, int.MaxValue, outputs, null, true, 15, true, 15);
        var scratch = new Int32sRef(10);

        for (int i = 0; i < _sortedTerms.Length; i++)
        {
            Lucene.Net.Util.Fst.Util.ToInt32sRef(new BytesRef(_sortedTerms[i]), scratch);
            builder.Add(scratch, (Int64)i);
        }

        _fst = builder.Finish();
    }
}

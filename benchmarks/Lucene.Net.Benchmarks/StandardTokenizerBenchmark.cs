using BenchmarkDotNet.Attributes;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Util;
using System;
using System.IO;

namespace Lucene.Net.Benchmarks;

[MemoryDiagnoser]
public class StandardTokenizerBenchmark
{
    private string _text;

    [Params(1000, 10000)]
    public int WordCount { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(42);
        var words = new string[WordCount];
        for (int i = 0; i < WordCount; i++)
        {
            int len = rng.Next(3, 12);
            var span = new char[len];
            for (int j = 0; j < len; j++)
                span[j] = (char)('a' + rng.Next(26));
            words[i] = new string(span);
        }
        _text = string.Join(" ", words);
    }

    [Benchmark]
    public int Tokenize()
    {
        var tokenizer = new StandardTokenizer(LuceneVersion.LUCENE_48, new StringReader(_text));
        ICharTermAttribute termAtt = tokenizer.GetAttribute<ICharTermAttribute>();
        tokenizer.Reset();
        int count = 0;
        while (tokenizer.IncrementToken())
        {
            _ = termAtt.ToString();
            count++;
        }
        tokenizer.End();
        tokenizer.Close();
        return count;
    }
}

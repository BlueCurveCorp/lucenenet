using BenchmarkDotNet.Attributes;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;

namespace Lucene.Net.Benchmarks;

[MemoryDiagnoser]
public class SegmentMergeBenchmark
{
    private RAMDirectory _directory;
    private IndexWriter _writer;

    [Params(10, 50)]
    public int NumSegments { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(42);
        _directory = new RAMDirectory();
        var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, new SimpleAnalyzer(LuceneVersion.LUCENE_48))
        {
            MergePolicy = new TieredMergePolicy()
        };
        _writer = new IndexWriter(_directory, config);

        for (int seg = 0; seg < NumSegments; seg++)
        {
            for (int i = 0; i < 100; i++)
            {
                var doc = new Document();
                doc.Add(new StringField("id", $"{seg}_{i}", Field.Store.YES));
                var span = new char[50];
                for (int j = 0; j < 50; j++)
                    span[j] = (char)('a' + rng.Next(26));
                doc.Add(new TextField("content", new string(span), Field.Store.NO));
                _writer.AddDocument(doc);
            }
            _writer.Commit();
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _writer?.Dispose();
        _directory?.Dispose();
    }

    [Benchmark]
    public void ForceMerge()
    {
        _writer.ForceMerge(1);
    }
}

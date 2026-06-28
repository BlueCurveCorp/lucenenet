using BenchmarkDotNet.Attributes;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;

namespace Lucene.Net.Benchmarks;

[MemoryDiagnoser]
public class IndexWriterBenchmark
{
    private RAMDirectory _directory;
    private IndexWriter _writer;
    private Document[] _documents;

    [Params(100, 1000)]
    public int NumDocuments { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var rng = new Random(42);
        _documents = new Document[NumDocuments];
        for (int i = 0; i < NumDocuments; i++)
        {
            var doc = new Document();
            doc.Add(new StringField("id", i.ToString(), Field.Store.YES));
            doc.Add(new TextField("title", $"Document number {i}", Field.Store.YES));
            doc.Add(new TextField("content", GenerateText(rng, 100), Field.Store.NO));
            _documents[i] = doc;
        }
    }

    [IterationSetup]
    public void SetupIteration()
    {
        _directory = new RAMDirectory();
        var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, new SimpleAnalyzer(LuceneVersion.LUCENE_48));
        _writer = new IndexWriter(_directory, config);
    }

    [IterationCleanup]
    public void CleanupIteration()
    {
        _writer?.Dispose();
        _directory?.Dispose();
    }

    [Benchmark(OperationsPerInvoke = 100)]
    public void AddDocument()
    {
        for (int i = 0; i < 100; i++)
            _writer.AddDocument(_documents[i % _documents.Length]);
    }

    private static string GenerateText(Random rng, int wordCount)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        var words = new string[wordCount];
        for (int i = 0; i < wordCount; i++)
        {
            int len = rng.Next(3, 10);
            var span = new char[len];
            for (int j = 0; j < len; j++)
                span[j] = chars[rng.Next(chars.Length)];
            words[i] = new string(span);
        }
        return string.Join(' ', words);
    }
}

using BenchmarkDotNet.Attributes;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;

namespace Lucene.Net.Benchmarks;

[MemoryDiagnoser]
public class IndexSearcherBenchmark
{
    private RAMDirectory _directory;
    private DirectoryReader _reader;
    private IndexSearcher _searcher;
    private TermQuery _termQuery;
    private MatchAllDocsQuery _matchAllQuery;

    [Params(1000, 10000)]
    public int NumDocuments { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _directory = new RAMDirectory();
        var config = new IndexWriterConfig(LuceneVersion.LUCENE_48, new SimpleAnalyzer(LuceneVersion.LUCENE_48));
        using var writer = new IndexWriter(_directory, config);

        var rng = new Random(42);
        for (int i = 0; i < NumDocuments; i++)
        {
            var doc = new Document();
            doc.Add(new StringField("id", i.ToString(), Field.Store.YES));
            doc.Add(new TextField("content", GenerateText(rng, 50), Field.Store.YES));
            doc.Add(new StringField("category", rng.Next(0, 10).ToString(), Field.Store.NO));
            writer.AddDocument(doc);
        }
        writer.Commit();

        _reader = DirectoryReader.Open(_directory);
        _searcher = new IndexSearcher(_reader);
        _termQuery = new TermQuery(new Term("category", "5"));
        _matchAllQuery = new MatchAllDocsQuery();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _reader?.Dispose();
        _directory?.Dispose();
    }

    [Benchmark]
    public int TermQuerySearch()
    {
        TopDocs results = _searcher.Search(_termQuery, 10);
        return results.TotalHits;
    }

    [Benchmark]
    public int MatchAllSearch()
    {
        TopDocs results = _searcher.Search(_matchAllQuery, 10);
        return results.TotalHits;
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

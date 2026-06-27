using System;
using System.Collections.Generic;

namespace Lucene.Net.Support
{
    /*
     * Licensed to the Apache Software Foundation (ASF) under one or more
     * contributor license agreements.  See the NOTICE file distributed with
     * this work for additional information regarding copyright ownership.
     * The ASF licenses this file to You under the Apache License, Version 2.0
     * (the "License"); you may not use this file except in compliance with
     * the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     */

    /// <summary>
    /// .NET Specific Helper Extensions for IEnumerable
    /// </summary>
    //Note: LUCENENET specific
    internal static class EnumerableExtensions
    {
        extension<T>(IEnumerable<T> source)
        {
            /// <summary>
            /// Enumerates a sequence in pairs
            /// </summary>
            /// <remarks>
            /// In the case of an uneven amount of elements, the list call to <paramref name="join" /> pases <code>default</code> as the second parameter.
            /// </remarks>
            /// <typeparam name="TOut">The type of the elements returned from <paramref name="join" />.</typeparam>
            /// <param name="join">A function that is invoked for each pair of elements.</param>
            /// <exception cref="ArgumentNullException"><paramref name="join" /> is <see langword="null" />.</exception>
            /// <returns>A new <see cref="T:System.Collections.Generic.IEnumerable`1" /> containing the results from each pair.</returns>
            public IEnumerable<TOut> InPairs<TOut>(Func<T, T, TOut> join)
            {
                if (source is null)
                    throw new ArgumentNullException(nameof(source));
                if (join is null)
                    throw new ArgumentNullException(nameof(join));

                using IEnumerator<T> enumerator = source.GetEnumerator();
                while (true)
                {
                    if (!enumerator.MoveNext())
                        yield break;

                    T x = enumerator.Current;
                    if (!enumerator.MoveNext())
                        yield return join(x, default);
                    yield return join(x, enumerator.Current);
                }
            }

            /// <summary>
            /// Take all but the last element of the sequence.
            /// </summary>
            /// <returns>The resulting <see cref="IEnumerable{T}"/>.</returns>
            public IEnumerable<T> TakeAllButLast()
            {
                if (source is null)
                    throw new ArgumentNullException(nameof(source));

                return EnumerableExtensionsImpl.TakeAllButLastImpl(source);
            }

            /// <summary>
            /// Take all but the last <paramref name="n"/> elements of the sequence.
            /// </summary>
            /// <param name="n">The number of elements at the end of the sequence to exclude.</param>
            /// <returns>The resulting <see cref="IEnumerable{T}"/>.</returns>
            public IEnumerable<T> TakeAllButLast(int n)
            {
                if (source is null)
                    throw new ArgumentNullException(nameof(source));

                if (n < 0)
                    throw new ArgumentOutOfRangeException(nameof(n),
                        "Argument n should be non-negative.");

                return EnumerableExtensionsImpl.TakeAllButLastImpl(source, n);
            }
        }
    }

    internal static class EnumerableExtensionsImpl
    {
        internal static IEnumerable<T> TakeAllButLastImpl<T>(IEnumerable<T> source)
        {
            T buffer = default;
            bool buffered = false;

            foreach (T x in source)
            {
                if (buffered)
                    yield return buffer;

                buffer = x;
                buffered = true;
            }
        }

        internal static IEnumerable<T> TakeAllButLastImpl<T>(IEnumerable<T> source, int n)
        {
            Queue<T> buffer = new Queue<T>(n + 1);

            foreach (T x in source)
            {
                buffer.Enqueue(x);

                if (buffer.Count == n + 1)
                    yield return buffer.Dequeue();
            }
        }
    }
}

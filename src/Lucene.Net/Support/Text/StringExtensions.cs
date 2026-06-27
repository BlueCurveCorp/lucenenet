using J2N.Text;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Lucene.Net.Support.Text
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
    /// Extensions to <see cref="string"/>.
    /// </summary>
    internal static class StringExtensions
    {
        extension(string s)
        {
            /// <summary>
            /// Returns <c>true</c> if <paramref name="s"/> contains any character from <paramref name="charsToCompare"/>.
            /// </summary>
            /// <param name="charsToCompare">An array of characters to check.</param>
            /// <returns><c>true</c> if any <paramref name="charsToCompare"/> are found; otherwise, <c>false</c>.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool ContainsAny(char[] charsToCompare)
            {
                if (s is null)
                    throw new ArgumentNullException(nameof(s));
                if (charsToCompare is null)
                    throw new ArgumentNullException(nameof(charsToCompare));

                // Ensure the strings passed don't contain invalid characters
                for (int i = 0; i < charsToCompare.Length; i++)
                {
                    if (s.Contains(charsToCompare[i]))
                        return true;
                }
                return false;
            }

            /// <summary>
            /// Returns <c>true</c> if <paramref name="s"/> is a valid, single path component for use in index
            /// file system access. A valid value is either a file name or a directory name, without any directory
            /// or volume separators.
            /// </summary>
            /// <returns><c>true</c> if <paramref name="s"/> is a valid path component;
            /// otherwise, <c>false</c>.</returns>
            public bool IsValidSinglePathComponent
            {
                get
                {
                    // NOTE: `s == Path.GetFileName(s)` ensures there are no directory components, such as
                    // directory separators or volume names. This works for both file and folder names, despite the use of
                    // Path.GetFileName() for the latter. If `s` ends with a directory separator, it's invalid anyway.
                    // Check IndexOfAny before Path.GetFileName: on .NET Framework, Path.GetFileName throws
                    // ArgumentException for strings containing NUL or other characters that are illegal in paths.
                    return !string.IsNullOrEmpty(s)
                           && s != "."
                           && s != ".."
                           && s.IndexOfAny(Path.GetInvalidFileNameChars()) < 0
                           && s.IndexOf('\\') < 0 // backslash is not an invalid character on Linux/macOS but is invalid for this purpose
                           && s == Path.GetFileName(s); // see NOTE above
                }
            }
        }
    }
}

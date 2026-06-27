using System;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Lucene
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
    /// The Java description is:
    /// Defines a general exception a servlet can throw when it encounters difficulty.
    /// <para/>
    /// This is a Java compatibility exception, and should be thrown in
    /// Lucene.NET everywhere Lucene throws it.
    /// </summary>

    internal class ServletException : Exception
    {
        [Obsolete("Use ServletException.Create() instead.", error: true)]
        public ServletException()
        {
        }

        [Obsolete("Use ServletException.Create() instead.", error: true)]
        public ServletException(string message) : base(message)
        {
        }

        [Obsolete("Use ServletException.Create() instead.", error: true)]
        public ServletException(string message, Exception innerException) : base(message, innerException)
        {
        }

        [Obsolete("Use ServletException.Create() instead.", error: true)]
        public ServletException(Exception cause)
            : base(cause?.ToString(), cause)
        {
        }


        // Static factory methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Exception Create() => new HttpRequestException();


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Exception Create(string message) => new HttpRequestException(message);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Exception Create(string message, Exception innerException) => new HttpRequestException(message, innerException);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Exception Create(Exception cause) => new HttpRequestException(cause.Message, cause);
    }
}

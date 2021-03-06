// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Buffers;

namespace Microsoft.AspNetCore.SignalR.Internal.Formatters
{
    public static class BinaryMessageFormatter
    {
        public static void WriteLengthPrefix(long length, IBufferWriter<byte> output)
        {
            Span<byte> lenBuffer = stackalloc byte[5];

            var lenNumBytes = WriteLengthPrefix(length, lenBuffer);

            output.Write(lenBuffer.Slice(0, lenNumBytes));
        }

        public static int WriteLengthPrefix(long length, Span<byte> output)
        {
            // This code writes length prefix of the message as a VarInt. Read the comment in
            // the BinaryMessageParser.TryParseMessage for details.
            var lenNumBytes = 0;
            do
            {
                ref var current = ref output[lenNumBytes];
                current = (byte)(length & 0x7f);
                length >>= 7;
                if (length > 0)
                {
                    current |= 0x80;
                }
                lenNumBytes++;
            }
            while (length > 0);

            return lenNumBytes;
        }

        public static int LengthPrefixLength(long length)
        {
            var lenNumBytes = 0;
            do
            {
                length >>= 7;
                lenNumBytes++;
            }
            while (length > 0);

            return lenNumBytes;
        }
    }
}

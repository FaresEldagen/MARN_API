using System.Security.Cryptography;

namespace MARN_API.Services.Implementations
{
    public class OpenTimestampsProofReader
    {
        private static readonly byte[] DetachedHeaderPrefix =
        [
            0x00, 0x4f, 0x70, 0x65, 0x6e, 0x54, 0x69, 0x6d, 0x65, 0x73, 0x74, 0x61, 0x6d, 0x70, 0x73, 0x00,
            0x00, 0x50, 0x72, 0x6f, 0x6f, 0x66, 0x00, 0xbf, 0x89, 0xe2, 0xe8, 0x84, 0xe8, 0x92, 0x94,
            0x01,
            0x08
        ];

        private static readonly byte[] PendingAttestationTag = [0x83, 0xdf, 0xe3, 0x0d, 0x2e, 0xf9, 0x0c, 0x8e];
        private static readonly byte[] BitcoinBlockHeaderAttestationTag = [0x05, 0x88, 0x96, 0x0d, 0x73, 0xd7, 0x19, 0x01];

        public OpenTimestampsProofExtractionResult Extract(byte[] otsBytes, byte[]? originalFileBytes = null)
        {
            ValidateDetachedHeader(otsBytes);

            var fileHashBytes = otsBytes.Skip(DetachedHeaderPrefix.Length).Take(32).ToArray();
            if (fileHashBytes.Length != 32)
            {
                throw new InvalidOperationException("The supplied .ots file does not contain a valid detached file hash.");
            }

            var reader = new OtsReader(otsBytes, DetachedHeaderPrefix.Length + 32);
            var root = DeserializeTimestamp(reader, fileHashBytes);

            if (!reader.IsAtEnd)
            {
                throw new InvalidOperationException("The supplied .ots file contains unread trailing bytes.");
            }

            string? originalHashHex = null;
            bool? originalFileMatches = null;

            if (originalFileBytes is not null)
            {
                var originalHashBytes = SHA256.HashData(originalFileBytes);
                originalHashHex = ToLowerHex(originalHashBytes);
                originalFileMatches = CryptographicOperations.FixedTimeEquals(fileHashBytes, originalHashBytes);
            }

            var transactionIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var commitmentMerkleRoots = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var bitcoinBlockMerkleRoots = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var bitcoinBlockHeights = new HashSet<long>();
            var pendingCalendarUrls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            Traverse(root, transactionIds, commitmentMerkleRoots, bitcoinBlockMerkleRoots, bitcoinBlockHeights, pendingCalendarUrls, null);

            return new OpenTimestampsProofExtractionResult
            {
                FileHash = ToLowerHex(fileHashBytes),
                OriginalFileHash = originalHashHex,
                OriginalFileMatches = originalFileMatches,
                TransactionIds = transactionIds.OrderBy(x => x).ToList(),
                MerkleRoots = commitmentMerkleRoots.OrderBy(x => x).ToList(),
                BitcoinBlockMerkleRoots = bitcoinBlockMerkleRoots.OrderBy(x => x).ToList(),
                BitcoinBlockHeights = bitcoinBlockHeights.OrderBy(x => x).ToList(),
                PendingCalendarUrls = pendingCalendarUrls.OrderBy(x => x).ToList()
            };
        }

        private static void ValidateDetachedHeader(byte[] otsBytes)
        {
            if (otsBytes.Length < DetachedHeaderPrefix.Length + 32)
            {
                throw new InvalidOperationException("The supplied .ots file is too short to be a valid detached OpenTimestamps proof.");
            }

            if (!otsBytes.AsSpan(0, DetachedHeaderPrefix.Length).SequenceEqual(DetachedHeaderPrefix))
            {
                throw new InvalidOperationException("The supplied file does not look like a detached SHA-256 OpenTimestamps proof.");
            }
        }

        private static ParsedTimestamp DeserializeTimestamp(OtsReader reader, byte[] message)
        {
            var timestamp = new ParsedTimestamp(message);

            void ReadBranch(byte tag)
            {
                if (tag == 0x00)
                {
                    timestamp.Attestations.Add(DeserializeAttestation(reader));
                    return;
                }

                var operation = DeserializeOperation(reader, tag);
                var nextMessage = operation.Apply(message);
                var child = DeserializeTimestamp(reader, nextMessage);
                timestamp.Branches.Add(new ParsedBranch(operation, child));
            }

            var tag = reader.ReadByte();
            while (tag == 0xff)
            {
                ReadBranch(reader.ReadByte());
                tag = reader.ReadByte();
            }

            ReadBranch(tag);
            return timestamp;
        }

        private static ParsedAttestation DeserializeAttestation(OtsReader reader)
        {
            var attestationTag = reader.ReadBytes(8);
            var payload = reader.ReadVarBytes();

            if (attestationTag.SequenceEqual(PendingAttestationTag))
            {
                var uriBytes = DecodeVarBytes(payload);
                return new ParsedAttestation("pending", uri: System.Text.Encoding.UTF8.GetString(uriBytes));
            }

            if (attestationTag.SequenceEqual(BitcoinBlockHeaderAttestationTag))
            {
                var payloadReader = new OtsReader(payload, 0);
                return new ParsedAttestation("bitcoin-block-header", height: payloadReader.ReadVarUInt());
            }

            return new ParsedAttestation("unknown");
        }

        private static ParsedOperation DeserializeOperation(OtsReader reader, byte tag)
        {
            return tag switch
            {
                0x03 => new ParsedOperation("ripemd160", tag, null, ComputeRipemd160),
                0x08 => new ParsedOperation("sha256", tag, null, message => SHA256.HashData(message)),
                0xf0 => CreateBinaryOperation(tag, reader.ReadVarBytes(), prepend: false),
                0xf1 => CreateBinaryOperation(tag, reader.ReadVarBytes(), prepend: true),
                _ => throw new InvalidOperationException($"Unsupported OpenTimestamps operation tag 0x{tag:x2}.")
            };
        }

        private static ParsedOperation CreateBinaryOperation(byte tag, byte[] argument, bool prepend)
        {
            return new ParsedOperation(
                prepend ? "prepend" : "append",
                tag,
                argument,
                message => prepend ? argument.Concat(message).ToArray() : message.Concat(argument).ToArray());
        }

        private static byte[] DecodeVarBytes(byte[] bytes)
        {
            var reader = new OtsReader(bytes, 0);
            var value = reader.ReadVarBytes();
            if (!reader.IsAtEnd)
            {
                throw new InvalidOperationException("Unexpected bytes found after an OpenTimestamps var-bytes payload.");
            }

            return value;
        }

        private static byte[] ComputeRipemd160(byte[] message)
        {
            using var hash = IncrementalHash.CreateHash(new HashAlgorithmName("RIPEMD160"));
            hash.AppendData(message);
            return hash.GetHashAndReset();
        }

        private static void Traverse(
            ParsedTimestamp timestamp,
            ISet<string> transactionIds,
            ISet<string> commitmentMerkleRoots,
            ISet<string> bitcoinBlockMerkleRoots,
            ISet<long> bitcoinBlockHeights,
            ISet<string> pendingCalendarUrls,
            string? currentCommitmentMerkleRoot)
        {
            var nextCommitmentMerkleRoot = timestamp.Message.Length == 32
                ? ToLowerHex(timestamp.Message)
                : currentCommitmentMerkleRoot;

            foreach (var attestation in timestamp.Attestations)
            {
                if (attestation.Type == "bitcoin-block-header")
                {
                    bitcoinBlockMerkleRoots.Add(ToLowerHex(timestamp.Message.Reverse().ToArray()));
                    if (attestation.Height.HasValue)
                    {
                        bitcoinBlockHeights.Add(attestation.Height.Value);
                    }
                }

                if (attestation.Type == "pending" && !string.IsNullOrWhiteSpace(attestation.Uri))
                {
                    pendingCalendarUrls.Add(attestation.Uri);
                }
            }

            foreach (var branch in timestamp.Branches)
            {
                if (LooksLikeBitcoinTransaction(branch.Child.Message))
                {
                    transactionIds.Add(ComputeBitcoinTransactionId(branch.Child.Message));
                    if (!string.IsNullOrEmpty(nextCommitmentMerkleRoot))
                    {
                        commitmentMerkleRoots.Add(nextCommitmentMerkleRoot);
                    }
                }

                Traverse(branch.Child, transactionIds, commitmentMerkleRoots, bitcoinBlockMerkleRoots, bitcoinBlockHeights, pendingCalendarUrls, nextCommitmentMerkleRoot);
            }
        }

        private static bool LooksLikeBitcoinTransaction(byte[] bytes)
        {
            return TryParseBitcoinTransaction(bytes, out _);
        }

        private static string ComputeBitcoinTransactionId(byte[] transactionBytes)
        {
            if (!TryParseBitcoinTransaction(transactionBytes, out var transactionForTxId))
            {
                throw new InvalidOperationException("Failed to parse the Bitcoin transaction embedded in the OpenTimestamps proof.");
            }

            var firstHash = SHA256.HashData(transactionForTxId);
            var secondHash = SHA256.HashData(firstHash);
            Array.Reverse(secondHash);
            return ToLowerHex(secondHash);
        }

        private static bool TryParseBitcoinTransaction(byte[] bytes, out byte[] transactionForTxId)
        {
            transactionForTxId = Array.Empty<byte>();

            try
            {
                var offset = 0;
                if (bytes.Length < 10)
                {
                    return false;
                }

                offset += 4;
                var hasWitness = false;
                if (offset + 2 <= bytes.Length && bytes[offset] == 0x00 && bytes[offset + 1] != 0x00)
                {
                    hasWitness = true;
                    offset += 2;
                }

                var inputCount = ReadBitcoinVarInt(bytes, ref offset);
                if (inputCount == 0)
                {
                    return false;
                }

                for (ulong index = 0; index < inputCount; index++)
                {
                    EnsureAvailable(bytes, offset, 36);
                    offset += 36;
                    var scriptLength = ReadBitcoinVarInt(bytes, ref offset);
                    EnsureAvailable(bytes, offset, checked((int)scriptLength + 4));
                    offset += checked((int)scriptLength + 4);
                }

                var outputCount = ReadBitcoinVarInt(bytes, ref offset);
                if (outputCount == 0)
                {
                    return false;
                }

                for (ulong index = 0; index < outputCount; index++)
                {
                    EnsureAvailable(bytes, offset, 8);
                    offset += 8;
                    var scriptLength = ReadBitcoinVarInt(bytes, ref offset);
                    EnsureAvailable(bytes, offset, checked((int)scriptLength));
                    offset += checked((int)scriptLength);
                }

                if (hasWitness)
                {
                    for (ulong index = 0; index < inputCount; index++)
                    {
                        var itemCount = ReadBitcoinVarInt(bytes, ref offset);
                        for (ulong itemIndex = 0; itemIndex < itemCount; itemIndex++)
                        {
                            var itemLength = ReadBitcoinVarInt(bytes, ref offset);
                            EnsureAvailable(bytes, offset, checked((int)itemLength));
                            offset += checked((int)itemLength);
                        }
                    }
                }

                EnsureAvailable(bytes, offset, 4);
                offset += 4;

                if (offset != bytes.Length)
                {
                    return false;
                }

                transactionForTxId = hasWitness ? StripWitness(bytes) : bytes;
                return true;
            }
            catch
            {
                transactionForTxId = Array.Empty<byte>();
                return false;
            }
        }

        private static byte[] StripWitness(byte[] bytes)
        {
            var offset = 0;
            var output = new List<byte>(bytes.Length);

            output.AddRange(bytes.Take(4));
            offset += 4;
            offset += 2;

            var inputCountStart = offset;
            var inputCount = ReadBitcoinVarInt(bytes, ref offset);
            output.AddRange(bytes.AsSpan(inputCountStart, offset - inputCountStart).ToArray());

            for (ulong index = 0; index < inputCount; index++)
            {
                var inputStart = offset;
                offset += 36;
                var scriptLength = ReadBitcoinVarInt(bytes, ref offset);
                offset += checked((int)scriptLength + 4);
                output.AddRange(bytes.AsSpan(inputStart, offset - inputStart).ToArray());
            }

            var outputCountStart = offset;
            var outputCount = ReadBitcoinVarInt(bytes, ref offset);
            output.AddRange(bytes.AsSpan(outputCountStart, offset - outputCountStart).ToArray());

            for (ulong index = 0; index < outputCount; index++)
            {
                var txOutStart = offset;
                offset += 8;
                var scriptLength = ReadBitcoinVarInt(bytes, ref offset);
                offset += checked((int)scriptLength);
                output.AddRange(bytes.AsSpan(txOutStart, offset - txOutStart).ToArray());
            }

            for (ulong index = 0; index < inputCount; index++)
            {
                var itemCount = ReadBitcoinVarInt(bytes, ref offset);
                for (ulong itemIndex = 0; itemIndex < itemCount; itemIndex++)
                {
                    var itemLength = ReadBitcoinVarInt(bytes, ref offset);
                    offset += checked((int)itemLength);
                }
            }

            output.AddRange(bytes.AsSpan(offset, 4).ToArray());
            return output.ToArray();
        }

        private static ulong ReadBitcoinVarInt(byte[] bytes, ref int offset)
        {
            EnsureAvailable(bytes, offset, 1);
            var prefix = bytes[offset++];

            return prefix switch
            {
                < 0xfd => prefix,
                0xfd => ReadLittleEndian(bytes, ref offset, 2),
                0xfe => ReadLittleEndian(bytes, ref offset, 4),
                _ => ReadLittleEndian(bytes, ref offset, 8)
            };
        }

        private static ulong ReadLittleEndian(byte[] bytes, ref int offset, int length)
        {
            EnsureAvailable(bytes, offset, length);
            ulong value = 0;
            for (var index = 0; index < length; index++)
            {
                value |= (ulong)bytes[offset + index] << (8 * index);
            }

            offset += length;
            return value;
        }

        private static void EnsureAvailable(byte[] bytes, int offset, int count)
        {
            if (offset < 0 || count < 0 || offset + count > bytes.Length)
            {
                throw new InvalidOperationException("Unexpected end of Bitcoin transaction data.");
            }
        }

        private static string ToLowerHex(byte[] bytes) => Convert.ToHexString(bytes).ToLowerInvariant();

        public sealed class OpenTimestampsProofExtractionResult
        {
            public string FileHash { get; init; } = string.Empty;
            public string? OriginalFileHash { get; init; }
            public bool? OriginalFileMatches { get; init; }
            public List<string> TransactionIds { get; init; } = [];
            public List<string> MerkleRoots { get; init; } = [];
            public List<string> BitcoinBlockMerkleRoots { get; init; } = [];
            public List<long> BitcoinBlockHeights { get; init; } = [];
            public List<string> PendingCalendarUrls { get; init; } = [];
        }

        private sealed class ParsedTimestamp(byte[] message)
        {
            public byte[] Message { get; } = message;
            public List<ParsedAttestation> Attestations { get; } = [];
            public List<ParsedBranch> Branches { get; } = [];
        }

        private sealed record ParsedBranch(ParsedOperation Operation, ParsedTimestamp Child);

        private sealed class ParsedAttestation(string type, string? uri = null, long? height = null)
        {
            public string Type { get; } = type;
            public string? Uri { get; } = uri;
            public long? Height { get; } = height;
        }

        private sealed class ParsedOperation(string name, byte tag, byte[]? argument, Func<byte[], byte[]> apply)
        {
            public string Name { get; } = name;
            public byte Tag { get; } = tag;
            public byte[]? Argument { get; } = argument;
            public Func<byte[], byte[]> Apply { get; } = apply;
        }

        private sealed class OtsReader(byte[] source, int offset)
        {
            private int _offset = offset;

            public bool IsAtEnd => _offset == source.Length;

            public byte ReadByte()
            {
                EnsureAvailable(1);
                return source[_offset++];
            }

            public byte[] ReadBytes(int count)
            {
                EnsureAvailable(count);
                var bytes = source.AsSpan(_offset, count).ToArray();
                _offset += count;
                return bytes;
            }

            public byte[] ReadVarBytes()
            {
                var count = checked((int)ReadVarUInt());
                return ReadBytes(count);
            }

            public long ReadVarUInt()
            {
                long value = 0;
                var shift = 0;

                while (true)
                {
                    var current = ReadByte();
                    value |= (long)(current & 0x7f) << shift;
                    if ((current & 0x80) == 0)
                    {
                        return value;
                    }

                    shift += 7;
                    if (shift > 63)
                    {
                        throw new InvalidOperationException("OpenTimestamps varuint is too large to fit into Int64.");
                    }
                }
            }

            private void EnsureAvailable(int count)
            {
                if (_offset < 0 || count < 0 || _offset + count > source.Length)
                {
                    throw new InvalidOperationException("Unexpected end of OpenTimestamps proof data.");
                }
            }
        }
    }
}

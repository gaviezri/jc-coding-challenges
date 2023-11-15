using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_compression_tool.codec
{
    internal partial class Codec
    {
        private static readonly char seperator = ':', terminator = ';';
        private static readonly string endOfHeader = "\r\n\r\n\r\n";
        private static readonly int EOF = -1;
        private class Encoder
        {
            private BinaryWriter writer;
            private Stream stream;
            private string fPath;
            private char buffer;
            private bool isFlushed = false;
            private int bufferUsedBits = 0;

            public Encoder(string i_path, Stream i_stream)
            {
                fPath = i_path;
                string outputPath = fPath.Substring(0, fPath.LastIndexOf('.')) + ".hmc";
                writer = new BinaryWriter(new FileStream(outputPath, FileMode.Create));
                stream = i_stream;
            }

            public void Encode()
            {
                Dictionary<char, int> tokensFreq = createTokenDict();
                var huffmanTree = new HuffmanTree(tokensFreq);
                Dictionary<char, BitArray> codeTable = huffmanTree.GeneratePrefixCodeTable();
                saveEncoding(codeTable);
            }

            private Dictionary<char, int> createTokenDict()
            {
                Dictionary<char, int> tokensFreqDict = new();
                for (int i = 0; i < stream.Length; ++i)
                {
                    int currentByteRead = stream.ReadByte();
                    if (currentByteRead == -1)
                    {
                        break;
                    }
                    countToken(tokensFreqDict, currentByteRead);
                }
                return tokensFreqDict;
            }

            private static void countToken(Dictionary<char, int> tokensFreq, int currentByteRead)
            {
                char ch = (char)currentByteRead;
                if (tokensFreq.ContainsKey(ch))
                {
                    tokensFreq[ch] = (int)tokensFreq[ch] + 1;
                }
                else
                {
                    tokensFreq[ch] = 1;
                }
            }

            private void saveEncoding(Dictionary<char, BitArray> i_codeTable)
            {
                generateHeader(i_codeTable);
                encodeText(i_codeTable);
            }

            private void encodeText(Dictionary<char, BitArray> i_codeTable)
            {

                stream.Position = 0;
                for (int i = 0; i < stream.Length; i += 2)
                {
                    char? charRead;
                    readChar(out charRead);
                    if (charRead.HasValue)
                    {
                        encodeChar((char)charRead, i_codeTable);
                    }

                }

                writer.Close();
            }

            private void readChar(out char? charRead)
            {

                int firstByte = stream.ReadByte();
                if (firstByte == EOF)
                {
                    charRead = null;
                }
                else
                {
                    int secondByte = stream.ReadByte();
                    if (secondByte == EOF)
                    {
                        charRead = (char)firstByte;
                    }
                    else
                    {
                        byte[] bytes = new byte[2];
                        bytes[0] = (byte)firstByte;
                        bytes[1] = (byte)secondByte;
                        charRead = BitConverter.ToChar(bytes);
                    }
                }
            }

            private void encodeChar(char charToEncode, Dictionary<char, BitArray> i_codeTable)
            {
                BitArray encoding = i_codeTable[charToEncode];
                // 1. put current encoding bits inside buffer such as possible
                // 2. if buffer is full, write its content and zero it;
                // 3. if more encoding bits are need to be written, repeat 1

            }

            private void generateHeader(Dictionary<char, BitArray> codeTable)
            {
                //format (binary):
                //[ASCII 1-byte][length in bits 4-bytes][binary encoding 8-bytes]
                //where ASCII is the ascii representation of the character,
                //seperator will be the ascii code of seperator,
                //binary encoding will be a sequence of bits representing the character's encoding.
                //end of header represented by 3x [\r\n]

                foreach (var character in codeTable.Keys)
                {
                    writer.Write(character);
                    writer.Write(codeTable[character].Count);
                    ulong encodedChar = extractULongFromBitArray(codeTable[character]);
                    writer.Write(encodedChar);
                }
                writer.Write(Codec.endOfHeader);
            }

            private ulong extractULongFromBitArray(BitArray bitArray)
            {
                if (bitArray.Length > 64)
                {
                    throw new ArgumentException("Bit Array must be 64 or less");
                }

                var bytes = new byte[8];
                bitArray.CopyTo(bytes, 0);
                return BitConverter.ToUInt64(bytes, 0);
            }
        }

    }
}

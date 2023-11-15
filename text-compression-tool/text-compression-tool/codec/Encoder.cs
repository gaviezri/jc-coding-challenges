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
        private class Encoder
        {
            private BinaryWriter writer;
            private Stream stream;
            private string fPath;

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
                throw new NotImplementedException();
            }

            private void generateHeader(Dictionary<char, BitArray> codeTable)
            {
                //format (binary):
                //[ASCII][seperator][binary encoding][terminator]
                //where ASCII is the ascii representation of the character,
                //seperator will be the ascii code of seperator,
                //binary encoding will be a sequence of bits representing the character's encoding.
                //end of header represented by 3x [\r\n]

                foreach (var character in codeTable.Keys)
                {
                    writer.Write(character);
                    writer.Write(Codec.seperator);
                    
                }
            }
        }

    }
}

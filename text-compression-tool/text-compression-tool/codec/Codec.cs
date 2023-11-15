using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using text_compression_tool.args;

namespace text_compression_tool.codec
{
    internal partial class Codec
    {   
        
        private Codec() {
            encoder = null;
            decoder = null;
        }
        public static readonly Codec Instance = new Codec();
        public static Codec getInstance() { return Instance; }


        private ProcedureCommand procom;
        private Encoder encoder;
        private Decoder decoder;
        public void Convert(ProcedureCommand i_procom)
        {
            procom = i_procom;

            switch (i_procom.Procedure)
            {
                case Procedure.ENCODE:
                    encoder = new(i_procom.fPath, i_procom.Stream);
                    encoder.Encode();
                    break;
                case Procedure.DECODE:
                    break;
                case Procedure.ERROR:
                    Console.WriteLine(procom.ErrMsg);
                    break;
            }
        }

    }
}

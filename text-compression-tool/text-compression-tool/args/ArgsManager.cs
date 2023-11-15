using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace text_compression_tool.args
{
    internal enum Procedure
    {
        ENCODE,
        DECODE,
        ERROR
    }

    internal class ProcedureCommand
    {
        public String fPath
        {
            get; set;
        }

        public Stream? Stream
        {
            get; set;
        }
        public Procedure Procedure
        {
            get; set;
        }
        public String? ErrMsg { get; set; }

    }
    internal class ArgsManager
    {
        private ArgsManager() { }

        private static void printManual()
        {
            Console.WriteLine("switches comes first, filepath comes last.\n" +
                                 "-e = encoding (default), -d = decoding.\n" +
                                 "lastly, state the file path.");
        }
        internal static ProcedureCommand Create(String[] i_args)
        {
            ProcedureCommand procom = new ProcedureCommand();

            if (i_args.Length > 3)
            {
                Console.WriteLine("Too many positional arguments, please be precise");
                printManual();
            }

            if (i_args[0].Equals("-h") || i_args == null)
            {
                printManual();
                return null;
            }
            else if (i_args[0].Equals("-e"))
            {
                procom.Procedure = Procedure.ENCODE;
            }
            else if (i_args[0].Equals("-d"))
            {
                procom.Procedure = Procedure.DECODE;
            }

            if (File.Exists(i_args[1]))
            {
                procom.Stream = File.OpenRead(i_args[1]);
                procom.fPath = i_args[1];
                procom.ErrMsg = String.Empty;
            }
            else
            {
                procom.Procedure = Procedure.ERROR;
                procom.ErrMsg = "File not found";
            }

            return procom;
        }

    }
}

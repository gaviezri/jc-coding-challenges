using text_compression_tool.args;
using text_compression_tool.codec;

public class Program
{
    
    public static void Main(String[] args)
    {
       ProcedureCommand procom = ArgsManager.Create(args);
        if(procom !=null)
        {
            Codec.getInstance().Convert(procom);
        }
    }
}
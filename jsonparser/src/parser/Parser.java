package parser;

public class Parser {
    private static boolean NotAValidJsonValue(String value){
        boolean isString = value.startsWith("\"") && value.endsWith("\"");
        boolean isNumber = value.matches("-?\\d+(\\.\\d+)?");
        boolean isBoolean = value.equals("true") || value.equals("false");
        boolean isNull = value.equals("null");
        return ! (isString || isNumber || isBoolean || isNull);
    }

    public static int parse(String input) {
        String whitespacesRemoved = input.replaceAll("\\s+", "");
        String outermostObject = whitespacesRemoved.substring(1, whitespacesRemoved.lastIndexOf("}"));
        String[] objectProperties = outermostObject.split(",");
        for (String prop : objectProperties) {
            String[] seperatedKV = prop.split(":");
            String key = seperatedKV[0];
            String value = seperatedKV[1];
            if ( ! key.startsWith("\"") || ! key.endsWith("\"")) {
                return 1;
            }
            if (NotAValidJsonValue(value)) {
                return 1;
            }

        }
        return 0;
    }
}

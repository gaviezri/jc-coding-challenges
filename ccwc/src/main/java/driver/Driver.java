package driver;

import count.strategy.CountStrategy;
import count.strategy.CountStrategyImpl;

import java.nio.file.Path;
import java.util.Arrays;

public class Driver {
    public static void instructionMessage(){
        System.out.println("Provide a path to a file from which to count.\n Available flags: -c (bytes), -w (words), -l (lines), -m (characters), no switch (all)." +
                "ccwc [-c | -w | -l | -m] pathtofile.txt (supports multiple flags)");
    }
    public Driver(String[] args) {
        Long result[] = null;
        if (args.length == 0) {
            instructionMessage();
        } else {
            if (! java.nio.file.Files.exists(Path.of(args[0]))){
                System.out.println("Error: File doesn't exist.");
                return;
            }
            if (args.length == 1) {
                System.out.println("No flag provided, defaulting to all.");
                result =  CountStrategyImpl.ALL.count(args[0]);
            } else if (args.length == 2) {
                switch (args[0]) {
                    case "-c":
                      result =  CountStrategyImpl.BYTE.count(args[1]);
                        break;
                    case "-w":
                        result = CountStrategyImpl.WORD.count(args[1]);
                        break;
                    case "-l":
                        result = CountStrategyImpl.LINE.count(args[1]);
                        break;
                    case "-m":
                        result = CountStrategyImpl.CHAR.count(args[1]);
                        break;
                    default:
                        System.out.println("Invalid flag provided, defaulting to all.");
                        result = CountStrategyImpl.ALL.count(args[0]);
                        break;
                }

            }
            System.out.println( result == null ? "Error: No result." : "Result: " + Arrays.stream(result).map(Object::toString).reduce("", (a, b) -> a + " " + b));;
        }
    }
}
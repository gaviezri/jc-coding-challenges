package driver;

import count.strategy.CountStrategy;
import count.strategy.CountStrategyImpl;

import java.nio.file.Path;
import java.util.Arrays;
import java.util.Collection;
import java.util.LinkedList;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.stream.Stream;

public class Driver {
    public static void instructionMessage(){
        System.out.println("Provide a path to a file from which to count.\n Available flags: -c (bytes), -w (words), -l (lines), -m (characters), no switch (all)." +
                "ccwc [-c | -w | -l | -m] pathtofile.txt (supports multiple flags)");
    }
    public Driver(String[] args) {
        Collection<Long> result;
        if (args.length == 0) {
            result = new LinkedList<Long>();
            instructionMessage();
        } else {
            if (! java.nio.file.Files.exists(Path.of(args[0]))){
                System.out.println("Error: File doesn't exist.");
                return;
            }
            if (args.length == 1) {
                System.out.println("No flag provided, defaulting to all.");
                result = CountStrategyImpl.ALL.count(args[0]);
            } else {
                result = new LinkedList<>();
                if (args.length > 2) {
                    // reorder args to put the file path at the end
                    AtomicInteger flagIndex = new AtomicInteger(0);
                    String[] reorderedArgs = new String[args.length];
                    Arrays.stream(args).filter(arg -> arg.length()==2 && arg.charAt(0) == '-').forEach(arg ->
                            reorderedArgs[flagIndex.getAndIncrement()] = arg);
                    Arrays.stream(args).filter(arg -> arg.length() > 2).forEach(arg -> reorderedArgs[reorderedArgs.length-1] = arg);
                    //count
                    Stream<CountStrategy> counters = CountStrategy.getCounters(reorderedArgs);

                    counters.forEach(counter -> {
                      result.addAll(counter.count(reorderedArgs[reorderedArgs.length-1]));

                    });

                }
            }
            System.out.println( result == null ? "Error: No result." : "Result: " + result.stream().map(Object::toString).reduce("", (a, b) -> a + " " + b));;
        }
    }
}
package count.strategy;

import java.util.Collection;
import java.util.stream.Stream;

public interface CountStrategy{
    Collection<Long> count(String path);

    static Stream<CountStrategy> getCounters(String[] flags){
        return Stream.of(flags).map(CountStrategy::getCounter);
    }
    static CountStrategy getCounter(String flag){
        CountStrategy result;
        switch (flag) {
            case "-c":
                result =  CountStrategyImpl.BYTE;
            case "-w":
                result = CountStrategyImpl.WORD;
            case "-l":
                result = CountStrategyImpl.LINE;
            case "-m":
                result = CountStrategyImpl.CHAR;
            default:
                result= CountStrategyImpl.ALL;
        }
        return result;
    }
}

package count.strategy;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.Collection;
import java.util.LinkedList;
import java.util.List;
import java.util.stream.Stream;
public enum CountStrategyImpl implements CountStrategy{
    BYTE{
        @Override
        public Collection<Long> count(String path) {
            try {
                return List.of( Files.size(Path.of(path)));
            } catch (Exception e) {
                System.out.println("Error: " + e.getMessage() + "Doesn't exist or is not a file.");
                return null;
            }
        }
    },
    WORD{
        @Override
        public Collection<Long> count(String path) {
            Stream<String> lines = utils.getLines(Path.of(path));
            if (lines != null) {
              return List.of(lines.flatMap(line -> Stream.of(line.split("\\s+"))).count());
            }
            return null;
        }
    },
    LINE{
        @Override
        public Collection<Long> count(String path) {
            Stream<String> lines = utils.getLines(Path.of(path));
            if (lines != null) {
                return List.of(lines.count());
            }
            return null;
        }
    },
    CHAR{
        @Override
        public Collection<Long> count(String path) {
            Stream<String> lines = utils.getLines(Path.of(path));
            if (lines != null) {
               return List.of(lines.map(line -> (long) line.length()).reduce(0L, Long::sum));
            }
            return null;
        }},
    ALL{
        @Override
        public Collection<Long>  count(String path) {
            Collection<Long> result = new LinkedList<>();
            result.addAll(CountStrategyImpl.BYTE.count(path));
            result.addAll(CountStrategyImpl.LINE.count(path));
            result.addAll(CountStrategyImpl.WORD.count(path));
            result.addAll(CountStrategyImpl.CHAR.count(path));
            return result;
        }
    }
}




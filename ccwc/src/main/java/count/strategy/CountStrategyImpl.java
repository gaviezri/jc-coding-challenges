package count.strategy;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.stream.Stream;
public enum CountStrategyImpl implements CountStrategy{
    BYTE{
        @Override
        public Long[] count(String path) {
            Long[] result = new Long[1];
            try {
                result[0] = Files.size(Path.of(path));
            } catch (Exception e) {
                System.out.println("Error: " + e.getMessage() + "Doesn't exist or is not a file.");
                result = null;
            }
            return result;
        }
    },
    WORD{
        @Override
        public Long[] count(String path) {
            Stream<String> lines = utils.getLines(Path.of(path));
            Long[] result = new Long[1];
            if (lines != null) {
                result[0] = lines.flatMap(line -> Stream.of(line.split("\\s+"))).count();
            }
            return result;
        }
    },
    LINE{
        @Override
        public Long[] count(String path) {
            Stream<String> lines = utils.getLines(Path.of(path));
            Long[] result = new Long[1];
            if (lines != null) {
                result[0] = lines.count();
            }
            return result;
        }
    },
    CHAR{
        @Override
        public Long[] count(String path) {
            Stream<String> lines = utils.getLines(Path.of(path));
            Long[] result = new Long[1];
            if (lines != null) {
                result[0] = lines.map(line -> (long) line.length()).reduce(0L, Long::sum);
            }
            return result;
        }},
    ALL{
        @Override
        public Long[] count(String path) {
            Long[] result = new Long[4];
            result[0] = CountStrategyImpl.BYTE.count(path)[0];
            result[1] = CountStrategyImpl.LINE.count(path)[0];
            result[2] = CountStrategyImpl.WORD.count(path)[0];
            result[3] = CountStrategyImpl.CHAR.count(path)[0];
            return result;
        }
    }
}




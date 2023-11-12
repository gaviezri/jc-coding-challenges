package count.strategy;

import java.nio.file.Path;
import java.util.stream.Stream;

public class utils {
    public static Stream<String> getLines(Path path) {
        try {
            return java.nio.file.Files.lines(path);
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }
}

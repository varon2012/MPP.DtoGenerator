using System.Collections.Generic;

namespace DtoGeneratorTest.Writers
{
    public interface IClassWriter
    {
        void Write(List<DtoGenerator.GenerationResult> classes, string directory);
    }
}

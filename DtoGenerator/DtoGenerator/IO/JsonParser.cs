using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DtoGenerator.IO
{
    internal sealed class JsonParser<T> : IFileParser<T>
    {
        private readonly JsonSerializer _serializer;

        public JsonParser()
        {
            _serializer = new JsonSerializer();
        }

        public IEnumerable<T> Parse(string filename)
        {
            IEnumerable<T> result;

            var fillpath = Path.GetFullPath(filename);

            try
            {
                using (var file = File.OpenText(fillpath))
                {
                    result = _serializer.Deserialize<IEnumerable<T>>(new JsonTextReader(file));
                }
            }
            catch (FileNotFoundException)
            {
                throw new BadInputException($"Input file '{fillpath}' not found");
            }
            catch (IOException e)
            {
                throw new BadInputException($"Error occurred while trying to access '{fillpath}': {e.Message}");
            }
            catch (JsonReaderException e)
            {
                throw new BadInputException($"Error occurred while reading '{fillpath}': {e.Message}");
            }
            catch (JsonSerializationException e)
            {
                throw new BadInputException($"Error occurred while parsing '{fillpath}': {e.Message}");
            }
            catch (JsonException e)
            {
                throw new BadInputException($"Error occurred while working with '{fillpath}': {e.Message}");
            }

            return result;
        }
    }
}

// script for www.json-generator.com
[
  '{{repeat(100)}}',
  {
    className: 'ExampleClass{{index()}}',
    properties: [
      '{{repeat(2, 10)}}',
      {
        name: 'Property{{index()}}',
        type: '{{random("integer", "number", "string", "boolean", "Class1", "MyCustomType", "MyCustomTypeWithFormats")}}',
        format: function (tags) {
          var formats = {
            integer: [
              'int32',
              'int64'
            ],
            number: [
              'float',
              'double'
            ],
            string: [
              'date',
              'string'
            ],
            MyCustomTypeWithFormats: [
              'Format1',
              'Format2',
              'Format3'
            ]
          };

          var currentTypeFormats = formats[this.type];

          return (currentTypeFormats)
              ? currentTypeFormats[tags.integer(0, currentTypeFormats.length - 1)]
              : undefined;
        }
      }
    ]
  }
]

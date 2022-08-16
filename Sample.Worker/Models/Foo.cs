using System.Collections.Generic;

namespace Sample.Worker.Models
{
    public class Foo
    {
        private string _id;
        private string _name;
        private string _motherName;
        private readonly IReadOnlyList<Bar> _bars;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string MotherName
        {
            get => _motherName;
            set => _motherName = value;
        }

        public IReadOnlyList<Bar> Bars
        {
            get => _bars;
            init => _bars = value;
        }
    }
}
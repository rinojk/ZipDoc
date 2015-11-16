using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipDocModel
{
    public class Document
    {
        private long _id;
        private string _name;
        private string _description;
        private long _size;
        private string _author;
        private string _path;
        private byte[] _hash;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public long Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public byte[] Hash
        {
            get { return _hash; }
            set { _hash = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public override string ToString()
        {
            return $"Document {_name} {_description}, Size {_size}";
        }
    }
}

namespace Db4oDoc.Code.Strategies.StoringStatic
{
    // #example: Class as enumeration
    public sealed class Color
    {
        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color White = new Color(255, 255, 255);
        public static readonly Color Red = new Color(255, 0, 0);
        public static readonly Color Green = new Color(0, 255, 0);
        public static readonly Color Blue = new Color(0, 0, 255);

        private readonly int red;
        private readonly int green;
        private readonly int blue;

        private Color(int red, int green, int blue)
        {
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public int RedValue
        {
            get { return red; }
        }

        public int GreenValue
        {
            get { return green; }
        }

        public int BlueValue
        {
            get { return blue; }
        }

        public bool Equals(Color other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.red == red && other.green == green && other.blue == blue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Color)) return false;
            return Equals((Color) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = red;
                result = (result*397) ^ green;
                result = (result*397) ^ blue;
                return result;
            }
        }

        public override string ToString()
        {
            return string.Format("Red: {0}, Green: {1}, Blue: {2}", red, green, blue);
        }
    }
    // #end example
}
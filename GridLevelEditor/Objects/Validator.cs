using System.Text.RegularExpressions;

namespace GridLevelEditor.Objects
{
    class Validator
    {
        Regex intString;

        public Validator()
        {
            intString = new Regex(@"^[0-9]*$");
        }

        public string IntString(string newValue, string oldValue)
        {
            if(intString.IsMatch(newValue))
            {
                return newValue;
            }
            return oldValue;
        }
    }
}

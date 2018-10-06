using System;

[Serializable]

internal class NumberException : Exception {

    public NumberException () 
    {
    }

    public NumberException (string message) : base(message)
    {
    }

}
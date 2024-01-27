using System;
public static class Precondition
{
    public static void CheckNotNull(object o)
    {
        if (o == null)
            throw new Exception("Object is null");
    }

    public static void CheckNotNull(object o, string message)
    {
        if (o == null)
            throw new Exception("Object is null. " + message);
    }
}
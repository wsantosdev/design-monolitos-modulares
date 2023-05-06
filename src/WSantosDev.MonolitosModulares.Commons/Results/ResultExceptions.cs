namespace WSantosDev.MonolitosModulares.Commons.Results
{
    public static class ResultExceptions
    {
        public static NullReferenceException NoErrorAvailable =>
            new("There is no error available for this result.");

        public static NullReferenceException NoValueAvailable =>
            new("There is no value available for this result.");
    }
}

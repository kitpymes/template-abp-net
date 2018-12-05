namespace Manager.Application
{
    public enum ResultType : byte
    {
        Success = 1,
        Error = 2,
        EmailAddressNotFindInSystem = 3,
        WaitingForActivationEmail = 4,
        WaitingForActivationMessage = 5,
        ResetCodeExpired = 6,
        InvalidPasswordResetCode = 7
    }
}
